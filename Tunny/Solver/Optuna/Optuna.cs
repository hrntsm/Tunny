using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

using Grasshopper.Kernel;

using Python.Runtime;

using Tunny.Optimization;
using Tunny.Settings;
using Tunny.UI;
using Tunny.Util;

namespace Tunny.Solver.Optuna
{
    public class Optuna
    {
        public double[] XOpt { get; private set; }
        private readonly string _componentFolder;
        private readonly TunnySettings _settings;

        public Optuna(string componentFolder, TunnySettings settings)
        {
            _componentFolder = componentFolder;
            _settings = settings;
            string envPath = PythonInstaller.GetEmbeddedPythonPath() + @"\python310.dll";
            Environment.SetEnvironmentVariable("PYTHONNET_PYDLL", envPath, EnvironmentVariableTarget.Process);
        }

        public bool RunSolver(
            List<Variable> variables,
            IEnumerable<IGH_Param> objectives,
            Func<IList<decimal>, int, EvaluatedGHResult> evaluate)
        {
            string[] objNickName = objectives.Select(x => x.NickName).ToArray();

            EvaluatedGHResult Eval(double[] x, int progress)
            {
                var decimals = x.Select(Convert.ToDecimal).ToList();
                return evaluate(decimals, progress);
            }

            try
            {
                var optimize = new Algorithm(variables, objNickName, _settings, Eval);
                optimize.Solve();
                XOpt = optimize.GetXOptimum();

                ShowEndMessages(optimize);
                return true;
            }
            catch (Exception e)
            {
                ShowErrorMessages(e);
                return false;
            }
        }

        private static void ShowEndMessages(Algorithm optimize)
        {
            switch (optimize.EndState)
            {
                case EndState.Timeout:
                    TunnyMessageBox.Show("Solver completed successfully.\nThe specified time has elapsed.", "Tunny");
                    break;
                case EndState.AllTrialCompleted:
                    TunnyMessageBox.Show("Solver completed successfully.\nThe specified number of trials has been completed.", "Tunny");
                    break;
                case EndState.StoppedByUser:
                    TunnyMessageBox.Show("Solver completed successfully.\nThe user stopped the solver.", "Tunny");
                    break;
                default:
                    TunnyMessageBox.Show("Solver error.", "Tunny");
                    break;
            }
        }

        private static void ShowErrorMessages(Exception e)
        {
            TunnyMessageBox.Show(
                "Tunny runtime error:\n" +
                "Please send below message (& gh file if possible) to Tunny support.\n\n" +
                "\" " + e.Message + " \"", "Tunny",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public void ShowSelectedTypePlot(string visualize, string studyName)
        {
            string storage = "sqlite:///" + _settings.Storage;
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
                    ShowPlot(optuna, visualize, study, nickNames);
                }
                catch (Exception)
                {
                    TunnyMessageBox.Show("This visualization type is not supported in this study case.", "Tunny", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            PythonEngine.Shutdown();
        }

        private static void ShowPlot(dynamic optuna, string visualize, dynamic study, string[] nickNames)
        {
            dynamic vis;
            switch (visualize)
            {
                case "contour":
                    vis = optuna.visualization.plot_contour(study, target_name: nickNames[0]);
                    break;
                case "EDF":
                    vis = optuna.visualization.plot_edf(study, target_name: nickNames[0]);
                    break;
                case "intermediate values":
                    vis = optuna.visualization.plot_intermediate_values(study);
                    break;
                case "optimization history":
                    vis = optuna.visualization.plot_optimization_history(study, target_name: nickNames[0]);
                    break;
                case "parallel coordinate":
                    vis = optuna.visualization.plot_parallel_coordinate(study, target_name: nickNames[0]);
                    break;
                case "param importances":
                    vis = optuna.visualization.plot_param_importances(study, target_name: nickNames[0]);
                    break;
                case "pareto front":
                    vis = optuna.visualization.plot_pareto_front(study, target_names: nickNames);
                    break;
                case "slice":
                    vis = optuna.visualization.plot_slice(study, target_name: nickNames[0]);
                    break;
                default:
                    TunnyMessageBox.Show("This visualization type is not supported in this study case.", "Tunny");
                    return;
            }
            vis.show();
        }

        public ModelResult[] GetModelResult(int[] resultNum, string studyName, BackgroundWorker worker)
        {
            string storage = "sqlite:///" + _settings.Storage;
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

                SetTrialsToModelResult(resultNum, modelResult, study, worker);
            }
            PythonEngine.Shutdown();

            return modelResult.ToArray();
        }

        private void SetTrialsToModelResult(int[] resultNum, List<ModelResult> modelResult, dynamic study, BackgroundWorker worker)
        {
            if (resultNum[0] == -1)
            {
                ParatoSolutions(modelResult, study, worker);
            }
            else if (resultNum[0] == -10)
            {
                AllTrials(modelResult, study, worker);
            }
            else
            {
                UseModelNumber(resultNum, modelResult, study, worker);
            }
        }

        private void UseModelNumber(int[] resultNum, List<ModelResult> modelResult, dynamic study, BackgroundWorker worker)
        {
            for (int i = 0; i < resultNum.Length; i++)
            {
                int res = resultNum[i];
                if (OutputLoop.IsForcedStopOutput)
                {
                    break;
                }
                dynamic trial = study.trials[res];
                ParseTrial(modelResult, trial);
                worker.ReportProgress(i * 100 / resultNum.Length);
            }
        }

        private void AllTrials(List<ModelResult> modelResult, dynamic study, BackgroundWorker worker)
        {
            var trials = (dynamic[])study.trials;
            for (int i = 0; i < trials.Length; i++)
            {
                dynamic trial = trials[i];
                if (OutputLoop.IsForcedStopOutput)
                {
                    break;
                }
                ParseTrial(modelResult, trial);
                worker.ReportProgress(i * 100 / trials.Length);
            }
        }

        private void ParatoSolutions(List<ModelResult> modelResult, dynamic study, BackgroundWorker worker)
        {
            var bestTrials = (dynamic[])study.best_trials;
            for (int i = 0; i < bestTrials.Length; i++)
            {
                dynamic trial = bestTrials[i];
                if (OutputLoop.IsForcedStopOutput)
                {
                    break;
                }
                ParseTrial(modelResult, trial);
                worker.ReportProgress(i * 100 / bestTrials.Length);
            }
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
