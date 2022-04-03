using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

using BayesOpt.Component;
using BayesOpt.Util;

using Grasshopper.GUI;

namespace BayesOpt.UI
{
    public partial class OptimizationWindow : Form
    {
        private readonly WithUI _component;
        internal enum GrasshopperStates
        {
            RequestSent,
            RequestProcessing,
            RequestProcessed
        }
        internal GrasshopperStates GrasshopperStatus;

        public OptimizationWindow(WithUI component)
        {
            InitializeComponent();
            _component = component;

            backgroundWorkerSolver.DoWork += Loop.RunOptimizationLoopMultiple;
            backgroundWorkerSolver.ProgressChanged += ProgressChangedHandler;
            backgroundWorkerSolver.RunWorkerCompleted += ButtonStop_Click;
            backgroundWorkerSolver.WorkerReportsProgress = true;
            backgroundWorkerSolver.WorkerSupportsCancellation = true;
        }

        private void ProgressChangedHandler(object sender, ProgressChangedEventArgs e)
        {
            switch (e.ProgressPercentage)
            {
                case 0:
                    var parameters = (IList<decimal>)e.UserState;
                    UpdateGrasshopper(parameters);
                    break;
            }
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

            RunOptimize.Enabled = false;

            backgroundWorkerSolver.RunWorkerAsync(_component);

            Stop.Enabled = true;
        }

        private void ButtonStop_Click(object sender, EventArgs e)
        {
            RunOptimize.Enabled = true;
            Stop.Enabled = false;

            //Enable GUI
            GH_DocumentEditor ghCanvas = Owner as GH_DocumentEditor;
            ghCanvas.EnableUI();
        }
    }
}