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
using Tunny.Settings;
using Tunny.Solver;

namespace Tunny.UI
{
    public partial class OptimizationWindow : Form
    {
        private readonly TunnyComponent _component;
        private TunnySettings _settings;
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
            LoadSettingJson();
            InitializeUIValues();

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

        private void LoadSettingJson()
        {
            string settingsPath = _component.GhInOut.ComponentFolder + @"\Settings.json";
            if (File.Exists(settingsPath))
            {
                _settings = TunnySettings.Deserialize(File.ReadAllText(settingsPath));
            }
            else
            {
                _settings = new TunnySettings
                {
                    Storage = _component.GhInOut.ComponentFolder + @"\Tunny_Opt_Result.db"
                };
                _settings.CreateNewSettingsFile(settingsPath);
            }
        }
        private void InitializeUIValues()
        {
            samplerComboBox.SelectedIndex = _settings.Optimize.SelectSampler;
            nTrialNumUpDown.Value = _settings.Optimize.NumberOfTrials;
            loadIfExistsCheckBox.Checked = _settings.Optimize.LoadExistStudy;
            studyNameTextBox.Text = _settings.StudyName;
            restoreModelNumTextBox.Text = _settings.Result.RestoreNumberString;
            visualizeTypeComboBox.SelectedIndex = _settings.Result.SelectVisualizeType;
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
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
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
            SaveUIValues();

            if (optimizeBackgroundWorker != null)
            {
                optimizeBackgroundWorker.CancelAsync();
            }

            if (restoreBackgroundWorker != null)
            {
                restoreBackgroundWorker.CancelAsync();
            }
        }

        private void SaveUIValues()
        {
            _settings.Optimize.SelectSampler = samplerComboBox.SelectedIndex;
            _settings.Optimize.NumberOfTrials = (int)nTrialNumUpDown.Value;
            _settings.Optimize.LoadExistStudy = loadIfExistsCheckBox.Checked;
            _settings.StudyName = studyNameTextBox.Text;
            _settings.Result.RestoreNumberString = restoreModelNumTextBox.Text;
            _settings.Result.SelectVisualizeType = visualizeTypeComboBox.SelectedIndex;
            _settings.Serialize(_component.GhInOut.ComponentFolder + @"\Settings.json");
        }

        private void RestoreRunButton_Click(object sender, EventArgs e)
        {
            optimizeRunButton.Enabled = false;
            optimizeStopButton.Enabled = true;

            RestoreLoop.StudyName = studyNameTextBox.Text;
            RestoreLoop.Mode = "Restore";
            RestoreLoop.NickNames = _component.GhInOut.Variables.Select(x => x.NickName).ToArray();
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
            switch (RestoreLoop.Mode)
            {
                case "Restore":
                    _component.ExpireSolution(true);
                    break;
                case "Reflect":
                    var decimalVar = _component.CFishes[0].Variables
                            .Select(x => (decimal)x.Value).ToList();
                    _component.GhInOut.NewSolution(decimalVar);
                    break;
            }
        }

        private void RestoreReflectButton_Click(object sender, EventArgs e)
        {
            optimizeRunButton.Enabled = false;
            optimizeStopButton.Enabled = true;

            RestoreLoop.StudyName = studyNameTextBox.Text;
            RestoreLoop.Mode = "Reflect";
            RestoreLoop.NickNames = _component.GhInOut.Variables.Select(x => x.NickName).ToArray();
            RestoreLoop.Indices = restoreModelNumTextBox.Text.Split(',').Select(int.Parse).ToArray();

            restoreBackgroundWorker.RunWorkerAsync(_component);

        }

        private void RestoreProgressChangedHandler(object sender, ProgressChangedEventArgs e)
        {
            restoreProgressBar.Value = e.ProgressPercentage;
            restoreProgressBar.Update();
        }

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

        private void SettingsOpenAPIPage_Click(object sender, EventArgs e)
        {
            int apiIndex = settingsAPIComboBox.SelectedIndex;
            switch (apiIndex)
            {
                case 0: // TPE
                    Process.Start("https://optuna.readthedocs.io/en/stable/reference/generated/optuna.samplers.TPESampler.html");
                    break;
                case 1: // NSGA2
                    Process.Start("https://optuna.readthedocs.io/en/stable/reference/generated/optuna.samplers.NSGAIISampler.html");
                    break;
                case 2: // CMA-ES
                    Process.Start("https://optuna.readthedocs.io/en/stable/reference/generated/optuna.samplers.CmaEsSampler.html");
                    break;
                case 3: // Random
                    Process.Start("https://optuna.readthedocs.io/en/stable/reference/generated/optuna.samplers.RandomSampler.html");
                    break;
            }
        }

        private void SettingsFromJson_Click(object sender, EventArgs e)
        {
            LoadSettingJson();
            InitializeUIValues();
        }

        private void SettingsToJson_Click(object sender, EventArgs e)
        {
            SaveUIValues();
        }

        private void SettingsFolderOpen_Click(object sender, EventArgs e)
        {
            SaveUIValues();
            Process.Start("EXPLORER.EXE", _component.GhInOut.ComponentFolder);
        }
    }
}
