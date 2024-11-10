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

        private void Plot(PlotActionType pActionType)
        {
            TLog.MethodStart();
        }

        private static bool CheckTargetValues(PlotSettings pSettings)
        {
            TLog.MethodStart();
            switch (pSettings.PlotTypeName)
            {
                case "contour":
                case "parallel coordinate":
                case "slice":
                case "param importances":
                    return CheckOneObjSomeVarTargets(pSettings);
                case "pareto front":
                    return CheckParetoFrontTargets(pSettings);
                case "clustering":
                    return CheckClusteringTargets(pSettings);
                default:
                    return CheckOneObjectives(pSettings);
            }
        }

        private static bool CheckClusteringTargets(PlotSettings pSettings)
        {
            TLog.MethodStart();
            bool result = true;
            if (pSettings.TargetObjectiveName.Length == 0 && pSettings.TargetVariableName.Length == 0)
            {
                TunnyMessageBox.Show("Please select one or more.", "Tunny");
                result = false;
            }

            return result;
        }

        private static bool CheckOneObjectives(PlotSettings pSettings)
        {
            TLog.MethodStart();
            bool result = true;
            if (pSettings.TargetObjectiveName.Length > 1)
            {
                result = HandleOnly1ObjectiveMessage();
            }

            return result;
        }

        private static bool CheckParetoFrontTargets(PlotSettings pSettings)
        {
            TLog.MethodStart();
            bool result = true;
            if (pSettings.TargetObjectiveName.Length > 3 || pSettings.TargetObjectiveName.Length < 2)
            {
                result = HandleOnly2or3ObjectiveMessage();
            }

            return result;
        }

        private static bool CheckOneObjSomeVarTargets(PlotSettings pSettings)
        {
            TLog.MethodStart();
            bool result = true;
            if (pSettings.TargetObjectiveName.Length > 1 || pSettings.TargetObjectiveName.Length == 0)
            {
                result = HandleOnly1ObjectiveMessage();
            }
            else if (pSettings.PlotTypeName == "contour" && pSettings.TargetVariableName.Length < 2)
            {
                result = RequireLeast2VariableMessage();
            }
            else if (pSettings.TargetVariableName.Length == 0)
            {
                result = RequireLeast1VariableMessage();
            }

            return result;
        }

        private void TTDesignExplorerButton_Click(object sender, EventArgs e)
        {
            TLog.MethodStart();
            if (visualizeTargetStudyComboBox.Text == string.Empty)
            {
                TunnyMessageBox.Show(
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
