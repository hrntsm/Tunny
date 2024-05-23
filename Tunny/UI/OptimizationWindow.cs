using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

using Grasshopper.GUI;

using Python.Runtime;

using Serilog.Events;

using Tunny.Component.Optimizer;
using Tunny.Core.Input;
using Tunny.Core.Settings;
using Tunny.Core.TEnum;
using Tunny.Core.Util;
using Tunny.Handler;

namespace Tunny.UI
{
    public partial class OptimizationWindow : Form
    {
        private readonly FishingComponent _component;
        private readonly TSettings _settings;
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
            _settings = TSettings.LoadFromJson();
            SetUIValues();
            RunPythonInstaller();
            SetOptimizeBackgroundWorker();
            SetOutputResultBackgroundWorker();
        }

        private void RunPythonInstaller()
        {
            TLog.MethodStart();
            string tunnyAssembleVersion = TEnvVariables.Version.ToString();
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
                samplerComboBox.SelectedIndex = (int)SamplerType.GP; // GP
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
            ignoreDuplicateSamplingCheckBox.Checked = _settings.Optimize.IgnoreDuplicateSampling;
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
            _settings.Optimize.IgnoreDuplicateSampling = ignoreDuplicateSamplingCheckBox.Checked;
            _settings.Optimize.Sampler = GetSamplerSettings(_settings.Optimize.Sampler);
            _settings.Optimize.GcAfterTrial = (GcAfterTrial)runGarbageCollectionComboBox.SelectedIndex;
            _settings.LogLevel = (LogEventLevel)miscLogComboBox.SelectedIndex;
        }

        private void TTDesignExplorerButton_Click(object sender, EventArgs e)
        {
            string envPath = Path.Combine(TEnvVariables.TunnyEnvPath, "python", @"python310.dll");
            Environment.SetEnvironmentVariable("PYTHONNET_PYDLL", envPath, EnvironmentVariableTarget.Process);
            if (PythonEngine.IsInitialized)
            {
                PythonEngine.Shutdown();
                TLog.Warning("PythonEngine is unintentionally initialized and therefore shut it down.");
            }
            PythonEngine.Initialize();
            using (Py.GIL())
            {
                PyModule ps = Py.CreateScope();
                ps.Exec(
"def export_t4de_csv(storage, target_study_name, output_path):\n" +
"    import optuna\n" +
"    import csv\n" +
"    import json\n" +

"    study = optuna.load_study(storage=storage, study_name=target_study_name)\n" +
"    s_id = (str)(study._study_id)\n" +
"    trial = study.get_trials(deepcopy=False, states=[optuna.trial.TrialState.COMPLETE])\n" +
"    metric_names = study.metric_names\n" +
"    param_keys = list(trial[0].params.keys())\n" +
"    artifact_path = 'http://127.0.0.1:8080/artifacts/' + s_id + '/'\n" +

"    label = []\n" +

"    for key in param_keys:\n" +
"        label.append('in:' + key)\n" +

"    if metric_names is not None:\n" +
"        for name in metric_names:\n" +
"            label.append('out:' + name)\n" +
"    for attr in trial[0].system_attrs:\n" +
"        if attr.startswith('artifacts:'):\n" +
"            artifact_value = trial[0].system_attrs[attr]\n" +
"            j = json.loads(artifact_value)\n" +
"            if j['mimetype'] == 'image/png':\n" +
"                label.append('img')\n" +

"    with open(output_path + '/fish2DesignExplorer.csv', 'w', newline='') as f:\n" +
"        writer = csv.writer(f)\n" +
"        writer.writerow(label)\n" +

"        for t in trial:\n" +
"            t_id = (str)(t.number)\n" +
"            row = []\n" +
"            for key in param_keys:\n" +
"                row.append(t.params[key])\n" +

"            for v in t.values:\n" +
"                row.append(v)\n" +

"            for attr in t.system_attrs:\n" +
"                if attr.startswith('artifacts:'):\n" +
"                    artifact_value = t.system_attrs[attr]\n" +
"                    j = json.loads(artifact_value)\n" +
"                    if j['mimetype'] == 'image/png':\n" +
"                        row.append(artifact_path + t_id + '/' + j['artifact_id'])\n" +
"                        break\n" +

"            writer.writerow(row)\n"
                );
                dynamic storage = _settings.Storage.CreateNewOptunaStorage(false);
                string target = visualizeTargetStudyComboBox.Text;
                dynamic func = ps.Get("export_t4de_csv");
                string csvPath = Path.GetDirectoryName(_settings.Storage.Path);
                func(storage, target, csvPath);

                var designExplorer = new Process();
                string path = Path.Combine(TEnvVariables.DesignExplorerPath, "index.html");
                designExplorer.StartInfo.FileName = path;
                designExplorer.StartInfo.UseShellExecute = true;
                designExplorer.Start();
            }
        }
    }
}
