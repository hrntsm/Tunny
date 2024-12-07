using System.Windows;

using Tunny.Core.Util;

namespace Tunny.WPF.Common
{
    internal static partial class TunnyMessageBox
    {
        private static SharedItems SharedItems => SharedItems.Instance;

        internal static MessageBoxResult Show(string messageBoxText, string caption, MessageBoxButton button = MessageBoxButton.OK, MessageBoxImage icon = MessageBoxImage.Information)
        {
            WriteLog(messageBoxText, icon);
            MessageBoxResult msgResult = MessageBoxResult.None;

            if (SharedItems.TunnyWindow == null)
            {
                msgResult = MessageBox.Show(messageBoxText, caption, button, icon);
            }
            else
            {
                SharedItems.TunnyWindow.Dispatcher.Invoke(() =>
                    msgResult = MessageBox.Show(SharedItems.TunnyWindow, messageBoxText, caption, button, icon)
                );
            }

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
    }
}
