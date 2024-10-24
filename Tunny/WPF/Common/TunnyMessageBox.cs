using System.Windows;

using Tunny.Core.Util;

namespace Tunny.WPF.Common
{
    internal static class TunnyMessageBox
    {
        private static MessageBoxResult Show(string messageBoxText, string caption, MessageBoxButton button = MessageBoxButton.OK, MessageBoxImage icon = MessageBoxImage.Information)
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
