using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using Grasshopper.Kernel;

using Python.Runtime;

using Tunny.Optimization;
using Tunny.Settings;
using Tunny.UI;
using Tunny.Util;

namespace Tunny.Solver
{
    public class Optuna
    {
        public double[] XOpt { get; private set; }
        private double[] FxOpt { get; set; }

        private readonly string _componentFolder;

        public Optuna(string componentFolder)
        {
            _componentFolder = componentFolder;
            string envPath = PythonInstaller.GetEmbeddedPythonPath() + @"\python310.dll";
            Environment.SetEnvironmentVariable("PYTHONNET_PYDLL", envPath, EnvironmentVariableTarget.Process);
        }

        public bool RunSolver(
            List<Variable> variables,
            IEnumerable<IGH_Param> objectives,
            Func<IList<decimal>, int, EvaluatedGHResult> evaluate,
            TunnySettings settings)
        {
            int dVar = variables.Count;
            double[] lb = new double[dVar];
            double[] ub = new double[dVar];
            bool[] isInteger = new bool[dVar];
            string[] varNickName = new string[dVar];
            string[] objNickName = objectives.Select(x => x.NickName).ToArray();

            for (int i = 0; i < dVar; i++)
            {
                lb[i] = Convert.ToDouble(variables[i].LowerBond);
                ub[i] = Convert.ToDouble(variables[i].UpperBond);
                isInteger[i] = variables[i].IsInteger;
                varNickName[i] = variables[i].NickName;
            }

            EvaluatedGHResult Eval(double[] x, int progress)
            {
                var decimals = x.Select(Convert.ToDecimal).ToList();
                return evaluate(decimals, progress);
            }

            try
            {
                var tpe = new OptunaAlgorithm(lb, ub, varNickName, objNickName, settings, Eval);
                tpe.Solve();
                XOpt = tpe.GetXOptimum();
                FxOpt = tpe.GetFxOptimum();

                TunnyMessageBox.Show("Solver completed successfully.", "Tunny");

                return true;
            }
            catch (Exception e)
            {
                TunnyMessageBox.Show(
                    "Tunny runtime error:\n" +
                    "Please send below message (& gh file if possible) to Tunny support.\n\n" +
                    "\" " + e.Message + " \"", "Tunny",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public void ShowSelectedTypePlot(string visualize, string studyName)
        {
            string storage = "sqlite:///" + _componentFolder + "/Tunny_Opt_Result.db";
            PythonEngine.Initialize();
            using (Py.GIL())
            {
                dynamic study;
                dynamic optuna = Py.Import("optuna");
                try
                {
                    study = optuna.load_study(storage: storage, study_name: studyName);
                }
                catch (Exception e)
                {
                    TunnyMessageBox.Show(e.Message, "Tunny", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string[] nickNames = ((string)study.user_attrs["objective_names"]).Split(',');
                try
                {
                    dynamic vis;
                    switch (visualize)
                    {
                        case "contour":
                            vis = optuna.visualization.plot_contour(study, target_name: nickNames[0]);
                            vis.show();
                            break;
                        case "EDF":
                            vis = optuna.visualization.plot_edf(study, target_name: nickNames[0]);
                            vis.show();
                            break;
                        case "intermediate values":
                            vis = optuna.visualization.plot_intermediate_values(study);
                            vis.show();
                            break;
                        case "optimization history":
                            vis = optuna.visualization.plot_optimization_history(study, target_name: nickNames[0]);
                            vis.show();
                            break;
                        case "parallel coordinate":
                            vis = optuna.visualization.plot_parallel_coordinate(study, target_name: nickNames[0]);
                            vis.show();
                            break;
                        case "param importances":
                            vis = optuna.visualization.plot_param_importances(study, target_name: nickNames[0]);
                            vis.show();
                            break;
                        case "pareto front":
                            vis = optuna.visualization.plot_pareto_front(study, target_names: nickNames);
                            vis.show();
                            break;
                        case "slice":
                            vis = optuna.visualization.plot_slice(study, target_name: nickNames[0]);
                            vis.show();
                            break;
                        default:
                            TunnyMessageBox.Show("This visualization type is not supported in this study case.", "Tunny");
                            break;
                    }
                }
                catch (Exception)
                {
                    TunnyMessageBox.Show("This visualization type is not supported in this study case.", "Tunny", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            PythonEngine.Shutdown();
        }

        public ModelResult[] GetModelResult(int[] resultNum, string studyName)
        {
            string storage = "sqlite:///" + _componentFolder + "/Tunny_Opt_Result.db";
            var modelResult = new List<ModelResult>();
            PythonEngine.Initialize();
            using (Py.GIL())
            {
                dynamic optuna = Py.Import("optuna");
                dynamic study;

                try
                {
                    study = optuna.load_study(storage: storage, study_name: studyName);
                }
                catch (Exception e)
                {
                    TunnyMessageBox.Show(e.Message, "Tunny", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return modelResult.ToArray();
                }

                if (resultNum[0] == -1)
                {
                    var bestTrials = (dynamic[])study.best_trials;
                    foreach (dynamic trial in bestTrials)
                    {
                        ParseTrial(modelResult, trial);
                    }
                }
                else if (resultNum[0] == -10)
                {
                    var trials = (dynamic[])study.trials;
                    foreach (dynamic trial in trials)
                    {
                        ParseTrial(modelResult, trial);
                    }
                }
                else
                {
                    foreach (int res in resultNum)
                    {
                        dynamic trial = study.trials[res];
                        ParseTrial(modelResult, trial);
                    }
                }
            }
            PythonEngine.Shutdown();

            return modelResult.ToArray();
        }

        private static void ParseTrial(ICollection<ModelResult> modelResult, dynamic trial)
        {
            var trialResult = new ModelResult
            {
                Number = (int)trial.number,
                Variables = ParseVariables(trial),
                Objectives = (double[])trial.values,
                Attributes = ParseAttributes(trial),
            };
            if (trialResult.Objectives != null)
            {
                modelResult.Add(trialResult);
            }
        }

        private static Dictionary<string, double> ParseVariables(dynamic trial)
        {
            var variables = new Dictionary<string, double>();
            double[] values = (double[])trial.@params.values();
            string[] keys = (string[])trial.@params.keys();
            for (int i = 0; i < keys.Length; i++)
            {
                variables.Add(keys[i], values[i]);
            }

            return variables;
        }

        private static Dictionary<string, List<string>> ParseAttributes(dynamic trial)
        {
            var attributes = new Dictionary<string, List<string>>();
            string[] keys = (string[])trial.user_attrs.keys();
            for (int i = 0; i < keys.Length; i++)
            {
                string[] values = (string[])trial.user_attrs[keys[i]];
                attributes.Add(keys[i], values.ToList());
            }

            return attributes;
        }
    }
}
