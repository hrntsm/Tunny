using System;
using System.IO;
using System.Linq;

using Optuna.Study;

using Tunny.Enum;
using Tunny.Handler;
using Tunny.PostProcess;
using Tunny.Solver;

namespace Tunny.UI
{
    public partial class OptimizationWindow
    {
        private void DashboardButton_Click(object sender, EventArgs e)
        {
            if (File.Exists(_settings.Storage.Path) == false)
            {
                ResultFileNotExistErrorMessage();
                return;
            }
            string argument = _settings.Storage.GetOptunaStorageCommandLinePathByExtension();
            string backendPath = _settings.Storage.GetArtifactBackendPath();
            if (Directory.Exists(backendPath))
            {
                argument += " --artifact-dir " + backendPath;
            }

            DashboardHandler.RunDashboardProcess(PythonInstaller.GetEmbeddedPythonPath() + @"\Scripts\optuna-dashboard.exe", argument);
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
                string[] metricNames = visualizeStudySummary.SystemAttrs["study:metric_names"] as string[];
                visualizeVariableListBox.Items.AddRange(metricNames);

                visualizeObjectiveListBox.Items.Clear();
                string[] variableNames = visualizeStudySummary.UserAttrs["variable_names"] as string[];
                visualizeObjectiveListBox.Items.AddRange(variableNames);
            }
        }

        private void VisualizeType_Changed(object sender, EventArgs e)
        {
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
            Plot(PlotActionType.Show);
        }

        private void VisualizeSavePlotButton_Click(object sender, EventArgs e)
        {
            Plot(PlotActionType.Save);
        }

        private void Plot(PlotActionType pActionType)
        {
            var optunaVis = new Visualize(_settings, _component.GhInOut.HasConstraint);
            var pSettings = new PlotSettings
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

        private static bool CheckTargetValues(PlotSettings pSettings)
        {
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

        private static bool CheckOneObjectives(PlotSettings pSettings)
        {
            bool result = true;
            if (pSettings.TargetObjectiveName.Length > 1)
            {
                result = HandleOnly1ObjectiveMessage();
            }

            return result;
        }

        private static bool CheckHypervolumeTargets(PlotSettings pSettings)
        {
            bool result = true;
            if (pSettings.TargetObjectiveName.Length != 2)
            {
                result = HandleOnly2ObjectivesMessage();
            }
            return result;
        }

        private static bool CheckParetoFrontTargets(PlotSettings pSettings)
        {
            bool result = true;
            if (pSettings.TargetObjectiveName.Length > 3 || pSettings.TargetObjectiveName.Length < 2)
            {
                result = HandleOnly2or3ObjectiveMessage();
            }

            return result;
        }

        private static bool CheckOneObjSomeVarTargets(PlotSettings pSettings)
        {
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
