using System;
using System.IO;
using System.Linq;

using Optuna.Study;

using Tunny.Core.Enum;
using Tunny.Core.Settings;
using Tunny.Core.Util;
using Tunny.Solver;

namespace Tunny.UI
{
    public partial class OptimizationWindow
    {
        private void DashboardButton_Click(object sender, EventArgs e)
        {
            TLog.MethodStart();
            if (File.Exists(_settings.Storage.Path) == false)
            {
                ResultFileNotExistErrorMessage();
                return;
            }
            string dashboardPath = Path.Combine(TEnvVariables.TunnyEnvPath, "python", "Scripts", "optuna-dashboard.exe");
            string storagePath = _settings.Storage.Path;

            var dashboard = new Optuna.Dashboard.Handler(dashboardPath, storagePath);
            dashboard.Run();
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
            var optunaVis = new Visualize(_settings, _component.GhInOut.HasConstraint);
            var pSettings = new Plot
            {
                TargetStudyName = visualizeTargetStudyComboBox.Text,
                PlotActionType = pActionType,
                PlotTypeName = visualizeTypeComboBox.Text,
                TargetObjectiveName = visualizeVariableListBox.SelectedItems.Cast<string>().ToArray(),
                TargetObjectiveIndex = visualizeVariableListBox.SelectedIndices.Cast<int>().ToArray(),
                TargetVariableName = visualizeObjectiveListBox.SelectedItems.Cast<string>().ToArray(),
                TargetVariableIndex = visualizeObjectiveListBox.SelectedIndices.Cast<int>().ToArray(),
                ClusterCount = (int)visualizeClusterNumUpDown.Value,
                IncludeDominatedTrials = visualizeIncludeDominatedCheckBox.Checked,
            };

            if (!CheckTargetValues(pSettings)) { return; }

            if (visualizeTypeComboBox.Text == @"clustering")
            {
                optunaVis.ClusteringPlot(pSettings);
            }
            else
            {
                optunaVis.Plot(pSettings);
            }
        }

        private static bool CheckTargetValues(Plot pSettings)
        {
            TLog.MethodStart();
            switch (pSettings.PlotTypeName)
            {
                case "contour":
                case "parallel coordinate":
                case "slice":
                    return CheckOneObjSomeVarTargets(pSettings);
                case "pareto front":
                case "clustering":
                    return CheckParetoFrontTargets(pSettings);
                case "hypervolume":
                    return CheckHypervolumeTargets(pSettings);
                default:
                    return CheckOneObjectives(pSettings);
            }
        }

        private static bool CheckOneObjectives(Plot pSettings)
        {
            TLog.MethodStart();
            bool result = true;
            if (pSettings.TargetObjectiveName.Length > 1)
            {
                result = HandleOnly1ObjectiveMessage();
            }

            return result;
        }

        private static bool CheckHypervolumeTargets(Plot pSettings)
        {
            TLog.MethodStart();
            bool result = true;
            if (pSettings.TargetObjectiveName.Length != 2)
            {
                result = HandleOnly2ObjectivesMessage();
            }
            return result;
        }

        private static bool CheckParetoFrontTargets(Plot pSettings)
        {
            TLog.MethodStart();
            bool result = true;
            if (pSettings.TargetObjectiveName.Length > 3 || pSettings.TargetObjectiveName.Length < 2)
            {
                result = HandleOnly2or3ObjectiveMessage();
            }

            return result;
        }

        private static bool CheckOneObjSomeVarTargets(Plot pSettings)
        {
            TLog.MethodStart();
            bool result = true;
            if (pSettings.TargetObjectiveName.Length > 1)
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
    }
}
