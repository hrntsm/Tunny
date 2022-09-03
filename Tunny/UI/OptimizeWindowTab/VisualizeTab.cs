using System;
using System.Diagnostics;
using System.Windows.Forms;

using Tunny.Solver.Optuna;
using Tunny.Util;

namespace Tunny.UI
{
    public partial class OptimizationWindow : Form
    {
        private void DashboardButton_Click(object sender, EventArgs e)
        {
            var dashboard = new Process();
            dashboard.StartInfo.FileName = PythonInstaller.GetEmbeddedPythonPath() + @"\Scripts\optuna-dashboard.exe";
            dashboard.StartInfo.Arguments = @"sqlite:///" + _settings.Storage;
            dashboard.StartInfo.UseShellExecute = false;
            dashboard.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;
            dashboard.Start();

            var browser = new Process();
            browser.StartInfo.FileName = @"http://127.0.0.1:8080/";
            browser.StartInfo.UseShellExecute = true;
            browser.Start();
        }

        private void SelectedTypePlotButton_Click(object sender, EventArgs e)
        {
            var optuna = new Optuna(_component.GhInOut.ComponentFolder, _settings, _component.GhInOut.HasConstraint);
            optuna.ShowSelectedTypePlot(visualizeTypeComboBox.Text, studyNameTextBox.Text);
        }

        private void VisualizeClusteringPlotButton_Click(object sender, EventArgs e)
        {
            var optuna = new Optuna(_component.GhInOut.ComponentFolder, _settings, _component.GhInOut.HasConstraint);
            optuna.ShowClusteringPlot(studyNameTextBox.Text, (int)visualizeClusterNumUpDown.Value);
        }
    }
}
