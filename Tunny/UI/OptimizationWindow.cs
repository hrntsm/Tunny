using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

using Grasshopper.GUI;

using Tunny.Component;
using Tunny.Optimization;
using Tunny.Settings;
using Tunny.Util;

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

            PythonInstaller.Path = _component.GhInOut.ComponentFolder;
            if (!PythonInstaller.CheckPackagesIsInstalled())
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
    }
}
