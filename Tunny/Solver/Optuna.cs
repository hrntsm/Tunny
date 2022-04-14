using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using Python.Runtime;

using Tunny.Optimization;
using Tunny.UI;
using Tunny.Util;

namespace Tunny.Solver
{
    public class Optuna : ISolver
    {
        public double[] XOpt { get; private set; }
        public double[] FxOpt { get; private set; }

        private readonly string _componentFolder;
        private readonly Dictionary<string, Dictionary<string, double>> _presets = new Dictionary<string, Dictionary<string, double>>();

        public Optuna(string componentFolder)
        {
            _componentFolder = componentFolder;
            string envPath = componentFolder + @"\Python\python310.dll";
            Environment.SetEnvironmentVariable("PYTHONNET_PYDLL", envPath, EnvironmentVariableTarget.Process);
        }

        public bool RunSolver(
            List<Variable> variables,
            Func<IList<decimal>, int, EvaluatedGHResult> evaluate,
            string preset,
            Dictionary<string, object> settings, string installFolder, string documentPath)
        {
            int dVar = variables.Count;
            var lb = new double[dVar];
            var ub = new double[dVar];
            var integer = new bool[dVar];
            var nickName = new string[dVar];

            for (var i = 0; i < dVar; i++)
            {
                lb[i] = Convert.ToDouble(variables[i].LowerBond);
                ub[i] = Convert.ToDouble(variables[i].UpperBond);
                integer[i] = variables[i].Integer;
                nickName[i] = variables[i].NickName;
            }

            EvaluatedGHResult Eval(double[] x, int progress)
            {
                List<decimal> decimals = x.Select(Convert.ToDecimal).ToList();
                return evaluate(decimals, progress);
            }

            try
            {
                var tpe = new OptunaAlgorithm(lb, ub, nickName, settings, Eval);
                tpe.Solve();
                XOpt = tpe.Get_XOptimum();
                FxOpt = tpe.Get_fxOptimum();

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

        public string GetErrorMessage() => "";
        public double[] Get_XOptimum => XOpt;
        public IEnumerable<string> GetPresetNames() => _presets.Keys;

        public void ShowResultVisualize(string visualize, string studyName)
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

                try
                {
                    dynamic vis;
                    switch (visualize)
                    {
                        case "contour":
                            vis = optuna.visualization.plot_contour(study);
                            vis.show();
                            break;
                        case "EDF":
                            vis = optuna.visualization.plot_edf(study);
                            vis.show();
                            break;
                        case "intermediate values":
                            vis = optuna.visualization.plot_intermediate_values(study);
                            vis.show();
                            break;
                        case "optimization history":
                            vis = optuna.visualization.plot_optimization_history(study);
                            vis.show();
                            break;
                        case "parallel coordinate":
                            vis = optuna.visualization.plot_parallel_coordinate(study);
                            vis.show();
                            break;
                        case "param importances":
                            vis = optuna.visualization.plot_param_importances(study);
                            vis.show();
                            break;
                        case "pareto front":
                            vis = optuna.visualization.plot_pareto_front(study);
                            vis.show();
                            break;
                        case "slice":
                            vis = optuna.visualization.plot_slice(study);
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
                else
                {
                    for (var i = 0; i < resultNum.Length; i++)
                    {
                        dynamic trial = study.trials[resultNum[i]];
                        ParseTrial(modelResult, trial);
                    }
                }
            }
            PythonEngine.Shutdown();

            return modelResult.ToArray();
        }

        private static void ParseTrial(ICollection<ModelResult> modelResult, dynamic trial)
        {
            var values = (double[])trial.@params.values();
            var keys = (string[])trial.@params.keys();
            var variables = new Dictionary<string, double>();
            for (var i = 0; i < keys.Length; i++)
            {
                variables.Add(keys[i], values[i]);
            }

            modelResult.Add(new ModelResult()
            {
                Number = (int)trial.number,
                Draco = (string)trial.user_attrs["geometry"],
                Variables = variables,
                Objectives = (double[])trial.values,
            });
        }
    }

    public class ModelResult
    {
        public int Number { get; set; }
        public string Draco { get; set; }
        public Dictionary<string, double> Variables { get; set; }
        public double[] Objectives { get; set; }
    }
}