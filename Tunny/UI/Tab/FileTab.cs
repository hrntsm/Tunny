using System;
using System.Diagnostics;
using System.IO;

using Tunny.Core.Storage;
using Tunny.Core.TEnum;
using Tunny.Core.Util;

namespace Tunny.UI
{
    public partial class OptimizationWindow
    {
        private void OpenResultFolderButton_Click(object sender, EventArgs e)
        {
            TLog.MethodStart();
            Process.Start("EXPLORER.EXE", Path.GetDirectoryName(_settings.Storage.Path));
        }

        private void ClearResultButton_Click(object sender, EventArgs e)
        {
            TLog.MethodStart();
            File.Delete(_settings.Storage.Path);
        }

        private void ShowTunnyLicenseButton_Click(object sender, EventArgs e)
        {
            //OpenBrowser.TunnyLicense();
        }
        private void ShowThirdPartyLicenseButton_Click(object sender, EventArgs e)
        {
            //OpenBrowser.PythonPackagesLicense();
        }

        private void SetResultFilePathButton_Click(object sender, EventArgs e)
        {
            TLog.MethodStart();
            var sfd = new Microsoft.Win32.SaveFileDialog
            {
                FileName = Path.GetFileName(_settings.Storage.Path),
                Filter = @"Journal Storage(*.log)|*.log|SQLite Storage(*.db,*.sqlite)|*.db;*.sqlite",
                Title = @"Set Tunny result file path",
            };
            if (sfd.ShowDialog() == true)
            {
                _settings.Storage.Path = sfd.FileName;
                _settings.Storage.Type = Path.GetExtension(sfd.FileName) == ".log"
                    ? StorageType.Journal : StorageType.Sqlite;
                existingStudyComboBox.SelectedIndex = -1;
                existingStudyComboBox.Items.Clear();

                string storagePath = _settings.Storage.Path;
                if (!File.Exists(storagePath))
                {
                    new StorageHandler().CreateNewTStorage(true, _settings.Storage);
                }
                UpdateStudyComboBox(storagePath);
            }
        }

        private void OutputDebugLogButton_Click(object sender, EventArgs e)
        {
            TLog.MethodStart();
            string path = TEnvVariables.TunnyEnvPath + "\\debug.log";
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            using (var process = new Process())
            {
                process.StartInfo.FileName = "PowerShell.exe";
                process.StartInfo.Arguments = $"tree {TEnvVariables.TunnyEnvPath} > {path}";
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.UseShellExecute = false;
                process.Start();
            }
            TunnyMessageBox.Show("Debug log file is created at\n" + path, "Tunny");
        }
    }
}
