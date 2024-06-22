using System;
using System.Windows.Forms;

using Tunny.Core.Util;

namespace Tunny.UI
{
    sealed class TunnyMessageBox
    {
        public static DialogResult Show(string message, string caption, MessageBoxButtons buttons = MessageBoxButtons.OK, MessageBoxIcon icon = MessageBoxIcon.Information)
        {
            DialogResult dialogResult;
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
                dialogResult = MessageBox.Show(ownerWindow, message, caption, buttons, icon);
                f.TopMost = false;
            }
            if (dialogResult != DialogResult.None && dialogResult != DialogResult.OK)
            {
                TLog.Info($"Dialog result: {dialogResult}");
            }
            return dialogResult;
        }

        private static void WriteLog(string message, MessageBoxIcon icon)
        {
            string noLineBreakMessage = message.Replace("\n", " ");
            switch (icon)
            {
                case MessageBoxIcon.Error:
                    TLog.Error(noLineBreakMessage);
                    break;
                case MessageBoxIcon.Warning:
                    TLog.Warning(noLineBreakMessage);
                    break;
                case MessageBoxIcon.Information:
                    TLog.Info(noLineBreakMessage);
                    break;
                default:
                    TLog.Debug(noLineBreakMessage);
                    break;
            }
        }
    }
}
