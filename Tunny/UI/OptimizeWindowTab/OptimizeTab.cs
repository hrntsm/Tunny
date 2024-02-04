using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;

using Grasshopper.GUI;

using Optuna.Study;

using Serilog;

using Tunny.Enum;
using Tunny.Handler;
using Tunny.Storage;

namespace Tunny.UI
{
    public partial class OptimizationWindow
    {
        private StudySummary[] _summaries;

        private void InMemoryCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (inMemoryCheckBox.Checked)
            {
                continueStudyCheckBox.Checked = false;
                continueStudyCheckBox.Enabled = false;
                copyStudyCheckBox.Checked = false;
                copyStudyCheckBox.Enabled = false;
                studyNameTextBox.Enabled = true;
                _settings.Storage.Type = StorageType.InMemory;
            }
            else
            {
                continueStudyCheckBox.Enabled = true;
                _settings.Storage.Type = Path.GetExtension(_settings.Storage.Path) == ".log" ? StorageType.Journal : StorageType.Sqlite;
            }
        }

        private void ContinueStudyCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (continueStudyCheckBox.Checked)
            {
                existingStudyComboBox.Enabled = true;
                copyStudyCheckBox.Enabled = true;
                studyNameTextBox.Enabled = copyStudyCheckBox.Checked;
                inMemoryCheckBox.Enabled = false;
            }
            else
            {
                copyStudyCheckBox.Enabled = false;
                existingStudyComboBox.Enabled = false;
                studyNameTextBox.Enabled = true;
                inMemoryCheckBox.Enabled = true;
            }
        }

        private void CopyStudyCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            studyNameTextBox.Enabled = copyStudyCheckBox.Checked;
        }

        private void OptimizeRunButton_Click(object sender, EventArgs e)
        {
            var ghCanvas = Owner as GH_DocumentEditor;
            ghCanvas?.DisableUI();

            optimizeRunButton.Enabled = false;
            GetUIValues();
            ShowRealtimeResultCheckBox.Enabled = false;
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
            bool checkResult = CheckObjectivesCount(ghCanvas);

            switch (checkResult)
            {
                case true when studyNameTextBox.Enabled && existingStudyComboBox.Items.Contains(studyNameTextBox.Text):
                    checkResult = NameAlreadyExistMessage(ghCanvas);
                    break;
                case true when copyStudyCheckBox.Enabled && copyStudyCheckBox.Checked:
                    new StorageHandler().DuplicateStudyInStorage(existingStudyComboBox.Text, studyNameTextBox.Text, _settings.Storage);
                    _settings.StudyName = studyNameTextBox.Text;
                    break;
                case true when continueStudyCheckBox.Checked:
                    checkResult = CheckSameStudyName(ghCanvas);
                    break;
                default:
                    break;
            }
            return checkResult;
        }

        private bool CheckSameStudyName(GH_DocumentEditor ghCanvas)
        {
            if (existingStudyComboBox.Text == string.Empty)
            {
                return SameStudyNameMassage(ghCanvas);
            }
            _settings.StudyName = existingStudyComboBox.Text;

            return true;
        }

        private bool CheckObjectivesCount(GH_DocumentEditor ghCanvas)
        {
            int length = _component.GhInOut.Objectives.Length;
            if (length == 0)
            {
                ghCanvas.EnableUI();
                optimizeRunButton.Enabled = true;
                return false;
            }
            else if (length > 1 && (samplerComboBox.Text == @"EvolutionStrategy (CMA-ES)"))
            {
                return CmaEsSupportOneObjectiveMessage(ghCanvas);
            }

            return true;
        }

        private void OptimizeStopButton_Click(object sender, EventArgs e)
        {
            optimizeRunButton.Enabled = true;
            optimizeStopButton.Enabled = false;
            OptimizeLoop.IsForcedStopOptimize = true;
            ShowRealtimeResultCheckBox.Enabled = true;
            optimizeBackgroundWorker?.Dispose();

            UpdateStudyComboBox();

            //Enable GUI
            var ghCanvas = Owner as GH_DocumentEditor;
            ghCanvas?.EnableUI();
        }

        private void UpdateStudyComboBox()
        {
            try
            {
                UpdateStudyComboBox(_settings.Storage.Path);
            }
            catch (Exception)
            {
                string message = "The storage file loading error.Please check the storage path or use new storage file.";
                TunnyMessageBox.Show(message, "Error");
            }
        }

        private void UpdateStudyComboBox(string storagePath)
        {
            existingStudyComboBox.Items.Clear();
            visualizeTargetStudyComboBox.Items.Clear();
            outputTargetStudyComboBox.Items.Clear();
            cmaEsWarmStartComboBox.Items.Clear();

            Log.Information("Get study summaries from storage file: {0}", storagePath);
            _summaries = new StorageHandler().GetStudySummaries(storagePath);

            if (_summaries.Length > 0)
            {
                string[] studyNames = _summaries.Select(summary => summary.StudyName).ToArray();
                existingStudyComboBox.Items.AddRange(studyNames);
                cmaEsWarmStartComboBox.Items.AddRange(studyNames);
                UpdateExistingStudyComboBox();

                visualizeTargetStudyComboBox.Items.AddRange(studyNames);
                UpdateVisualizeTargetStudyComboBox();

                outputTargetStudyComboBox.Items.AddRange(studyNames);

                if (!_summaries[0].SystemAttrs.ContainsKey("study:metric_names") || !_summaries[0].UserAttrs.ContainsKey("variable_names"))
                {
                    return;
                }

                UpdateVisualizeListBox();
            }
        }

        private void UpdateVisualizeTargetStudyComboBox()
        {
            if (visualizeTargetStudyComboBox.Items.Count > 0 && visualizeTargetStudyComboBox.Items.Count - 1 < visualizeTargetStudyComboBox.SelectedIndex)
            {
                visualizeTargetStudyComboBox.SelectedIndex = 0;
            }
            else if (visualizeTargetStudyComboBox.Items.Count == 0)
            {
                visualizeTargetStudyComboBox.Text = string.Empty;
            }
        }

        private void UpdateExistingStudyComboBox()
        {
            if (existingStudyComboBox.Items.Count > 0 && existingStudyComboBox.Items.Count - 1 < existingStudyComboBox.SelectedIndex)
            {
                existingStudyComboBox.SelectedIndex = 0;
                cmaEsWarmStartComboBox.SelectedIndex = 0;
            }
            else if (existingStudyComboBox.Items.Count == 0)
            {
                existingStudyComboBox.Text = string.Empty;
                continueStudyCheckBox.Checked = false;
                cmaEsWarmStartComboBox.Text = string.Empty;
                cmaEsWarmStartCmaEsCheckBox.Checked = false;
            }
        }

        private void OptimizeProgressChangedHandler(object sender, ProgressChangedEventArgs e)
        {
            var pState = (ProgressState)e.UserState;
            _component.UpdateGrasshopper(pState.Parameter);
            const string trialNumLabel = "Trial: ";
            optimizeTrialNumLabel.Text = e.ProgressPercentage == 100
                ? trialNumLabel + "#"
                : trialNumLabel + (pState.TrialNumber + 1);
            SetBestValues(e, pState);

            EstimatedTimeRemainingLabel.Text = pState.EstimatedTimeRemaining.TotalSeconds > 0
                ? "Estimated Time Remaining: " + new DateTime(0).Add(pState.EstimatedTimeRemaining).ToString("HH:mm:ss", CultureInfo.InvariantCulture)
                : "Estimated Time Remaining: 00:00:00";
            optimizeProgressBar.Value = e.ProgressPercentage;
            optimizeProgressBar.Update();
        }

        private void SetBestValues(ProgressChangedEventArgs e, ProgressState pState)
        {
            if (pState.BestValues == null)
            {
                optimizeBestValueLabel.Text = @"BestValue: #";
            }
            else if (e.ProgressPercentage == 0 || e.ProgressPercentage == 100)
            {
                optimizeBestValueLabel.Text = pState.ObjectiveNum == 1
                    ? @"BestValue: #"
                    : @"Hypervolume Ratio: #";
            }
            else if (pState.BestValues.Length > 0)
            {
                optimizeBestValueLabel.Text = pState.ObjectiveNum == 1
                    ? $"BestValue: {pState.BestValues[0][0]:e4}"
                    : $"Hypervolume Ratio: {pState.HypervolumeRatio:0.000}";
            }
        }
    }
}
