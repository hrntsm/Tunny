using System.Windows.Forms;

namespace Tunny.UI
{
    class TunnyMessageBox
    {
        public static void Show(string message, string caption, MessageBoxButtons buttons = MessageBoxButtons.OK, MessageBoxIcon icon = MessageBoxIcon.Information)
        {
            using (var f = new Form())
            {
                f.Owner = Grasshopper.Instances.DocumentEditor;
                f.TopMost = true;
                MessageBox.Show(message, caption, buttons, icon);
                f.TopMost = false;
            }
        }
    }
}
