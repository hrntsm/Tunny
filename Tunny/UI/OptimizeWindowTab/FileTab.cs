using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

using Tunny.Settings;
using Tunny.Storage;
using Tunny.Util;

namespace Tunny.UI
{
    public partial class OptimizationWindow
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
                Filter = @"Journal Storage(*.log)|*.log|SQLite Storage(*.db,*.sqlite)|*.db;*.sqlite",
                Title = @"Set Tunny result file path",
            };
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                _settings.Storage.Path = sfd.FileName;
                _settings.Storage.Type = Path.GetExtension(sfd.FileName) == ".log"
                    ? StorageType.Journal : StorageType.Sqlite;
                existingStudyComboBox.SelectedIndex = -1;
                existingStudyComboBox.Items.Clear();

                string storagePath = _settings.Storage.Path;
                if (!File.Exists(storagePath))
                {
                    new StorageHandler().CreateNewStorage(true, _settings.Storage);
                }
                UpdateStudyComboBox(storagePath);
            }
        }

        private void OutputDebugLogButton_Click(object sender, EventArgs e)
        {
            if (File.Exists(TunnyVariables.TunnyEnvPath + "\\debug.log"))
            {
                File.Delete(TunnyVariables.TunnyEnvPath + "\\debug.log");
            }
            using (var process = new Process())
            {
                process.StartInfo.FileName = "PowerShell.exe";
                process.StartInfo.Arguments = $"tree {TunnyVariables.TunnyEnvPath} > {TunnyVariables.TunnyEnvPath}\\debug.log";
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.UseShellExecute = false;
                process.Start();
            }
            TunnyMessageBox.Show("Debug log file is created at\n" + TunnyVariables.TunnyEnvPath + "\\debug.log", "Tunny");
        }
    }
}
