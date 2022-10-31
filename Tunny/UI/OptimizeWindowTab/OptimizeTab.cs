using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

using Grasshopper.GUI;

using Tunny.Handler;

namespace Tunny.UI
{
    public partial class OptimizationWindow : Form
    {
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

            return true;
        }

        private void OptimizeStopButton_Click(object sender, EventArgs e)
        {
            optimizeRunButton.Enabled = true;
            optimizeStopButton.Enabled = false;
            OptimizeLoop.IsForcedStopOptimize = true;

            if (optimizeBackgroundWorker != null)
            {
                optimizeBackgroundWorker.Dispose();
            }

            //Enable GUI
            var ghCanvas = Owner as GH_DocumentEditor;
            ghCanvas?.EnableUI();
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
