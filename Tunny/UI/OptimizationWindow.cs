using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

using Grasshopper.GUI;

using Tunny.Component;
using Tunny.Handler;
using Tunny.Settings;

namespace Tunny.UI
{
    public partial class OptimizationWindow : Form
    {
        private readonly FishingComponent _component;
        private TunnySettings _settings;
        internal enum GrasshopperStates
        {
            RequestSent,
            RequestProcessing,
            RequestProcessed
        }
        internal GrasshopperStates GrasshopperStatus;

        public OptimizationWindow(FishingComponent component)
        {
            InitializeComponent();

            _component = component;
            _component.GhInOutInstantiate();
            LoadSettingJson();
            InitializeUIValues();

            PythonInstaller.Path = _component.GhInOut.ComponentFolder;
            if (_settings.CheckPythonLibraries && !PythonInstaller.CheckPackagesIsInstalled())
            {
                var installer = new PythonInstallDialog()
                {
                    StartPosition = FormStartPosition.CenterScreen
                };
                installer.Show(Owner);
            }

            optimizeBackgroundWorker.DoWork += OptimizeLoop.RunMultiple;
            optimizeBackgroundWorker.ProgressChanged += OptimizeProgressChangedHandler;
            optimizeBackgroundWorker.RunWorkerCompleted += OptimizeStopButton_Click;
            optimizeBackgroundWorker.WorkerReportsProgress = true;
            optimizeBackgroundWorker.WorkerSupportsCancellation = true;

            outputResultBackgroundWorker.DoWork += OutputLoop.Run;
            outputResultBackgroundWorker.ProgressChanged += OutputProgressChangedHandler;
            outputResultBackgroundWorker.RunWorkerCompleted += OutputStopButton_Click;
            outputResultBackgroundWorker.WorkerReportsProgress = true;
            outputResultBackgroundWorker.WorkerSupportsCancellation = true;
        }

        public void BGDispose()
        {
            optimizeBackgroundWorker.Dispose();
            outputResultBackgroundWorker.Dispose();
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
                    StoragePath = _component.GhInOut.ComponentFolder + @"\Fish.db"
                };
                _settings.CreateNewSettingsFile(settingsPath);
            }
        }

        private void InitializeUIValues()
        {
            samplerComboBox.SelectedIndex = _settings.Optimize.SelectSampler;
            nTrialNumUpDown.Value = _settings.Optimize.NumberOfTrials;
            timeoutNumUpDown.Value = (decimal)_settings.Optimize.Timeout;

            // Study Name GroupBox
            studyNameTextBox.Text = _settings.StudyName;
            continueStudyCheckBox.Checked = _settings.Optimize.ContinueStudy;
            existingStudyComboBox.Enabled = continueStudyCheckBox.Checked;
            studyNameTextBox.Enabled = !continueStudyCheckBox.Checked;
            copyStudyCheckBox.Enabled = _settings.Optimize.CopyStudy;
            UpdateStudyComboBox();

            outputModelNumTextBox.Text = _settings.Result.OutputNumberString;
            visualizeTypeComboBox.SelectedIndex = _settings.Result.SelectVisualizeType;
            visualizeClusterNumUpDown.Value = _settings.Result.NumberOfClusters;
            InitializeSamplerSettings();
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

        private void FormClosingXButton(object sender, FormClosingEventArgs e)
        {
            var ghCanvas = Owner as GH_DocumentEditor;
            ghCanvas?.EnableUI();
            GetUIValues();
            _settings.Serialize(_component.GhInOut.ComponentFolder + @"\Settings.json");

            //TODO: use cancelAsync to stop the background worker safely
            optimizeBackgroundWorker?.Dispose();
            outputResultBackgroundWorker?.Dispose();
        }

        private void GetUIValues()
        {
            _settings.Optimize.SelectSampler = samplerComboBox.SelectedIndex;
            _settings.Optimize.NumberOfTrials = (int)nTrialNumUpDown.Value;
            _settings.Optimize.Timeout = (double)timeoutNumUpDown.Value;
            _settings.Optimize.ContinueStudy = continueStudyCheckBox.Checked;
            _settings.Optimize.CopyStudy = copyStudyCheckBox.Checked;
            _settings.StudyName = studyNameTextBox.Text;
            _settings.Result.OutputNumberString = outputModelNumTextBox.Text;
            _settings.Result.SelectVisualizeType = visualizeTypeComboBox.SelectedIndex;
            _settings.Result.NumberOfClusters = (int)visualizeClusterNumUpDown.Value;
            _settings.Optimize.Sampler = GetSamplerSettings();
        }
    }
}
