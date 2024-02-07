using System.Windows.Forms;

using Tunny.Util;

namespace Tunny.UI
{
    sealed class TunnyMessageBox
    {
        public static void Show(string message, string caption, MessageBoxButtons buttons = MessageBoxButtons.OK, MessageBoxIcon icon = MessageBoxIcon.Information)
        {
            WriteLog(message, icon);
            using (var f = new Form())
            {
                f.Owner = Grasshopper.Instances.DocumentEditor;
                f.TopMost = true;
                MessageBox.Show(message, caption, buttons, icon);
                f.TopMost = false;
            }
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
