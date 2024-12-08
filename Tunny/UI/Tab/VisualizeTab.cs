using System;
using System.IO;
using System.Linq;
using System.Windows;

using Optuna.Study;

using Tunny.Core.Handler;
using Tunny.Core.Settings;
using Tunny.Core.TEnum;
using Tunny.Core.Util;
using Tunny.Solver;

namespace Tunny.UI
{
    public partial class OptimizationWindow
    {
        private void DashboardButton_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException("Already implemented in Tunny.WPF");
        }

        private void VisualizeTargetStudy_Changed(object sender, EventArgs e)
        {
            TLog.MethodStart();
            UpdateVisualizeListBox();
        }

        private void UpdateVisualizeListBox()
        {
            TLog.MethodStart();
            StudySummary visualizeStudySummary = _summaries.FirstOrDefault(s => s.StudyName == visualizeTargetStudyComboBox.Text);
            if (visualizeStudySummary != null)
            {
                visualizeVariableListBox.Items.Clear();
                string versionString = (visualizeStudySummary.UserAttrs["tunny_version"] as string[])[0];
                var version = new Version(versionString);
                string[] metricNames = Array.Empty<string>();
                metricNames = version <= TEnvVariables.OldStorageVersion
                    ? visualizeStudySummary.UserAttrs["objective_names"] as string[]
                    : visualizeStudySummary.SystemAttrs["study:metric_names"] as string[];
                visualizeVariableListBox.Items.AddRange(metricNames);

                visualizeObjectiveListBox.Items.Clear();
                string[] variableNames = visualizeStudySummary.UserAttrs["variable_names"] as string[];
                visualizeObjectiveListBox.Items.AddRange(variableNames);
            }
        }

        private void VisualizeType_Changed(object sender, EventArgs e)
        {
            TLog.MethodStart();
            if (visualizeTypeComboBox.SelectedItem.ToString() == "clustering")
            {
                visualizeClusterNumUpDown.Enabled = true;
                visualizeIncludeDominatedCheckBox.Enabled = false;
            }
            else if (visualizeTypeComboBox.SelectedItem.ToString() == "pareto front")
            {
                visualizeClusterNumUpDown.Enabled = false;
                visualizeIncludeDominatedCheckBox.Enabled = true;
            }
            else
            {
                visualizeClusterNumUpDown.Enabled = false;
                visualizeIncludeDominatedCheckBox.Enabled = false;
            }
        }

        private void VisualizeShowPlotButton_Click(object sender, EventArgs e)
        {
            TLog.MethodStart();
            Plot(PlotActionType.Show);
        }

        private void VisualizeSavePlotButton_Click(object sender, EventArgs e)
        {
            TLog.MethodStart();
            Plot(PlotActionType.Save);
        }

        private static void Plot(PlotActionType pActionType)
        {
            TLog.MethodStart();
        }

        private void TTDesignExplorerButton_Click(object sender, EventArgs e)
        {
            TLog.MethodStart();
            if (visualizeTargetStudyComboBox.Text == string.Empty)
            {
                WPF.Common.TunnyMessageBox.Show(
                    "There is no study to visualize.\nPlease set 'Target Study'",
                    "Tunny",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
                return;
            }

            var designExplorer = new DesignExplorer(visualizeTargetStudyComboBox.Text, _settings.Storage);
            designExplorer.Run();
        }
    }
}
