using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;

using Python.Runtime;

using Tunny.Component.Params;
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
            string[] objNickName = GetObjectiveNickName(objectives);
            EvaluatedGHResult Eval(ProgressState pState, int progress)
            {
                return evaluate(pState, progress);
            }

            try
            {
                var optimize = new Algorithm(variables, _hasConstraint, objNickName, fishEggs, _settings, Eval);
                optimize.Solve();
                XOpt = optimize.XOpt;
                ShowEndMessages(optimize.EndState);

                return true;
            }
            catch (Exception e)
            {
                ShowErrorMessages(e);
                return false;
            }
        }

        private string[] GetObjectiveNickName(IEnumerable<IGH_Param> objectives)
        {
            string[] objNickName = new string[objectives.Count()];
            int hitlCount = 0;
            foreach ((IGH_Param ghParam, int i) in objectives.Select((ghParam, i) => (ghParam, i)))
            {
                switch (ghParam)
                {
                    case Param_Number param:
                        objNickName[i] = param.NickName;
                        break;
                    case Param_FishPrint param:
                        objNickName[i] = "Human-in-the-Loop " + param.NickName;
                        _settings.Optimize.IsHumanInTheLoop = true;
                        hitlCount++;
                        break;
                    default:
                        break;
                }
            }
            if (hitlCount == 0)
            {
                _settings.Optimize.IsHumanInTheLoop = false;
            }
            // FIXME: Fix JournalStorage when the usage of JournalStorage is understood.
            else if (_settings.Storage.Type != StorageType.Sqlite)
            {
                string message = "Human-in-the-Loop is not available with the current storage type.\nPlease change the storage type to Sqlite.";
                TunnyMessageBox.Show(message, "Tunny", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw new ArgumentException(message);
            }

            return objNickName;
        }

        private static void ShowEndMessages(EndState endState)
        {
            switch (endState)
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
                case EndState.Error:
                    TunnyMessageBox.Show("Solver error.", "Tunny");
                    break;
                default:
                    TunnyMessageBox.Show("Solver unexpected error.", "Tunny");
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
            var modelResult = new List<ModelResult>();
            PythonEngine.Initialize();
            using (Py.GIL())
            {
                dynamic optuna = Py.Import("optuna");
                dynamic study;

                try
                {
                    dynamic storage = _settings.Storage.CreateNewOptunaStorage(false);
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

        private static void UseModelNumber(IReadOnlyList<int> resultNum, ICollection<ModelResult> modelResult, dynamic study, BackgroundWorker worker)
        {
            for (int i = 0; i < resultNum.Count; i++)
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
                worker.ReportProgress(i * 100 / resultNum.Count);
            }
        }

        private static void AllTrials(ICollection<ModelResult> modelResult, dynamic study, BackgroundWorker worker)
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

        private void ParatoSolutions(ICollection<ModelResult> modelResult, dynamic study, BackgroundWorker worker)
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
            foreach (string key in keys)
            {
                List<string> values;
                if (key == "Constraint")
                {
                    double[] constraint = (double[])trial.user_attrs[key];
                    values = constraint.Select(v => v.ToString(CultureInfo.InvariantCulture)).ToList();
                }
                else
                {
                    string[] valueArray = (string[])trial.user_attrs[key];
                    values = valueArray.ToList();
                }
                attributes.Add(key, values);
            }
            return attributes;
        }
    }
}
