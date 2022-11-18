using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

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
            dashboard.StartInfo.Arguments = @"sqlite:///" + _settings.StoragePath;
            dashboard.StartInfo.UseShellExecute = false;
            dashboard.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;
            dashboard.Start();

            var browser = new Process();
            browser.StartInfo.FileName = @"http://127.0.0.1:8080/";
            browser.StartInfo.UseShellExecute = true;
            browser.Start();
        }

        private void VisualizeTargetStudy_Changed(object sender, EventArgs e)
        {
            UpdateVisualizeListBox();
        }

        private void UpdateVisualizeListBox()
        {
            StudySummary visualizeStudySummary = _summaries.FirstOrDefault(s => s.StudyName == visualizeTargetStudyComboBox.Text);
            if (visualizeStudySummary != null)
            {
                visualizeVariableListBox.Items.Clear();
                visualizeVariableListBox.Items.AddRange(visualizeStudySummary.UserAttributes["objective_names"].ToArray());

                visualizeObjectiveListBox.Items.Clear();
                visualizeObjectiveListBox.Items.AddRange(visualizeStudySummary.UserAttributes["variable_names"].ToArray());
            }
        }

        private void VisualizeShowPlotButton_Click(object sender, EventArgs e)
        {
            Plot(PlotType.Show);
        }

        private void VisualizeSavePlotButton_Click(object sender, EventArgs e)
        {
            Plot(PlotType.Save);
        }

        private void Plot(PlotType plotType)
        {
            var optunaVis = new Visualize(_settings, _component.GhInOut.HasConstraint);
            optunaVis.Plot(visualizeTypeComboBox.Text, studyNameTextBox.Text, plotType);
        }

        private void VisualizeShowClusteringPlotButton_Click(object sender, EventArgs e)
        {
            ClusteringPlot(PlotType.Show);
        }

        private void VisualizeSaveClusteringPlotButton_Click(object sender, EventArgs e)
        {
            ClusteringPlot(PlotType.Save);
        }

        private void ClusteringPlot(PlotType plotType)
        {
            var optunaVis = new Visualize(_settings, _component.GhInOut.HasConstraint);
            optunaVis.ClusteringPlot(studyNameTextBox.Text, (int)visualizeClusterNumUpDown.Value, plotType);
        }
    }

    public enum PlotType
    {
        Save,
        Show,
    }
}
