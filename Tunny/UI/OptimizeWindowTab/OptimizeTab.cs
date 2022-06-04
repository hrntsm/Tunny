using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

using Grasshopper.GUI;

using Tunny.Optimization;

namespace Tunny.UI
{
    public partial class OptimizationWindow : Form
    {
        private void OptimizeRunButton_Click(object sender, EventArgs e)
        {
            var ghCanvas = Owner as GH_DocumentEditor;
            ghCanvas.DisableUI();

            optimizeRunButton.Enabled = false;
            _settings.Optimize.NumberOfTrials = (int)nTrialNumUpDown.Value;
            _settings.Optimize.SelectSampler = samplerComboBox.SelectedIndex;
            _settings.StudyName = studyNameTextBox.Text;
            OptimizeLoop.Settings = _settings;

            List<double> objectiveValues = _component.GhInOut.GetObjectiveValues();
            if (objectiveValues.Count == 0)
            {
                ghCanvas.EnableUI();
                optimizeRunButton.Enabled = true;
                return;
            }
            else if (objectiveValues.Count > 1
                     && (samplerComboBox.Text == "CMA-ES" || samplerComboBox.Text == "Random" || samplerComboBox.Text == "Grid"))
            {
                TunnyMessageBox.Show(
                    "CMA-ES, Random and Grid samplers only support single objective optimization.",
                    "Tunny",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                ghCanvas.EnableUI();
                optimizeRunButton.Enabled = true;
                return;
            }

            optimizeBackgroundWorker.RunWorkerAsync(_component);
            optimizeStopButton.Enabled = true;
        }

        private void OptimizeStopButton_Click(object sender, EventArgs e)
        {
            optimizeRunButton.Enabled = true;
            optimizeStopButton.Enabled = false;

            if (optimizeBackgroundWorker != null)
            {
                optimizeBackgroundWorker.CancelAsync();
            }

            //Enable GUI
            var ghCanvas = Owner as GH_DocumentEditor;
            ghCanvas?.EnableUI();
        }

        private void OptimizeProgressChangedHandler(object sender, ProgressChangedEventArgs e)
        {
            var parameters = (IList<decimal>)e.UserState;
            UpdateGrasshopper(parameters);

            optimizeProgressBar.Value = e.ProgressPercentage;
            optimizeProgressBar.Update();
        }
    }
}
