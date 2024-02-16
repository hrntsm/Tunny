using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

using Python.Runtime;

using Tunny.Component.Optimizer;
using Tunny.Enum;
using Tunny.Handler;
using Tunny.Input;
using Tunny.PostProcess;
using Tunny.Settings;
using Tunny.Type;
using Tunny.UI;
using Tunny.Util;

namespace Tunny.Solver
{
    public class Optuna : PythonInit
    {
        public Parameter[] OptimalParameters { get; private set; }
        private readonly bool _hasConstraint;
        private readonly TunnySettings _settings;

        public Optuna(TunnySettings settings, bool hasConstraint)
        {
            TLog.MethodStart();
            _settings = settings;
            _hasConstraint = hasConstraint;
        }

        public bool RunSolver(
            List<VariableBase> variables,
            Objective objectives,
            Dictionary<string, FishEgg> fishEggs,
            Func<ProgressState, int, TrialGrasshopperItems> evaluate)
        {
            TLog.MethodStart();
            TrialGrasshopperItems Eval(ProgressState pState, int progress)
            {
                return evaluate(pState, progress);
            }

            if (_settings.Storage.Type != StorageType.Sqlite && objectives.HumanInTheLoopType != HumanInTheLoopType.None)
            {
                TunnyMessageBox.Show("Human-in-the-Loop only supports SQlite storage.In the \"File\" tab, select \"Set file path\" and change the file type to sqlite storage", "Tunny", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            try
            {
                var optimize = new Algorithm(variables, _hasConstraint, objectives, fishEggs, _settings, Eval);
                optimize.Solve();
                OptimalParameters = optimize.OptimalParameters;
                EndMessage(optimize);

                return true;
            }
            catch (Exception e)
            {
                ShowErrorMessages(e);
                return false;
            }
        }

        private static void EndMessage(Algorithm optimize)
        {
            TLog.MethodStart();
            switch (OptimizeLoop.Component)
            {
                case BoneFishComponent zombie:
                    ToComponentEndMessage(optimize, zombie);
                    break;
                default:
                    ShowUIEndMessages(optimize.EndState);
                    break;
            }
        }

        private static void ToComponentEndMessage(Algorithm optimize, BoneFishComponent zombie)
        {
            TLog.MethodStart();
            string message;
            switch (optimize.EndState)
            {
                case EndState.Timeout:
                    message = "Solver completed successfully. The specified time has elapsed.";
                    break;
                case EndState.AllTrialCompleted:
                    message = "Solver completed successfully. The specified number of trials has been completed.";
                    break;
                case EndState.StoppedByUser:
                    message = "Solver completed successfully. The user stopped the solver.";
                    break;
                case EndState.DirectionNumNotMatch:
                    message = "Solver error. The number of Objective in the existing Study does not match the one that you tried to run; Match the number of objective, or change the \"Study Name\".";
                    break;
                case EndState.UseExitStudyWithoutContinue:
                    message = "Solver error. \"Load if study file exists\" was false even though the same \"Study Name\" exists. Please change the name or set it to true.";
                    break;
                default:
                    message = "Solver error.";
                    break;
            }
            TLog.Info(message);
            zombie.SetInfo(message);
        }

        private static void ShowUIEndMessages(EndState endState)
        {
            TLog.MethodStart();
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
                case EndState.UseExitStudyWithoutContinue:
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
            TLog.MethodStart();
            TunnyMessageBox.Show(
                "Tunny runtime error:\n" +
                "Please send below message (& gh file if possible) to Tunny support.\n" +
                "If this error occurs, the Tunny solver will not work after this unless Rhino is restarted.\n\n" +
                "\" " + e.Message + " \"", "Tunny",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public ModelResult[] GetModelResult(int[] resultNum, string studyName, BackgroundWorker worker)
        {
            TLog.MethodStart();
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
                    TunnyMessageBox.Show(e.Message, "Tunny", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return modelResult.ToArray();
                }

                SetTrialsToModelResult(resultNum, modelResult, study, worker);
            }
            PythonEngine.Shutdown();

            return modelResult.ToArray();
        }

        private void SetTrialsToModelResult(int[] resultNum, List<ModelResult> modelResult, dynamic study, BackgroundWorker worker)
        {
            TLog.MethodStart();
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
            TLog.MethodStart();
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
            TLog.MethodStart();
            var trials = (dynamic[])study.trials;
            for (int i = 0; i < trials.Length; i++)
            {
                dynamic trial = trials[i];
                if (OutputLoop.IsForcedStopOutput)
                {
                    break;
                }
                ParseTrial(modelResult, trial);
                worker?.ReportProgress(i * 100 / trials.Length);
            }
        }

        private void ParatoSolutions(ICollection<ModelResult> modelResult, dynamic study, BackgroundWorker worker)
        {
            TLog.MethodStart();
            var bestTrials = (dynamic[])study.best_trials;
            for (int i = 0; i < bestTrials.Length; i++)
            {
                dynamic trial = bestTrials[i];
                if (OutputLoop.IsForcedStopOutput)
                {
                    break;
                }
                ParseTrial(modelResult, trial);
                worker?.ReportProgress(i * 100 / bestTrials.Length);
            }
        }

        private static void ParseTrial(ICollection<ModelResult> modelResult, dynamic trial)
        {
            TLog.MethodStart();
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

        private static Dictionary<string, object> ParseVariables(dynamic trial)
        {
            TLog.MethodStart();
            var variables = new Dictionary<string, object>();
            object[] pyValues = (object[])trial.@params.values();
            object[] values = new object[pyValues.Length];
            for (int i = 0; i < pyValues.Length; i++)
            {
                object v = pyValues[i];
                switch (v)
                {
                    case PyInt pyInt:
                        values[i] = Convert.ToDouble(pyInt, CultureInfo.InvariantCulture);
                        break;
                    case PyFloat pyFloat:
                        values[i] = Convert.ToDouble(pyFloat, CultureInfo.InvariantCulture);
                        break;
                    case PyString pyString:
                        values[i] = pyString.ToString(CultureInfo.InvariantCulture);
                        break;
                    case double num:
                        values[i] = num;
                        break;
                    case string str:
                        values[i] = str;
                        break;
                }
            }
            string[] keys = (string[])trial.@params.keys();
            for (int i = 0; i < keys.Length; i++)
            {
                variables.Add(keys[i], values[i]);
            }

            return variables;
        }

        private static Dictionary<string, List<string>> ParseAttributes(dynamic trial)
        {
            TLog.MethodStart();
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
