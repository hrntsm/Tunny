using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

using Tunny.Optimization;
using Tunny.Solver;

namespace Tunny.UI
{
    public partial class OptimizationWindow : Form
    {
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
            optimizeRunButton.Enabled = false;
            optimizeStopButton.Enabled = true;

            RestoreLoop.StudyName = studyNameTextBox.Text;
            RestoreLoop.Mode = "Restore";
            RestoreLoop.NickNames = _component.GhInOut.Variables.Select(x => x.NickName).ToArray();
            RestoreLoop.Indices = restoreModelNumTextBox.Text.Split(',').Select(int.Parse).ToArray();

            restoreBackgroundWorker.RunWorkerAsync(_component);
        }

        private void RestoreStopButton_Click(object sender, EventArgs e)
        {
            optimizeRunButton.Enabled = true;
            optimizeStopButton.Enabled = false;

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

        private void RestoreReflectButton_Click(object sender, EventArgs e)
        {
            optimizeRunButton.Enabled = false;
            optimizeStopButton.Enabled = true;

            RestoreLoop.StudyName = studyNameTextBox.Text;
            RestoreLoop.Mode = "Reflect";
            RestoreLoop.NickNames = _component.GhInOut.Variables.Select(x => x.NickName).ToArray();
            RestoreLoop.Indices = restoreModelNumTextBox.Text.Split(',').Select(int.Parse).ToArray();

            restoreBackgroundWorker.RunWorkerAsync(_component);
        }

        private void RestoreProgressChangedHandler(object sender, ProgressChangedEventArgs e)
        {
            restoreProgressBar.Value = e.ProgressPercentage;
            restoreProgressBar.Update();
        }
    }
}
