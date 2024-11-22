using System;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;

using Grasshopper.GUI;

using Rhino.Display;

using Tunny.Core.Handler;
using Tunny.Core.Settings;
using Tunny.Core.TEnum;
using Tunny.Core.Util;
using Tunny.Process;
using Tunny.WPF.Common;
using Tunny.WPF.ViewModels.Optimize;
using Tunny.WPF.Views.Pages.Settings.Sampler;

namespace Tunny.WPF.Views.Pages.Optimize
{
    public partial class OptimizePage : Page
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

        public OptimizePage()
        {
            _settings = OptimizeProcess.Settings;
            InitializeComponent();
            InitializeUIValues();
            InitializeChart();
            InitializeSamplerPage();
            ChangeTargetSampler(_settings.Optimize.SamplerType);
        }

        private void InitializeUIValues()
        {
            OptimizeTimeoutTextBox.Text = _settings.Optimize.Timeout.ToString(CultureInfo.InvariantCulture);

            OptimizeInMemoryCheckBox.IsChecked = _settings.Storage.Type == StorageType.InMemory;
            OptimizeContinueCheckBox.IsChecked = _settings.Optimize.ContinueStudy;
            OptimizeCopyCheckBox.IsChecked = _settings.Optimize.CopyStudy;

            OptimizeIgnoreDuplicateSamplingCheckBox.IsChecked = _settings.Optimize.IgnoreDuplicateSampling;
            OptimizeDisableViewportUpdateCheckBox.IsChecked = _settings.Optimize.DisableViewportDrawing;
            OptimizeMinimizeRhinoWindowCheckBox.IsChecked = _settings.Optimize.MinimizeRhinoWindow;
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

        private void InitializeChart()
        {
            _chart1 = new LiveChartPage();
            OptimizeLiveChart1.Content = _chart1;
            _chart2 = new LiveChartPage();
            OptimizeLiveChart2.Content = _chart2;
        }

        public void ChangeTargetSampler(SamplerType samplerType)
        {
            ITrialNumberParam param;
            switch (samplerType)
            {
                case SamplerType.TPE:
                    param = _tpePage;
                    OptimizeSettingsPage.Content = _tpePage;
                    break;
                case SamplerType.GP:
                    param = _gpOptunaPage;
                    OptimizeSettingsPage.Content = _gpOptunaPage;
                    break;
                case SamplerType.BoTorch:
                    param = _gpBoTorchPage;
                    OptimizeSettingsPage.Content = _gpBoTorchPage;
                    break;
                case SamplerType.NSGAII:
                    param = _nsgaiiPage;
                    OptimizeSettingsPage.Content = _nsgaiiPage;
                    break;
                case SamplerType.NSGAIII:
                    param = _nsgaiiiPage;
                    OptimizeSettingsPage.Content = _nsgaiiiPage;
                    break;
                case SamplerType.CmaEs:
                    param = _cmaesPage;
                    OptimizeSettingsPage.Content = _cmaesPage;
                    break;
                case SamplerType.Random:
                    param = _randomPage;
                    OptimizeSettingsPage.Content = _randomPage;
                    break;
                case SamplerType.QMC:
                    param = _qmcPage;
                    OptimizeSettingsPage.Content = _qmcPage;
                    break;
                case SamplerType.BruteForce:
                    param = _bruteForcePage;
                    OptimizeSettingsPage.Content = _bruteForcePage;
                    break;
                case SamplerType.AUTO:
                    param = _autoPage;
                    OptimizeSettingsPage.Content = _autoPage;
                    break;
                case SamplerType.MOEAD:
                    param = _moeadPage;
                    OptimizeSettingsPage.Content = _moeadPage;
                    break;
                case SamplerType.MoCmaEs:
                    param = _moCmaEsPage;
                    OptimizeSettingsPage.Content = _moCmaEsPage;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(samplerType), samplerType, null);
            }
            OptimizeSamplerTypeLabel.Content = SamplerTypeLabelPrefix + samplerType;
            OptimizeTrialNumberParam1Label.Content = param.Param1Label;
            OptimizeTrialNumberParam2Label.Content = param.Param2Label;
            OptimizeTrialNumberParam2Label.Visibility = param.Param2Visibility;
            OptimizeTrialNumberParam2TextBox.Visibility = param.Param2Visibility;

            if (samplerType == SamplerType.NSGAII || samplerType == SamplerType.NSGAIII || samplerType == SamplerType.MOEAD)
            {
                int total = _settings.Optimize.NumberOfTrials;
                int populationSize = _settings.Optimize.Sampler.NsgaII.PopulationSize;
                int numGeneration = total / _settings.Optimize.Sampler.NsgaII.PopulationSize;
                OptimizeTrialNumberParam1TextBox.Text = numGeneration.ToString(CultureInfo.InvariantCulture);
                OptimizeTrialNumberParam2TextBox.Text = populationSize.ToString(CultureInfo.InvariantCulture);
            }
            else
            {
                OptimizeTrialNumberParam1TextBox.Text = _settings.Optimize.NumberOfTrials.ToString(CultureInfo.InvariantCulture);
            }
        }

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        private async void RunOptimizeButton_Click(object sender, RoutedEventArgs e)
        {
            TLog.MethodStart();
            OptimizeRunButton.IsEnabled = false;
            OptimizeStopButton.IsEnabled = true;
            _chart1.ClearPoints();
            _chart2.ClearPoints();

            _settings.Optimize = GetCurrentSettings(true);

            SetupWindow(out MainWindow parentWindow, out GH_DocumentEditor ghCanvas);

            _progressBar = parentWindow.StatusBarProgressBar;
            _progressBar.Value = 0;

            InitializeOptimizeProcess(parentWindow, _settings);

            try
            {
                await OptimizeProcess.RunAsync(DataContext as OptimizeViewModel);
            }
            finally
            {
                FinalizeWindow(parentWindow, ghCanvas);
            }
        }

        private void SetupWindow(out MainWindow parentWindow, out GH_DocumentEditor ghCanvas)
        {
            parentWindow = Window.GetWindow(this) as MainWindow;
            ghCanvas = parentWindow.DocumentEditor;
            ghCanvas?.DisableUI();
            RhinoView.EnableDrawing = !_settings.Optimize.DisableViewportDrawing;

            if (_settings.Optimize.MinimizeRhinoWindow)
            {
                RhinoWindowHandle(6);
            }
        }

        private void FinalizeWindow(MainWindow parentWindow, GH_DocumentEditor ghCanvas)
        {
            OptimizeRunButton.IsEnabled = true;
            OptimizeStopButton.IsEnabled = false;
            ghCanvas?.EnableUI();
            parentWindow.StatusBarTrialNumberLabel.Content = $"{TrialNumberLabelPrefix}#";
            RhinoView.EnableDrawing = true;

            if (_settings.Optimize.MinimizeRhinoWindow)
            {
                RhinoWindowHandle(9);
                parentWindow.Topmost = true;
                parentWindow.Topmost = false;
            }
        }

        private static void RhinoWindowHandle(int status)
        {
            IntPtr rhinoWindow = Rhino.RhinoApp.MainWindowHandle();
            ShowWindow(rhinoWindow, status);
        }

        private void InitializeOptimizeProcess(MainWindow parentWindow, TSettings settings)
        {
            Progress<ProgressState> progress = CreateProgressAction(parentWindow);
            OptimizeProcess.Settings = settings;
            OptimizeProcess.AddProgress(progress);
        }

        private Progress<ProgressState> CreateProgressAction(MainWindow parentWindow)
        {
            return new Progress<ProgressState>(value =>
            {
                TLog.MethodStart();
                OptimizeProcess.Component.UpdateGrasshopper(value);
                parentWindow.StatusBarTrialNumberLabel.Content = $"{TrialNumberLabelPrefix}{value.TrialNumber + 1}";
                _progressBar.Value = value.PercentComplete;
                if (!value.IsReportOnly)
                {
                    Input.Objective objectives = OptimizeProcess.Component.GhInOut.Objectives;
                    _chart1.AddPoint(value.TrialNumber + 1, objectives.Numbers);
                    _chart2.AddPoint(value.TrialNumber + 1, objectives.Numbers);
                }
            });
        }

        private void OptimizeStopButton_Click(object sender, RoutedEventArgs e)
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
                MoCmaEs = new MoCmaEsSettingsPage().ToSettings()
            };

            int param1 = int.Parse(OptimizeTrialNumberParam1TextBox.Text, CultureInfo.InvariantCulture);
            int param2 = int.Parse(OptimizeTrialNumberParam2TextBox.Text, CultureInfo.InvariantCulture);
            SamplerType type = _settings.Optimize.SamplerType;
            int numOfTrials = type == SamplerType.NSGAII || type == SamplerType.NSGAIII || type == SamplerType.MOEAD
                ? param1 * param2
                : param1;
            sampler.NsgaII.PopulationSize = param2;
            sampler.NsgaIII.PopulationSize = param2;
            sampler.MOEAD.PopulationSize = param2;

            var settings = new Core.Settings.Optimize
            {
                StudyName = OptimizeStudyNameTextBox.Text,
                Sampler = sampler,
                NumberOfTrials = numOfTrials,
                ContinueStudy = OptimizeContinueCheckBox.IsChecked == true,
                CopyStudy = OptimizeCopyCheckBox.IsChecked == true,
                SamplerType = _settings.Optimize.SamplerType,
                Timeout = double.Parse(OptimizeTimeoutTextBox.Text, CultureInfo.InvariantCulture),
                GcAfterTrial = GcAfterTrial.HasGeometry,
                ShowRealtimeResult = false,
                IgnoreDuplicateSampling = OptimizeIgnoreDuplicateSamplingCheckBox.IsChecked == true,
                DisableViewportDrawing = OptimizeDisableViewportUpdateCheckBox.IsChecked == true,
                MinimizeRhinoWindow = OptimizeMinimizeRhinoWindowCheckBox.IsChecked == true
            };

            if (computeAutoValue)
            {
                settings.ComputeAutoValue();
            }

            return settings;
        }
    }
}
