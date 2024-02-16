using System;
using System.Collections.Generic;
using System.Windows.Forms;

using Grasshopper.GUI;

using Serilog.Events;

using Tunny.Component.Optimizer;
using Tunny.Core;
using Tunny.Enum;
using Tunny.Handler;
using Tunny.Input;
using Tunny.Settings;
using Tunny.Util;

namespace Tunny.UI
{
    public partial class OptimizationWindow : Form
    {
        private readonly FishingComponent _component;
        private readonly TunnySettings _settings;
        internal GrasshopperStates GrasshopperStatus;

        public OptimizationWindow(FishingComponent component)
        {
            TLog.MethodStart();
            TLog.Info("OptimizationWindow is open");
            InitializeComponent();

            _component = component;
            _component.GhInOutInstantiate();
            if (!_component.GhInOut.IsLoadCorrectly)
            {
                FormClosingXButton(this, null);
            }
            _settings = TunnySettings.LoadFromJson();
            SetUIValues();
            RunPythonInstaller();
            SetOptimizeBackgroundWorker();
            SetOutputResultBackgroundWorker();
        }

        private void RunPythonInstaller()
        {
            TLog.MethodStart();
            string tunnyAssembleVersion = TEnvVariables.Version.ToString(3);
            if (_settings.CheckPythonLibraries || _settings.Version != tunnyAssembleVersion)
            {
                TLog.Info("Run Python installer");
                var installer = new PythonInstallDialog()
                {
                    StartPosition = FormStartPosition.CenterScreen
                };
                installer.Show(Owner);
                _settings.Version = tunnyAssembleVersion;
                _settings.CheckPythonLibraries = false;
                checkPythonLibrariesCheckBox.Checked = false;
            }
            else
            {
                TLog.Info("Skip Python installer");
            }
        }

        private void SetOptimizeBackgroundWorker()
        {
            TLog.MethodStart();
            optimizeBackgroundWorker.DoWork += OptimizeLoop.RunMultiple;
            optimizeBackgroundWorker.ProgressChanged += OptimizeProgressChangedHandler;
            optimizeBackgroundWorker.RunWorkerCompleted += OptimizeStopButton_Click;
            optimizeBackgroundWorker.WorkerReportsProgress = true;
            optimizeBackgroundWorker.WorkerSupportsCancellation = true;
        }

        private void SetOutputResultBackgroundWorker()
        {
            TLog.MethodStart();
            outputResultBackgroundWorker.DoWork += OutputLoop.Run;
            outputResultBackgroundWorker.ProgressChanged += OutputProgressChangedHandler;
            outputResultBackgroundWorker.RunWorkerCompleted += OutputStopButton_Click;
            outputResultBackgroundWorker.WorkerReportsProgress = true;
            outputResultBackgroundWorker.WorkerSupportsCancellation = true;
        }

        public void BGDispose()
        {
            TLog.MethodStart();
            optimizeBackgroundWorker.Dispose();
            outputResultBackgroundWorker.Dispose();
        }


        private void UpdateGrasshopper(IList<Parameter> parameters)
        {
            TLog.MethodStart();
            GrasshopperStatus = GrasshopperStates.RequestProcessing;

            //Calculate Grasshopper
            _component.GhInOut.NewSolution(parameters);

            GrasshopperStatus = GrasshopperStates.RequestProcessed;
        }

        private void OptimizationWindow_Load(object sender, EventArgs e)
        {
            TLog.MethodStart();
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
        }

        private void FormClosingXButton(object sender, FormClosingEventArgs e)
        {
            TLog.MethodStart();
            var ghCanvas = Owner as GH_DocumentEditor;
            ghCanvas?.EnableUI();
            GetUIValues();
            _settings.Serialize(TEnvVariables.OptimizeSettingsPath);

            //TODO: use cancelAsync to stop the background worker safely
            optimizeBackgroundWorker?.Dispose();
            outputResultBackgroundWorker?.Dispose();
        }

        private void SetUIValues()
        {
            TLog.MethodStart();
            TLog.Info("Set UI values");
            HumanInTheLoopType type = _component.GhInOut.Objectives.HumanInTheLoopType;
            if (type != HumanInTheLoopType.None)
            {
                TLog.Info("Set Tunny Human-in-the-loop mode");
                Text = "Tunny (Human in the Loop mode)";
                samplerComboBox.SelectedIndex = 1; // GP
                samplerComboBox.Enabled = false;
                nTrialText.Text = "Number of batches";
                nTrialNumUpDown.Value = type == HumanInTheLoopType.Preferential ? 6 : 4;
                timeoutNumUpDown.Value = 0;
                timeoutNumUpDown.Enabled = false;
            }
            else
            {
                TLog.Info("Set Tunny normal optimization mode");
                Text = "Tunny";
                samplerComboBox.Enabled = true;
                samplerComboBox.SelectedIndex = (int)_settings.Optimize.SelectSampler;
                nTrialText.Text = "Number of trials";
                nTrialNumUpDown.Value = _settings.Optimize.NumberOfTrials;
                timeoutNumUpDown.Enabled = true;
                timeoutNumUpDown.Value = (decimal)_settings.Optimize.Timeout;
            }

            // Study Name GroupBox
            studyNameTextBox.Text = _settings.StudyName;
            continueStudyCheckBox.Checked = _settings.Optimize.ContinueStudy;
            existingStudyComboBox.Enabled = continueStudyCheckBox.Checked;
            studyNameTextBox.Enabled = !continueStudyCheckBox.Checked;
            copyStudyCheckBox.Enabled = _settings.Optimize.CopyStudy;
            UpdateStudyComboBox();
            ShowRealtimeResultCheckBox.Checked = _settings.Optimize.ShowRealtimeResult;

            outputModelNumTextBox.Text = _settings.Result.OutputNumberString;
            visualizeTypeComboBox.SelectedIndex = _settings.Result.SelectVisualizeType;
            visualizeClusterNumUpDown.Value = _settings.Result.NumberOfClusters;
            InitializeSamplerSettings();

            runGarbageCollectionComboBox.SelectedIndex = (int)_settings.Optimize.GcAfterTrial;
            miscLogComboBox.SelectedIndex = (int)_settings.LogLevel;
        }
        private void GetUIValues()
        {
            TLog.MethodStart();
            _settings.Optimize.SelectSampler = (SamplerType)samplerComboBox.SelectedIndex;
            _settings.Optimize.NumberOfTrials = (int)nTrialNumUpDown.Value;
            _settings.Optimize.Timeout = (double)timeoutNumUpDown.Value;
            _settings.Optimize.ContinueStudy = continueStudyCheckBox.Checked;
            _settings.Optimize.CopyStudy = copyStudyCheckBox.Checked;
            _settings.Optimize.ShowRealtimeResult = ShowRealtimeResultCheckBox.Checked;
            _settings.Storage.Type = inMemoryCheckBox.Checked ? StorageType.InMemory : _settings.Storage.Type;
            _settings.StudyName = studyNameTextBox.Text;
            _settings.Result.OutputNumberString = outputModelNumTextBox.Text;
            _settings.Result.SelectVisualizeType = visualizeTypeComboBox.SelectedIndex;
            _settings.Result.NumberOfClusters = (int)visualizeClusterNumUpDown.Value;
            _settings.CheckPythonLibraries = checkPythonLibrariesCheckBox.Checked;
            _settings.Optimize.Sampler = GetSamplerSettings(_settings.Optimize.Sampler);
            _settings.Optimize.GcAfterTrial = (GcAfterTrial)runGarbageCollectionComboBox.SelectedIndex;
            _settings.LogLevel = (LogEventLevel)miscLogComboBox.SelectedIndex;
        }
    }
}
