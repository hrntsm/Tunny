using System.Windows.Forms;

using Tunny.Core.Util;

namespace Tunny.UI
{
    sealed class TunnyMessageBox
    {
        public static DialogResult Show(string message, string caption, MessageBoxButtons buttons = MessageBoxButtons.OK, MessageBoxIcon icon = MessageBoxIcon.Information)
        {
            DialogResult dialogResult = DialogResult.None;
            WriteLog(message, icon);
            using (var f = new Form())
            {
                f.Owner = Grasshopper.Instances.DocumentEditor;
                f.TopMost = true;
                dialogResult = MessageBox.Show(message, caption, buttons, icon);
                f.TopMost = false;
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
