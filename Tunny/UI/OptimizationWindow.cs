using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

using Grasshopper.GUI;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;

using Rhino.FileIO;
using Rhino.Geometry;

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

            if (_component.GhInOut.GetObjectiveValues().Count > 1 && (samplerComboBox.Text == "CMA-ES" || samplerComboBox.Text == "Random"))
            {
                MessageBox.Show("This sampler does not support multiple objectives optimization.", "Tunny", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ghCanvas.EnableUI();
                runOptimizeButton.Enabled = true;
                return;
            }

            backgroundWorkerSolver.RunWorkerAsync(_component);

            stopButton.Enabled = true;
        }

        private void ButtonStop_Click(object sender, EventArgs e)
        {
            runOptimizeButton.Enabled = true;
            stopButton.Enabled = false;

            //Enable GUI
            var ghCanvas = Owner as GH_DocumentEditor;
            ghCanvas?.EnableUI();
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

        private void RestoreButton_Click(object sender, EventArgs e)
        {
            var modelMesh = new GH_Structure<GH_Mesh>();
            var variables = new GH_Structure<GH_Number>();
            var objectives = new GH_Structure<GH_Number>();
            var nickName = _component.GhInOut.Sliders.Select(x => x.NickName).ToArray();

            var optuna = new Optuna(_component.GhInOut.ComponentFolder);
            string studyName = studyNameTextBox.Text;

            int[] num = restoreModelNumTextBox.Text.Split(',').Select(int.Parse).ToArray();
            ModelResult[] modelResult = optuna.GetModelResult(num, studyName);
            foreach (ModelResult model in modelResult)
            {
                SetVariables(variables, model, nickName);
                SetObjectives(objectives, model);
                SetModelMesh(modelMesh, model);
            }
            _component.Variables = variables;
            _component.Objectives = objectives;
            _component.ModelMesh = modelMesh;
            _component.ExpireSolution(true);
        }

        private static void SetVariables(GH_Structure<GH_Number> objectives, ModelResult model, IEnumerable<string> nickName)
        {
            foreach (string name in nickName)
            {
                foreach (var obj in model.Variables.Where(obj => obj.Key == name))
                {
                    objectives.Append(new GH_Number(obj.Value), new GH_Path(0, model.Number));
                }
            }
        }

        private static void SetObjectives(GH_Structure<GH_Number> objectives, ModelResult model)
        {
            foreach (double obj in model.Objectives)
            {
                objectives.Append(new GH_Number(obj), new GH_Path(0, model.Number));
            }
        }

        private static void SetModelMesh(GH_Structure<GH_Mesh> modelMesh, ModelResult model)
        {
            if (model.Draco == string.Empty)
            {
                return;
            }
            var mesh = (Mesh)DracoCompression.DecompressBase64String(model.Draco);
            modelMesh.Append(new GH_Mesh(mesh), new GH_Path(0, model.Number));
        }

        private void FormClosingXButton(object sender, FormClosingEventArgs e)
        {
            var ghCanvas = Owner as GH_DocumentEditor;
            ghCanvas?.EnableUI();
        }
    }
}