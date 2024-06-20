using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

using Tunny.Component.Optimizer;
using Tunny.Core.Handler;
using Tunny.Core.Input;
using Tunny.Core.Settings;
using Tunny.Core.TEnum;
using Tunny.Core.Util;
using Tunny.Handler;
using Tunny.Input;
using Tunny.PostProcess;
using Tunny.Type;
using Tunny.UI;

namespace Tunny.Solver
{
    public class Solver
    {
        public Parameter[] OptimalParameters { get; private set; }
        private readonly bool _hasConstraint;
        private readonly TSettings _settings;
        private const string CompleteMessagePrefix = "Solver completed successfully.";
        private const string ErrorMessagePrefix = "Solver error.";

        public Solver(TSettings settings, bool hasConstraint)
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

            try
            {
                InitializeTmpDir();
                var optimize = new Algorithm(variables, _hasConstraint, objectives, fishEggs, _settings, Eval);
                optimize.Solve();
                OptimalParameters = optimize.OptimalParameters;
                DialogResult dialogResult = EndMessage(optimize, objectives.Length > 1);

                return dialogResult == DialogResult.Yes;
            }
            catch (Exception e)
            {
                ShowErrorMessages(e);
                return false;
            }
        }

        private static void InitializeTmpDir()
        {
            TLog.MethodStart();
            if (!Directory.Exists(TEnvVariables.TmpDirPath))
            {
                TLog.Info("Create tmp folder");
                Directory.CreateDirectory(TEnvVariables.TmpDirPath);
            }
            else
            {
                TLog.Info("Start clean tmp files");
                var tmpDir = new DirectoryInfo(TEnvVariables.TmpDirPath);
                foreach (FileInfo file in tmpDir.GetFiles())
                {
                    try
                    {
                        file.Delete();
                    }
                    catch (Exception e)
                    {
                        TLog.Error(e.Message);
                    }
                }
                TLog.Info("Finish clean tmp files");
            }
        }

        private static DialogResult EndMessage(Algorithm optimize, bool isMultiObjective)
        {
            TLog.MethodStart();
            DialogResult dialogResult = DialogResult.None;
            ToComponentEndMessage(optimize);
            if (OptimizeLoop.Component is UIOptimizeComponentBase)
            {
                dialogResult = ShowUIEndMessages(optimize.EndState, isMultiObjective);
            }
            return dialogResult;
        }

        private static void ToComponentEndMessage(Algorithm optimize)
        {
            TLog.MethodStart();
            string message;
            switch (optimize.EndState)
            {
                case EndState.Timeout:
                    message = CompleteMessagePrefix + " The specified time has elapsed.";
                    break;
                case EndState.AllTrialCompleted:
                    message = CompleteMessagePrefix + " The specified number of trials has been completed.";
                    break;
                case EndState.StoppedByUser:
                    message = CompleteMessagePrefix + " The user stopped the solver.";
                    break;
                case EndState.StoppedByOptuna:
                    message = CompleteMessagePrefix + " The Optuna stopped the solver.";
                    break;
                case EndState.DirectionNumNotMatch:
                    message = ErrorMessagePrefix + " The number of Objective in the existing Study does not match the one that you tried to run; Match the number of objective, or change the \"Study Name\".";
                    break;
                case EndState.UseExitStudyWithoutContinue:
                    message = ErrorMessagePrefix + " \"Load if study file exists\" was false even though the same \"Study Name\" exists. Please change the name or set it to true.";
                    break;
                default:
                    message = ErrorMessagePrefix;
                    break;
            }
            if (OptimizeLoop.Component is BoneFishComponent)
            {
                TLog.Info(message);
            }
            OptimizeLoop.Component.SetInfo(message);
        }

        private static DialogResult ShowUIEndMessages(EndState endState, bool isMultiObjective)
        {
            TLog.MethodStart();
            DialogResult dialogResult;
            MessageBoxButtons button = isMultiObjective ? MessageBoxButtons.OK : MessageBoxButtons.YesNo;
            string reinstateMessage = isMultiObjective ? string.Empty : "\nReinstate the best trial to the slider?";
            switch (endState)
            {
                case EndState.Timeout:
                    dialogResult = TunnyMessageBox.Show(CompleteMessagePrefix + "\n\nThe specified time has elapsed."+ reinstateMessage, "Tunny", button);
                    break;
                case EndState.AllTrialCompleted:
                    dialogResult = TunnyMessageBox.Show(CompleteMessagePrefix + "\n\nThe specified number of trials has been completed." + reinstateMessage, "Tunny", button);
                    break;
                case EndState.StoppedByUser:
                    dialogResult = TunnyMessageBox.Show(CompleteMessagePrefix + "\n\nThe user stopped the solver."+ reinstateMessage, "Tunny", button);
                    break;
                case EndState.StoppedByOptuna:
                    dialogResult = TunnyMessageBox.Show(CompleteMessagePrefix + "\n\nThe Optuna stopped the solver." + reinstateMessage, "Tunny", button);
                    break;
                case EndState.DirectionNumNotMatch:
                    dialogResult = TunnyMessageBox.Show(ErrorMessagePrefix + "\n\nThe number of Objective in the existing Study does not match the one that you tried to run; Match the number of objective, or change the \"Study Name\".", "Tunny", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                case EndState.UseExitStudyWithoutContinue:
                    dialogResult = TunnyMessageBox.Show(ErrorMessagePrefix + "\n\n\"Load if study file exists\" was false even though the same \"Study Name\" exists. Please change the name or set it to true.", "Tunny", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                case EndState.Error:
                    dialogResult = TunnyMessageBox.Show(ErrorMessagePrefix, "Tunny");
                    break;
                default:
                    dialogResult = TunnyMessageBox.Show(ErrorMessagePrefix + "\n\n Unexpected exception.", "Tunny");
                    break;
            }
            return dialogResult;
        }

        private static void ShowErrorMessages(Exception e)
        {
            TLog.MethodStart();
            TunnyMessageBox.Show(
                "Tunny runtime error:\n" +
                "Please send below message (& gh file if possible) to Tunny support.\n" +
                "If this error occurs, the Tunny solver will not work after this unless Rhino is restarted.\n\n" +
                "Source: " + e.Source + " \n" +
                "Message: " + e.Message,
                "Tunny",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
