using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace Tunny.UI
{
    public partial class OptimizationWindow : Form
    {
        private void OpenResultFolderButton_Click(object sender, EventArgs e)
        {
            Process.Start("EXPLORER.EXE", _component.GhInOut.ComponentFolder);
        }

        private void ClearResultButton_Click(object sender, EventArgs e)
        {
            File.Delete(_settings.Storage);
        }
    }
}
