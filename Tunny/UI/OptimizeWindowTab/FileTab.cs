using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

using Tunny.Solver;

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
            File.Delete(_settings.StoragePath);
        }

        private void ShowTunnyLicenseButton_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/hrntsm/Tunny/blob/main/LICENSE");
        }
        private void ShowThirdPartyLicenseButton_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/hrntsm/Tunny/blob/main/PYTHON_PACKAGE_LICENSES");
        }

        private void SetResultFilePathButton_Click(object sender, EventArgs e)
        {
            var sfd = new SaveFileDialog
            {
                FileName = Path.GetFileName(_settings.StoragePath),
                Filter = "SQLite database(*.db)|*.db",
                Title = "Set Tunny result file path",

            };
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                _settings.StoragePath = sfd.FileName;
                existingStudyComboBox.SelectedIndex = -1;
                existingStudyComboBox.Items.Clear();

                var study = new Study(_component.GhInOut.ComponentFolder, _settings);
                if (!File.Exists(_settings.StoragePath))
                {
                    study.CreateNewStorage();
                }
                UpdateStudyComboBox(study);
            }
        }
    }
}
