using System.Windows.Forms;

namespace Tunny.UI
{
    class TunnyMessageBox
    {
        public static void Show(string message, string caption)
        {
            using (Form f = new Form())
            {
                f.Owner = Grasshopper.Instances.DocumentEditor;
                f.TopMost = true;
                MessageBox.Show(message, caption);
                f.TopMost = false;
            }
        }
    }
}