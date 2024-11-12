using System.Windows;

using Tunny.Core.Util;

namespace Tunny.WPF.Common
{
    internal static class TunnyMessageBox
    {
        internal static MessageBoxResult Show(string messageBoxText, string caption, MessageBoxButton button = MessageBoxButton.OK, MessageBoxImage icon = MessageBoxImage.Information)
        {
            WriteLog(messageBoxText, icon);

            MessageBoxResult msgResult = MessageBox.Show(messageBoxText, caption, button, icon);
            if (msgResult != MessageBoxResult.None && msgResult != MessageBoxResult.OK)
            {
                TLog.Info($"Dialog result: {msgResult}");
            }
            return msgResult;
        }

        private static void WriteLog(string message, MessageBoxImage icon)
        {
            string noLineBreakMessage = message.Replace("\n", " ");
            switch (icon)
            {
                case MessageBoxImage.Error:
                    TLog.Error(noLineBreakMessage);
                    break;
                case MessageBoxImage.Warning:
                    TLog.Warning(noLineBreakMessage);
                    break;
                case MessageBoxImage.Information:
                    TLog.Info(noLineBreakMessage);
                    break;
                default:
                    TLog.Debug(noLineBreakMessage);
                    break;
            }
        }

        internal static void Error_IncorrectVariableInput()
        {
            TLog.MethodStart();
            Show(
                "Input variables must be either a number slider or a gene pool.\nError input will automatically remove.",
                "Tunny",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }

        internal static void Error_PrunerPath()
        {
            TLog.MethodStart();
            Show(
                "PrunerPath has something wrong. Please check.",
                "Tunny",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }

        internal static void Error_IncorrectObjectiveInput()
        {
            TLog.MethodStart();
            Show(
                "Objective must be either a number or a FishPrint.\nError input will automatically remove.",
                "Tunny",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }

        internal static void Error_IncorrectAttributeInput()
        {
            TLog.MethodStart();
            Show(
                "Inputs to Attribute should be grouped together into one FishAttribute.",
                "Tunny",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }

        internal static void Error_ObjectiveNicknamesMustUnique()
        {
            TLog.MethodStart();
            Show(
                "Objective nicknames must be unique.",
                "Tunny",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }

        internal static MessageBoxResult Info_PythonAlreadyInstalled()
        {
            TLog.MethodStart();
            return Show(
                "It appears that the Tunny Python environment is already installed.\nWould you like to reinstall it?",
                "Python is already installed",
                MessageBoxButton.OKCancel,
                MessageBoxImage.Information
            );
        }

        internal static void Error_NoVariableInput()
        {
            TLog.MethodStart();
            Show(
                "No input variables found. \nPlease connect a number slider to the input of the component.",
                "Tunny",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }

        internal static bool Error_ShowNoObjectiveFound()
        {
            TLog.MethodStart();
            Show(
                "No objective found.\nPlease connect number or FishPrint to the objective.",
                "Tunny",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
            return false;
        }

        internal static void Error_DirectionCountNotMatch()
        {
            TLog.MethodStart();
            Show(
                "The number of the direction in FishAttr must be the same as the number of the objective.",
                "Tunny",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }

        internal static void Error_DirectionValue()
        {
            TLog.MethodStart();
            Show(
                "Direction must be either 1(maximize) or -1(minimize).",
                "Tunny",
                MessageBoxButton.OK,
                MessageBoxImage.Error
            );
        }

        internal static void Info_OptunaDashboardAlreadyInstalled()
        {
            TLog.MethodStart();
            Show("optuna-dashboard is not installed.\nFirst install optuna-dashboard from the Tunny component.",
                "optuna-dashboard is not installed",
                MessageBoxButton.OK,
                MessageBoxImage.Error
            );
        }

        internal static void Error_ComponentLoadFail()
        {
            TLog.MethodStart();
            Show(
                "Fail to load Grasshopper data into Tunny",
                "Tunny",
                MessageBoxButton.OK,
                MessageBoxImage.Error
            );
        }

        internal static void Warn_SettingsJsonFileLoadFail()
        {
            TLog.MethodStart();
            Show(
                "Failed to load settings file. Start with default settings.",
                "Tunny",
                MessageBoxButton.OK,
                MessageBoxImage.Warning
            );
        }

        internal static void Error_NoStudyNameSelected()
        {
            TLog.MethodStart();
            Show(
                "Please set StudyName.",
                "Error",
                MessageBoxButton.OK,
                MessageBoxImage.Error
            );
        }

        internal static void Error_ResultFileNotExist()
        {
            TLog.MethodStart();
            Show(
                "Please set exist result file path.",
                "Error",
                MessageBoxButton.OK,
                MessageBoxImage.Error
            );
        }

        internal static void Info_ResultFileHasNoStudy()
        {
            TLog.MethodStart();
            Show(
                "There is no study to visualize.\nPlease set 'Target Study'",
                "Tunny",
                MessageBoxButton.OK,
                MessageBoxImage.Information
            );
        }

        internal static void Error_VisualizationTypeNotSupported()
        {
            TLog.MethodStart();
            Show(
                "This visualization type is not supported in this study case.",
                "Tunny",
                MessageBoxButton.OK,
                MessageBoxImage.Error
            );
        }

        internal static void Error_NoImplemented()
        {
            TLog.MethodStart();
            Show(
                "This feature is not implemented yet.",
                "Tunny",
                MessageBoxButton.OK,
                MessageBoxImage.Error
            );
        }
    }
}
