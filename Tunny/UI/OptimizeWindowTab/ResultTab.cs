using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

using Tunny.Optimization;
using Tunny.Solver;
using Tunny.Util;

namespace Tunny.UI
{
    public partial class OptimizationWindow : Form
    {
        private void DashboardButton_Click(object sender, EventArgs e)
        {
            var dashboard = new Process();
            dashboard.StartInfo.FileName = PythonInstaller.GetEmbeddedPythonPath() + @"\Scripts\optuna-dashboard.exe";
            dashboard.StartInfo.Arguments = @"sqlite:///" + _component.GhInOut.ComponentFolder + @"\Tunny_Opt_Result.db";
            dashboard.StartInfo.UseShellExecute = false;
            dashboard.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;
            dashboard.Start();

            var browser = new Process();
            browser.StartInfo.FileName = @"http://127.0.0.1:8080/";
            browser.StartInfo.UseShellExecute = true;
            browser.Start();
        }

        private void VisualizeButton_Click(object sender, EventArgs e)
        {
            var optuna = new Optuna(_component.GhInOut.ComponentFolder);
            optuna.ShowResultVisualize(visualizeTypeComboBox.Text, studyNameTextBox.Text);
        }

        private void OpenResultFolderButton_Click(object sender, EventArgs e)
        {
            Process.Start("EXPLORER.EXE", _component.GhInOut.ComponentFolder);
        }

        private void ClearResultButton_Click(object sender, EventArgs e)
        {
            File.Delete(_component.GhInOut.ComponentFolder + "/Tunny_Opt_Result.db");
        }

        private void RestoreRunButton_Click(object sender, EventArgs e)
        {
            RunRestoreLoop("Restore");
        }

        private void RestoreReflectButton_Click(object sender, EventArgs e)
        {
            RunRestoreLoop("Reflect");
        }

        private void RunRestoreLoop(string mode)
        {
            RestoreLoop.Mode = mode;
            RestoreLoop.StudyName = studyNameTextBox.Text;
            RestoreLoop.NickNames = _component.GhInOut.Variables.Select(x => x.NickName).ToArray();
            RestoreLoop.Indices = restoreModelNumTextBox.Text.Split(',').Select(int.Parse).ToArray();
            restoreBackgroundWorker.RunWorkerAsync(_component);
        }

        private void RestoreStopButton_Click(object sender, EventArgs e)
        {
            if (restoreBackgroundWorker != null)
            {
                restoreBackgroundWorker.CancelAsync();
            }
            switch (RestoreLoop.Mode)
            {
                case "Restore":
                    _component.ExpireSolution(true);
                    break;
                case "Reflect":
                    var decimalVar = _component.Fishes[0].Variables
                            .Select(x => (decimal)x.Value).ToList();
                    _component.GhInOut.NewSolution(decimalVar);
                    break;
            }
        }

        private void RestoreProgressChangedHandler(object sender, ProgressChangedEventArgs e)
        {
            restoreProgressBar.Value = e.ProgressPercentage;
            restoreProgressBar.Update();
        }
    }
}
