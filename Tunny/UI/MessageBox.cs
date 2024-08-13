using System;
using System.Windows;
using System.Windows.Forms;

using Tunny.Core.Util;

namespace Tunny.UI
{
    sealed class TunnyMessageBox
    {
        public static MessageBoxResult Show(string message, string caption, MessageBoxButton buttons = MessageBoxButton.OK, MessageBoxImage icon = MessageBoxImage.Information)
        {
            MessageBoxResult msgResult;
            WriteLog(message, icon);

            IntPtr ownerHWND = TEnvVariables.GrasshopperWindowHandle == IntPtr.Zero
                ? Rhino.RhinoApp.MainWindowHandle()
                : TEnvVariables.GrasshopperWindowHandle;
            var ownerWindow = new NativeWindow();
            ownerWindow.AssignHandle(ownerHWND);

            using (var f = new Form())
            {
                f.Owner = Grasshopper.Instances.DocumentEditor;
                f.TopMost = true;
                // dialogResult = System.Windows.MessageBox.Show(ownerWindow, message, caption, buttons, icon);
                msgResult = System.Windows.MessageBox.Show(message, caption, buttons, icon);
                f.TopMost = false;
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
