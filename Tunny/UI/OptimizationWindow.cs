using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

using Tunny.Component;
using Tunny.Solver;
using Tunny.Util;

using Grasshopper.GUI;

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
            samplerComboBox.SelectedIndex = 0;
            visualizeTypeComboBox.SelectedIndex = 3;

            backgroundWorkerSolver.DoWork += Loop.RunOptimizationLoopMultiple;
            backgroundWorkerSolver.ProgressChanged += ProgressChangedHandler;
            backgroundWorkerSolver.RunWorkerCompleted += ButtonStop_Click;
            backgroundWorkerSolver.WorkerReportsProgress = true;
            backgroundWorkerSolver.WorkerSupportsCancellation = true;
        }

        private void ProgressChangedHandler(object sender, ProgressChangedEventArgs e)
        {
            var parameters = (IList<decimal>)e.UserState;
            UpdateGrasshopper(parameters);

            progressBar.Value = e.ProgressPercentage;
            progressBar.Update();
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

        private void ButtonRunOptimize_Click(object sender, EventArgs e)
        {
            GH_DocumentEditor ghCanvas = Owner as GH_DocumentEditor;
            ghCanvas.DisableUI();

            runOptimizeButton.Enabled = false;
            Loop.NTrials = (int)nTrialNumUpDown.Value;
            Loop.LoadIfExists = loadIfExistsCheckBox.Checked;
            Loop.SamplerType = samplerComboBox.Text;
            Loop.StudyName = studyNameTextBox.Text;

            backgroundWorkerSolver.RunWorkerAsync(_component);

            stopButton.Enabled = true;
        }

        private void ButtonStop_Click(object sender, EventArgs e)
        {
            runOptimizeButton.Enabled = true;
            stopButton.Enabled = false;

            //Enable GUI
            GH_DocumentEditor ghCanvas = Owner as GH_DocumentEditor;
            ghCanvas.EnableUI();
        }

        private void VisualizeButton_Click(object sender, EventArgs e)
        {
            var optuna = new OptunaTPE(_component.GhInOut.ComponentFolder);
            optuna.ShowResult(visualizeTypeComboBox.Text, studyNameTextBox.Text);
        }
    }
}