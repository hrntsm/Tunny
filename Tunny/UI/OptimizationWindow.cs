using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

using Grasshopper.GUI;

using Tunny.Component;
using Tunny.Optimization;
using Tunny.Solver;

namespace Tunny.UI
{
    public partial class OptimizationWindow : Form
    {
        private readonly TunnyComponent _component;
        internal enum GrasshopperStates
        {
            RequestSent,
            RequestProcessing,
            RequestProcessed
        }
        internal GrasshopperStates GrasshopperStatus;

        public OptimizationWindow(TunnyComponent component)
        {
            InitializeComponent();
            _component = component;
            _component.GhInOutInstantiate();
            samplerComboBox.SelectedIndex = 0;
            visualizeTypeComboBox.SelectedIndex = 3;

            optimizeBackgroundWorker.DoWork += OptimizeLoop.RunMultiple;
            optimizeBackgroundWorker.ProgressChanged += OptimizeProgressChangedHandler;
            optimizeBackgroundWorker.RunWorkerCompleted += OptimizeStopButton_Click;
            optimizeBackgroundWorker.WorkerReportsProgress = true;
            optimizeBackgroundWorker.WorkerSupportsCancellation = true;

            restoreBackgroundWorker.DoWork += RestoreLoop.Run;
            restoreBackgroundWorker.ProgressChanged += RestoreProgressChangedHandler;
            restoreBackgroundWorker.RunWorkerCompleted += RestoreStopButton_Click;
            restoreBackgroundWorker.WorkerReportsProgress = true;
            restoreBackgroundWorker.WorkerSupportsCancellation = true;
        }

        private void UpdateGrasshopper(IList<decimal> parameters)
        {
            GrasshopperStatus = GrasshopperStates.RequestProcessing;

            //Calculate Grasshopper
            _component.GhInOut.NewSolution(parameters);

            GrasshopperStatus = GrasshopperStates.RequestProcessed;
        }

        private void OptimizationWindow_Load(object sender, EventArgs e)
        {
        }

        private void VisualizeButton_Click(object sender, EventArgs e)
        {
            var optuna = new Optuna(_component.GhInOut.ComponentFolder);
            optuna.ShowResultVisualize(visualizeTypeComboBox.Text, studyNameTextBox.Text);
        }

        private void OpenResultFolderButton_Click(object sender, EventArgs e)
        {
            Process.Start("EXPLORER.EXE", _component.GhInOut.ComponentFolder);
        }

        private void ClearResultButton_Click(object sender, EventArgs e)
        {
            File.Delete(_component.GhInOut.ComponentFolder + "/Tunny_Opt_Result.db");
        }

        private void FormClosingXButton(object sender, FormClosingEventArgs e)
        {
            var ghCanvas = Owner as GH_DocumentEditor;
            ghCanvas?.EnableUI();

            if (optimizeBackgroundWorker != null)
            {
                optimizeBackgroundWorker.CancelAsync();
            }

            if (restoreBackgroundWorker != null)
            {
                restoreBackgroundWorker.CancelAsync();
            }
        }

        private void RestoreRunButton_Click(object sender, EventArgs e)
        {
            optimizeRunButton.Enabled = false;
            optimizeStopButton.Enabled = true;

            RestoreLoop.StudyName = studyNameTextBox.Text;
            RestoreLoop.NickNames = _component.GhInOut.Sliders.Select(x => x.NickName).ToArray();
            RestoreLoop.Indices = restoreModelNumTextBox.Text.Split(',').Select(int.Parse).ToArray();

            restoreBackgroundWorker.RunWorkerAsync(_component);
        }

        private void RestoreStopButton_Click(object sender, EventArgs e)
        {
            optimizeRunButton.Enabled = true;
            optimizeStopButton.Enabled = false;

            if (restoreBackgroundWorker != null)
            {
                restoreBackgroundWorker.CancelAsync();
            }
            _component.ExpireSolution(true);
        }

        private void RestoreProgressChangedHandler(object sender, ProgressChangedEventArgs e)
        {
            restoreProgressBar.Value = e.ProgressPercentage;
            restoreProgressBar.Update();
        }

        private void OptimizeRunButton_Click(object sender, EventArgs e)
        {
            GH_DocumentEditor ghCanvas = Owner as GH_DocumentEditor;
            ghCanvas.DisableUI();

            optimizeRunButton.Enabled = false;
            OptimizeLoop.NTrials = (int)nTrialNumUpDown.Value;
            OptimizeLoop.LoadIfExists = loadIfExistsCheckBox.Checked;
            OptimizeLoop.SamplerType = samplerComboBox.Text;
            OptimizeLoop.StudyName = studyNameTextBox.Text;

            var objectiveValues = _component.GhInOut.GetObjectiveValues();
            if (objectiveValues.Count == 0)
            {
                ghCanvas.EnableUI();
                optimizeRunButton.Enabled = true;
                return;
            }
            else if (objectiveValues.Count > 1 && (samplerComboBox.Text == "CMA-ES" || samplerComboBox.Text == "Random"))
            {
                MessageBox.Show("This sampler does not support multiple objectives optimization.", "Tunny", MessageBoxButtons.OK, MessageBoxIcon.Error);
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