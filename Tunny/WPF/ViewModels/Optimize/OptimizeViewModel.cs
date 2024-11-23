using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using Prism.Commands;
using Prism.Mvvm;

using Rhino.Display;

using Tunny.Core.Handler;
using Tunny.Core.Settings;
using Tunny.Core.TEnum;
using Tunny.Core.Util;
using Tunny.Process;
using Tunny.WPF.Common;
using Tunny.WPF.Models;
using Tunny.WPF.Views.Pages.Optimize;
using Tunny.WPF.Views.Pages.Settings.Sampler;

namespace Tunny.WPF.ViewModels.Optimize
{
    internal class OptimizeViewModel : BindableBase
    {
        private const string SamplerTypeLabelPrefix = "SamplerType: ";
        private const string TrialNumberLabelPrefix = "Trial: ";
        private readonly TSettings _settings;
        private ProgressBar _progressBar;
        private LiveChartPage _chart1;
        private LiveChartPage _chart2;
        private TPESettingsPage _tpePage;
        private GPOptunaSettingsPage _gpOptunaPage;
        private GPBoTorchSettingsPage _gpBoTorchPage;
        private NSGAIISettingsPage _nsgaiiPage;
        private NSGAIIISettingsPage _nsgaiiiPage;
        private CmaEsSettingsPage _cmaesPage;
        private RandomSettingsPage _randomPage;
        private QmcSettingsPage _qmcPage;
        private BruteForceSettingsPage _bruteForcePage;
        private AutoSettingsPage _autoPage;
        private MOEADSettingsPage _moeadPage;
        private MoCmaEsSettingsPage _moCmaEsPage;


        private string _trialNumberParam1Label;
        public string TrialNumberParam1Label { get => _trialNumberParam1Label; set => SetProperty(ref _trialNumberParam1Label, value); }
        private string _trialNumberParam1;
        public string TrialNumberParam1 { get => _trialNumberParam1; set => SetProperty(ref _trialNumberParam1, value); }
        private Visibility _trialNumberParam2Visibility;
        public Visibility TrialNumberParam2Visibility { get => _trialNumberParam2Visibility; set => SetProperty(ref _trialNumberParam2Visibility, value); }
        private string _trialNumberParam2Label;
        public string TrialNumberParam2Label { get => _trialNumberParam2Label; set => SetProperty(ref _trialNumberParam2Label, value); }
        private string _trialNumberParam2;
        public string TrialNumberParam2 { get => _trialNumberParam2; set => SetProperty(ref _trialNumberParam2, value); }
        private string _timeout;
        public string Timeout { get => _timeout; set => SetProperty(ref _timeout, value); }
        private string _studyName;
        public string StudyName { get => _studyName; set => SetProperty(ref _studyName, value); }
        private bool? _isInMemory;
        public bool? IsInMemory { get => _isInMemory; set => SetProperty(ref _isInMemory, value); }
        private bool? _isContinue;
        public bool? IsContinue { get => _isContinue; set => SetProperty(ref _isContinue, value); }
        private bool? _isCopy;
        public bool? IsCopy { get => _isCopy; set => SetProperty(ref _isCopy, value); }
        private bool? _enableIgnoreDuplicateSampling;
        public bool? EnableIgnoreDuplicateSampling { get => _enableIgnoreDuplicateSampling; set => SetProperty(ref _enableIgnoreDuplicateSampling, value); }
        private bool? _disableViewportUpdate;
        public bool? DisableViewportUpdate { get => _disableViewportUpdate; set => SetProperty(ref _disableViewportUpdate, value); }
        private bool? _minimizeRhinoWindow;
        public bool? MinimizeRhinoWindow { get => _minimizeRhinoWindow; set => SetProperty(ref _minimizeRhinoWindow, value); }
        private bool _enableRunOptimizeButton;
        public bool EnableRunOptimizeButton { get => _enableRunOptimizeButton; set => SetProperty(ref _enableRunOptimizeButton, value); }
        private bool _enableStopOptimizeButton;
        public bool EnableStopOptimizeButton { get => _enableStopOptimizeButton; set => SetProperty(ref _enableStopOptimizeButton, value); }
        private string _samplerTypeLabel;
        public string SamplerTypeLabel { get => _samplerTypeLabel; set => SetProperty(ref _samplerTypeLabel, value); }
        private Page _optimizeSettingsPage;
        public Page OptimizeSettingsPage { get => _optimizeSettingsPage; set => SetProperty(ref _optimizeSettingsPage, value); }
        private LiveChartPage _liveChart1;
        public LiveChartPage LiveChart1 { get => _liveChart1; set => SetProperty(ref _liveChart1, value); }
        private LiveChartPage _liveChart2;
        public LiveChartPage LiveChart2 { get => _liveChart2; set => SetProperty(ref _liveChart2, value); }
        private ObservableCollection<ObjectiveSettingItem> _objectiveSettingItems;
        public ObservableCollection<ObjectiveSettingItem> ObjectiveSettingItems { get => _objectiveSettingItems; set => SetProperty(ref _objectiveSettingItems, value); }
        private ObservableCollection<VariableSettingItem> _variableSettingItems;
        public ObservableCollection<VariableSettingItem> VariableSettingItems { get => _variableSettingItems; set => SetProperty(ref _variableSettingItems, value); }

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        public OptimizeViewModel()
        {
            _settings = OptimizeProcess.Settings;
            InitializeUIValues();
            InitializeChart();
            InitializeSamplerPage();
            ChangeTargetSampler(_settings.Optimize.SamplerType);
            InitializeObjectivesAndVariables();
        }

        private void InitializeUIValues()
        {
            Timeout = _settings.Optimize.Timeout.ToString(CultureInfo.InvariantCulture);

            IsInMemory = _settings.Storage.Type == StorageType.InMemory;
            IsContinue = _settings.Optimize.ContinueStudy;
            IsCopy = _settings.Optimize.CopyStudy;

            EnableIgnoreDuplicateSampling = _settings.Optimize.IgnoreDuplicateSampling;
            DisableViewportUpdate = _settings.Optimize.DisableViewportDrawing;
            MinimizeRhinoWindow = _settings.Optimize.MinimizeRhinoWindow;

            EnableRunOptimizeButton = true;
        }

        private void InitializeChart()
        {
            _chart1 = new LiveChartPage();
            LiveChart1 = _chart1;
            _chart2 = new LiveChartPage();
            LiveChart2 = _chart2;
        }

        private void InitializeSamplerPage()
        {
            _tpePage = TPESettingsPage.FromSettings(_settings);
            _gpOptunaPage = GPOptunaSettingsPage.FromSettings(_settings);
            _gpBoTorchPage = GPBoTorchSettingsPage.FromSettings(_settings);
            _nsgaiiPage = NSGAIISettingsPage.FromSettings(_settings);
            _nsgaiiiPage = NSGAIIISettingsPage.FromSettings(_settings);
            _cmaesPage = CmaEsSettingsPage.FromSettings(_settings);
            _randomPage = RandomSettingsPage.FromSettings(_settings);
            _qmcPage = QmcSettingsPage.FromSettings(_settings);
            _bruteForcePage = BruteForceSettingsPage.FromSettings(_settings);
            _autoPage = AutoSettingsPage.FromSettings(_settings);
            _moeadPage = MOEADSettingsPage.FromSettings(_settings);
            _moCmaEsPage = MoCmaEsSettingsPage.FromSettings(_settings);
        }

        public void ChangeTargetSampler(SamplerType samplerType)
        {
            ITrialNumberParam param;
            switch (samplerType)
            {
                case SamplerType.TPE:
                    param = _tpePage;
                    OptimizeSettingsPage = _tpePage;
                    break;
                case SamplerType.GP:
                    param = _gpOptunaPage;
                    OptimizeSettingsPage = _gpOptunaPage;
                    break;
                case SamplerType.BoTorch:
                    param = _gpBoTorchPage;
                    OptimizeSettingsPage = _gpBoTorchPage;
                    break;
                case SamplerType.NSGAII:
                    param = _nsgaiiPage;
                    OptimizeSettingsPage = _nsgaiiPage;
                    break;
                case SamplerType.NSGAIII:
                    param = _nsgaiiiPage;
                    OptimizeSettingsPage = _nsgaiiiPage;
                    break;
                case SamplerType.CmaEs:
                    param = _cmaesPage;
                    OptimizeSettingsPage = _cmaesPage;
                    break;
                case SamplerType.Random:
                    param = _randomPage;
                    OptimizeSettingsPage = _randomPage;
                    break;
                case SamplerType.QMC:
                    param = _qmcPage;
                    OptimizeSettingsPage = _qmcPage;
                    break;
                case SamplerType.BruteForce:
                    param = _bruteForcePage;
                    OptimizeSettingsPage = _bruteForcePage;
                    break;
                case SamplerType.AUTO:
                    param = _autoPage;
                    OptimizeSettingsPage = _autoPage;
                    break;
                case SamplerType.MOEAD:
                    param = _moeadPage;
                    OptimizeSettingsPage = _moeadPage;
                    break;
                case SamplerType.MoCmaEs:
                    param = _moCmaEsPage;
                    OptimizeSettingsPage = _moCmaEsPage;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(samplerType), samplerType, null);
            }
            SamplerTypeLabel = SamplerTypeLabelPrefix + samplerType;
            TrialNumberParam1Label = param.Param1Label;
            TrialNumberParam2Label = param.Param2Label;
            TrialNumberParam2Visibility = param.Param2Visibility;

            if (samplerType == SamplerType.NSGAII || samplerType == SamplerType.NSGAIII || samplerType == SamplerType.MOEAD)
            {
                int total = _settings.Optimize.NumberOfTrials;
                int populationSize = _settings.Optimize.Sampler.NsgaII.PopulationSize > 0
                    ? _settings.Optimize.Sampler.NsgaII.PopulationSize
                    : 1;
                int numGeneration = total / populationSize;
                TrialNumberParam1 = numGeneration.ToString(CultureInfo.InvariantCulture);
                TrialNumberParam2 = populationSize.ToString(CultureInfo.InvariantCulture);
            }
            else
            {
                TrialNumberParam1 = _settings.Optimize.NumberOfTrials.ToString(CultureInfo.InvariantCulture);
            }
        }

        private void InitializeObjectivesAndVariables()
        {
            Util.GrasshopperInOut ghIO = OptimizeProcess.Component.GhInOut;
            ObjectiveSettingItems = new ObservableCollection<ObjectiveSettingItem>();
            foreach (string item in ghIO.Objectives.GetNickNames())
            {
                ObjectiveSettingItems.Add(new ObjectiveSettingItem
                {
                    Name = item,
                    Minimize = true
                });
            }
            VariableSettingItems = new ObservableCollection<VariableSettingItem>();
            foreach (Core.Input.VariableBase item in ghIO.Variables)
            {
                if (item is Core.Input.NumberVariable numVariable)
                {
                    VariableSettingItems.Add(new VariableSettingItem
                    {
                        Name = numVariable.NickName,
                        Low = numVariable.LowerBond,
                        High = numVariable.UpperBond,
                        Step = numVariable.Epsilon,
                        IsLogScale = false
                    });
                }
            }
        }

        private DelegateCommand _runOptimize;
        public ICommand RunOptimize
        {
            get
            {
                if (_runOptimize == null)
                {
                    _runOptimize = new DelegateCommand(PerformRunOptimize);
                }
                return _runOptimize;
            }
        }
        private async void PerformRunOptimize()
        {
            TLog.MethodStart();
            EnableRunOptimizeButton = false;
            EnableStopOptimizeButton = true;
            _chart1.ClearPoints();
            _chart2.ClearPoints();

            _settings.Optimize = GetCurrentSettings(true);
            SetupWindow();

            _progressBar = OptimizeProcess.TunnyWindow.StatusBarProgressBar;
            _progressBar.Value = 0;

            InitializeOptimizeProcess();

            try
            {
                await OptimizeProcess.RunAsync(this);
            }
            finally
            {
                FinalizeWindow();
            }
        }

        private void InitializeOptimizeProcess()
        {
            Progress<ProgressState> progress = CreateProgressAction();
            OptimizeProcess.Settings = _settings;
            OptimizeProcess.AddProgress(progress);
        }

        private Progress<ProgressState> CreateProgressAction()
        {
            return new Progress<ProgressState>(value =>
            {
                TLog.MethodStart();
                OptimizeProcess.Component.UpdateGrasshopper(value);
                OptimizeProcess.TunnyWindow.StatusBarTrialNumberLabel.Content = $"{TrialNumberLabelPrefix}{value.TrialNumber + 1}";
                _progressBar.Value = value.PercentComplete;
                if (!value.IsReportOnly)
                {
                    Input.Objective objectives = OptimizeProcess.Component.GhInOut.Objectives;
                    _chart1.AddPoint(value.TrialNumber + 1, objectives.Numbers);
                    _chart2.AddPoint(value.TrialNumber + 1, objectives.Numbers);
                }
            });
        }

        private void SetupWindow()
        {
            OptimizeProcess.GH_DocumentEditor?.DisableUI();
            RhinoView.EnableDrawing = !_settings.Optimize.DisableViewportDrawing;

            if (_settings.Optimize.MinimizeRhinoWindow)
            {
                RhinoWindowHandle(6);
            }
        }
        private void FinalizeWindow()
        {
            EnableRunOptimizeButton = true;
            EnableStopOptimizeButton = false;
            OptimizeProcess.GH_DocumentEditor?.EnableUI();
            RhinoView.EnableDrawing = true;

            if (_settings.Optimize.MinimizeRhinoWindow)
            {
                RhinoWindowHandle(9);

            }
        }
        private static void RhinoWindowHandle(int status)
        {
            IntPtr rhinoWindow = Rhino.RhinoApp.MainWindowHandle();
            ShowWindow(rhinoWindow, status);
        }

        private DelegateCommand _stopOptimize;
        public ICommand StopOptimize
        {
            get
            {
                if (_stopOptimize == null)
                {
                    _stopOptimize = new DelegateCommand(PerformStopOptimize);
                }

                return _stopOptimize;
            }
        }

        public LiveChartPage Chart2 { get => _chart2; set => _chart2 = value; }
        public GPOptunaSettingsPage GpOptunaPage { get => _gpOptunaPage; set => _gpOptunaPage = value; }

        private void PerformStopOptimize()
        {
            TLog.MethodStart();
            OptimizeProcess.IsForcedStopOptimize = true;
            using (FileStream fs = File.Create(TEnvVariables.QuitFishingPath))
            {
            }
        }

        internal Core.Settings.Optimize GetCurrentSettings(bool computeAutoValue = false)
        {
            TLog.MethodStart();
            var sampler = new Sampler
            {
                Tpe = _tpePage.ToSettings(),
                GP = _gpOptunaPage.ToSettings(),
                BoTorch = _gpBoTorchPage.ToSettings(),
                NsgaII = _nsgaiiPage.ToSettings(),
                NsgaIII = _nsgaiiiPage.ToSettings(),
                CmaEs = _cmaesPage.ToSettings(),
                Random = _randomPage.ToSettings(),
                QMC = _qmcPage.ToSettings(),
                BruteForce = _bruteForcePage.ToSettings(),
                Auto = _autoPage.ToSettings(),
                MOEAD = _moeadPage.ToSettings(),
                MoCmaEs = _moCmaEsPage.ToSettings()
            };

            int param1 = int.Parse(TrialNumberParam1, CultureInfo.InvariantCulture);
            bool result = int.TryParse(TrialNumberParam2, NumberStyles.Integer, CultureInfo.InvariantCulture, out int param2);
            if (!result)
            {
                param2 = 0;
            }
            SamplerType type = _settings.Optimize.SamplerType;
            int numOfTrials = type == SamplerType.NSGAII || type == SamplerType.NSGAIII || type == SamplerType.MOEAD
                ? param1 * param2
                : param1;
            sampler.NsgaII.PopulationSize = param2;
            sampler.NsgaIII.PopulationSize = param2;
            sampler.MOEAD.PopulationSize = param2;

            var settings = new Core.Settings.Optimize
            {
                StudyName = StudyName,
                Sampler = sampler,
                NumberOfTrials = numOfTrials,
                ContinueStudy = IsContinue == true,
                CopyStudy = IsCopy == true,
                SamplerType = _settings.Optimize.SamplerType,
                Timeout = double.Parse(Timeout, CultureInfo.InvariantCulture),
                GcAfterTrial = GcAfterTrial.HasGeometry,
                ShowRealtimeResult = false,
                IgnoreDuplicateSampling = EnableIgnoreDuplicateSampling == true,
                DisableViewportDrawing = DisableViewportUpdate == true,
                MinimizeRhinoWindow = MinimizeRhinoWindow == true
            };

            if (computeAutoValue)
            {
                settings.ComputeAutoValue();
            }

            return settings;
        }
    }
}
