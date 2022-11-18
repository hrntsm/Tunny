using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

using Grasshopper.GUI;

using Tunny.Handler;

namespace Tunny.UI
{
    public partial class OptimizationWindow : Form
    {
        private void ContinueStudyCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (continueStudyCheckBox.Checked)
            {
                existingStudyComboBox.Enabled = true;
                copyStudyCheckBox.Enabled = true;
                studyNameTextBox.Enabled = copyStudyCheckBox.Checked;
            }
            else if (!continueStudyCheckBox.Checked)
            {
                copyStudyCheckBox.Enabled = false;
                existingStudyComboBox.Enabled = false;
                studyNameTextBox.Enabled = true;
            }
        }

        private void CopyStudyCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            studyNameTextBox.Enabled = copyStudyCheckBox.Checked;
        }

        private void OptimizeRunButton_Click(object sender, EventArgs e)
        {
            var ghCanvas = Owner as GH_DocumentEditor;
            ghCanvas.DisableUI();

            optimizeRunButton.Enabled = false;
            GetUIValues();
            OptimizeLoop.Settings = _settings;

            if (!CheckInputValue(ghCanvas))
            {
                return;
            }

            if (optimizeBackgroundWorker.IsBusy)
            {
                optimizeBackgroundWorker.Dispose();
            }
            optimizeBackgroundWorker.RunWorkerAsync(_component);
            optimizeStopButton.Enabled = true;
        }

        private bool CheckInputValue(GH_DocumentEditor ghCanvas)
        {
            List<double> objectiveValues = _component.GhInOut.GetObjectiveValues();
            if (objectiveValues.Count == 0)
            {
                ghCanvas.EnableUI();
                optimizeRunButton.Enabled = true;
                return false;
            }
            else if (objectiveValues.Count > 1
                     && (samplerComboBox.Text == "EvolutionStrategy (CMA-ES)"))
            {
                TunnyMessageBox.Show(
                    "CMA-ES samplers only support single objective optimization.",
                    "Tunny",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                ghCanvas.EnableUI();
                optimizeRunButton.Enabled = true;
                return false;
            }

            if (studyNameTextBox.Enabled && existingStudyComboBox.Items.Contains(studyNameTextBox.Text))
            {
                TunnyMessageBox.Show(
                    "New study name already exists. Please choose another name. Or check 'Continue' checkbox.",
                    "Tunny",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                ghCanvas.EnableUI();
                optimizeRunButton.Enabled = true;
                return false;
            }
            else if (copyStudyCheckBox.Enabled && copyStudyCheckBox.Checked)
            {
                var study = new Solver.Study(_component.GhInOut.ComponentFolder, _settings);
                study.Copy(existingStudyComboBox.Text, studyNameTextBox.Text);
                _settings.StudyName = studyNameTextBox.Text;
            }
            else if (continueStudyCheckBox.Checked)
            {
                _settings.StudyName = existingStudyComboBox.Text;
            }

            return true;
        }

        private void OptimizeStopButton_Click(object sender, EventArgs e)
        {
            optimizeRunButton.Enabled = true;
            optimizeStopButton.Enabled = false;
            OptimizeLoop.IsForcedStopOptimize = true;

            optimizeBackgroundWorker?.Dispose();

            UpdateExistingStudiesComboBox();

            //Enable GUI
            var ghCanvas = Owner as GH_DocumentEditor;
            ghCanvas?.EnableUI();
        }

        private void UpdateExistingStudiesComboBox()
        {
            existingStudyComboBox.Items.Clear();

            var study = new Solver.Study(_component.GhInOut.ComponentFolder, _settings);
            Solver.StudySummary[] summaries = study.GetAllStudySummariesCS();

            if (summaries.Length > 0)
            {
                existingStudyComboBox.Items.AddRange(summaries.Select(summary => summary.StudyName).ToArray());
                if (existingStudyComboBox.Items.Count > 0 && existingStudyComboBox.Items.Count - 1 < existingStudyComboBox.SelectedIndex)
                {
                    existingStudyComboBox.SelectedIndex = 0;
                }
                else if (existingStudyComboBox.Items.Count == 0)
                {
                    existingStudyComboBox.Text = string.Empty;
                    continueStudyCheckBox.Checked = false;
                }

                if (!summaries[0].UserAttributes.ContainsKey("objective_names") || !summaries[0].UserAttributes.ContainsKey("variable_names"))
                {
                    return;
                }
                Solver.StudySummary visualizeStudySummary = summaries.FirstOrDefault(s => s.StudyName == studyNameTextBox.Text);
                if (visualizeStudySummary != null)
                {
                    visualizeVariableListBox.Items.Clear();
                    visualizeVariableListBox.Items.AddRange(visualizeStudySummary.UserAttributes["objective_names"].ToArray());

                    visualizeObjectiveListBox.Items.Clear();
                    visualizeObjectiveListBox.Items.AddRange(visualizeStudySummary.UserAttributes["variable_names"].ToArray());
                }
            }
        }

        private void OptimizeProgressChangedHandler(object sender, ProgressChangedEventArgs e)
        {
            var pState = (ProgressState)e.UserState;
            UpdateGrasshopper(pState.Values);
            string trialNumLabel = "Trial: ";
            optimizeTrialNumLabel.Text = e.ProgressPercentage == 100
                ? trialNumLabel + "#"
                : trialNumLabel + (pState.TrialNumber + 1);

            if (e.ProgressPercentage == 0 || e.ProgressPercentage == 100)
            {
                optimizeBestValueLabel.Text = pState.ObjectiveNum == 1
                    ? "BestValue: #"
                    : "Hypervolume Ratio: #";
            }
            else if (pState.BestValues.Length > 0)
            {
                optimizeBestValueLabel.Text = pState.ObjectiveNum == 1
                    ? "BestValue: " + pState.BestValues[0][0]
                    : $"Hypervolume Ratio: {pState.HypervolumeRatio:0.000}";
            }

            optimizeProgressBar.Value = e.ProgressPercentage;
            optimizeProgressBar.Update();
        }
    }
}
