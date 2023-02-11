using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

using Tunny.Solver;

namespace Tunny.UI
{
    public partial class OptimizationWindow : Form
    {
        private void OpenResultFolderButton_Click(object sender, EventArgs e)
        {
            Process.Start("EXPLORER.EXE", Path.GetDirectoryName(_settings.Storage.Path));
        }

        private void ClearResultButton_Click(object sender, EventArgs e)
        {
            File.Delete(_settings.Storage.Path);
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
                FileName = Path.GetFileName(_settings.Storage.Path),
                Filter = "SQLite database(*.db)|*.db",
                Title = "Set Tunny result file path",

            };
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                _settings.Storage.Path = sfd.FileName;
                existingStudyComboBox.SelectedIndex = -1;
                existingStudyComboBox.Items.Clear();

                var study = new Study(_component.GhInOut.ComponentFolder, _settings);
                if (!File.Exists(_settings.Storage.Path))
                {
                    study.CreateNewStorage();
                }
                UpdateStudyComboBox(study);
            }
        }
    }
}
