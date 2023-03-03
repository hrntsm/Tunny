using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

using Grasshopper.Kernel;

using Python.Runtime;

using Tunny.Handler;
using Tunny.Settings;
using Tunny.Type;
using Tunny.UI;
using Tunny.Util;

namespace Tunny.Solver
{
    public class Optuna : PythonInit
    {
        public double[] XOpt { get; private set; }
        private readonly string _componentFolder;
        private readonly bool _hasConstraint;
        private readonly TunnySettings _settings;

        public Optuna(string componentFolder, TunnySettings settings, bool hasConstraint)
        {
            _componentFolder = componentFolder;
            _settings = settings;
            _hasConstraint = hasConstraint;
        }

        public bool RunSolver(
            List<Variable> variables,
            IEnumerable<IGH_Param> objectives,
            Dictionary<string, FishEgg> fishEggs,
            Func<ProgressState, int, EvaluatedGHResult> evaluate)
        {
            string[] objNickName = objectives.Select(x => x.NickName).ToArray();

            EvaluatedGHResult Eval(ProgressState pState, int progress)
            {
                return evaluate(pState, progress);
            }

            try
            {
                var optimize = new Algorithm(variables, _hasConstraint, objNickName, fishEggs, _settings, Eval);
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
                    TunnyMessageBox.Show("Solver completed successfully.\n\nThe specified time has elapsed.", "Tunny");
                    break;
                case EndState.AllTrialCompleted:
                    TunnyMessageBox.Show("Solver completed successfully.\n\nThe specified number of trials has been completed.", "Tunny");
                    break;
                case EndState.StoppedByUser:
                    TunnyMessageBox.Show("Solver completed successfully.\n\nThe user stopped the solver.", "Tunny");
                    break;
                case EndState.DirectionNumNotMatch:
                    TunnyMessageBox.Show("Solver error.\n\nThe number of Objective in the existing Study does not match the one that you tried to run; Match the number of objective, or change the \"Study Name\".", "Tunny", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                case EndState.UseExitStudyWithoutLoading:
                    TunnyMessageBox.Show("Solver error.\n\n\"Load if study file exists\" was false even though the same \"Study Name\" exists. Please change the name or set it to true.", "Tunny", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                "Please send below message (& gh file if possible) to Tunny support.\n" +
                "If this error occurs, the Tunny solver will not work after this unless Rhino is restarted.\n\n" +
                "\" " + e.Message + " \"", "Tunny",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public ModelResult[] GetModelResult(int[] resultNum, string studyName, BackgroundWorker worker)
        {
            string storage = "sqlite:///" + _settings.Storage.Path;
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

                try
                {
                    dynamic trial = study.trials[res];
                    ParseTrial(modelResult, trial);
                }
                catch (Exception e)
                {
                    TunnyMessageBox.Show("Error\n\n" + e.Message, "Tunny", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
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
                bool isFeasible = CheckFeasible(trial);
                if (OutputLoop.IsForcedStopOutput)
                {
                    break;
                }
                if (isFeasible)
                {
                    ParseTrial(modelResult, trial);
                }
                worker.ReportProgress(i * 100 / bestTrials.Length);
            }
        }

        private static bool CheckFeasible(dynamic trial)
        {
            string[] keys = (string[])trial.user_attrs.keys();
            if (keys.Contains("Constraint"))
            {
                double[] constraint = (double[])trial.user_attrs["Constraint"];
                if (constraint.Max() > 0)
                {
                    return false;
                }
            }

            return true;
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
                var values = new List<string>();
                if (keys[i] == "Constraint")
                {
                    double[] constraint = (double[])trial.user_attrs[keys[i]];
                    values = constraint.Select(v => v.ToString(CultureInfo.InvariantCulture)).ToList();
                }
                else
                {
                    string[] valueArray = (string[])trial.user_attrs[keys[i]];
                    values = valueArray.ToList();
                }
                attributes.Add(keys[i], values);
            }
            return attributes;
        }
    }
}
