using System.Windows.Forms;

using Serilog;

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
                    Log.Error(noLineBreakMessage);
                    break;
                case MessageBoxIcon.Warning:
                    Log.Warning(noLineBreakMessage);
                    break;
                case MessageBoxIcon.Information:
                    Log.Information(noLineBreakMessage);
                    break;
                default:
                    Log.Debug(noLineBreakMessage);
                    break;
            }
        }
    }
}
