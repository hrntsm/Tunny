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
            switch (icon)
            {
                case MessageBoxIcon.Error:
                    Log.Error(message);
                    break;
                case MessageBoxIcon.Warning:
                    Log.Warning(message);
                    break;
                case MessageBoxIcon.Information:
                    Log.Information(message);
                    break;
                default:
                    Log.Debug(message);
                    break;
            }
        }
    }
}
