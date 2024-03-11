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

            if (_settings.Storage.Type != StorageType.Sqlite && objectives.HumanInTheLoopType != HumanInTheLoopType.None)
            {
                TunnyMessageBox.Show("Human-in-the-Loop only supports SQlite storage.In the \"File\" tab, select \"Set file path\" and change the file type to sqlite storage", "Tunny", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            try
            {
                InitializeTmpDir();
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
    }
}
