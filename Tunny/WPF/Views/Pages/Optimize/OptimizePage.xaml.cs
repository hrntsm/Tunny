using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

using Grasshopper.GUI;

using LiveChartsCore.Defaults;

using Rhino.Display;

using Tunny.Core.Handler;
using Tunny.Core.Settings;
using Tunny.Core.TEnum;
using Tunny.Core.Util;
using Tunny.Process;
using Tunny.WPF.Common;
using Tunny.WPF.Views.Pages.Settings.Sampler;

namespace Tunny.WPF.Views.Pages.Optimize
{
    public partial class OptimizePage : Page
    {
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

        public OptimizePage(TSettings settings)
        {
            _settings = settings;
            InitializeComponent();
            InitializeChart();
            InitializeSamplerPage();
            ChangeTargetSampler(settings.Optimize.SelectSampler);
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
        }

        private void InitializeChart()
        {
            _chart1 = new LiveChartPage("Trial Number", "Objective 1", ChartType.Line);
            OptimizeLiveChart1.Content = _chart1;
            _chart2 = new LiveChartPage("Objective 1", "Objective 2", ChartType.Scatter);
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
                default:
                    throw new ArgumentOutOfRangeException(nameof(samplerType), samplerType, null);
            }
            OptimizeTrialNumberParam1Label.Content = param.Param1Label;
            OptimizeTrialNumberParam2Label.Content = param.Param2Label;
            OptimizeTrialNumberParam2Label.Visibility = param.Param2Visibility;
            OptimizeTrialNumberParam2TextBox.Visibility = param.Param2Visibility;
        }

        private async void RunOptimizeButton_Click(object sender, RoutedEventArgs e)
        {
            TLog.MethodStart();
            var parentWindow = Window.GetWindow(this) as MainWindow;
            GH_DocumentEditor ghCanvas = parentWindow.DocumentEditor;
            ghCanvas?.DisableUI();

            OptimizeRunButton.IsEnabled = false;
            OptimizeStopButton.IsEnabled = true;
            RhinoView.EnableDrawing = !_settings.Optimize.DisableViewportDrawing;

            _progressBar = parentWindow.StatusBarProgressBar;
            _progressBar.Value = 0;

            InitializeOptimizeProcess(parentWindow, _settings);

            try
            {
                await OptimizeProcess.RunAsync();
            }
            finally
            {
                OptimizeRunButton.IsEnabled = true;
                OptimizeStopButton.IsEnabled = false;
                ghCanvas?.EnableUI();
                parentWindow.StatusBarTrialNumberLabel.Content = $"{TrialNumberLabelPrefix}#";
            }
        }

        private void InitializeOptimizeProcess(MainWindow parentWindow, TSettings settings)
        {
            Progress<ProgressState> progress = CreateProgressAction(parentWindow);
            OptimizeProcess.Component = parentWindow.Component;
            OptimizeProcess.Settings = settings;
            OptimizeProcess.AddProgress(progress);
        }

        private Progress<ProgressState> CreateProgressAction(MainWindow parentWindow)
        {
            return new Progress<ProgressState>(value =>
            {
                TLog.MethodStart();
                parentWindow.Component.UpdateGrasshopper(value);
                parentWindow.StatusBarTrialNumberLabel.Content = $"{TrialNumberLabelPrefix}{value.TrialNumber + 1}";
                _progressBar.Value = value.PercentComplete;
                if (!value.IsReportOnly)
                {
                    Input.Objective objectives = parentWindow.Component.GhInOut.Objectives;
                    _chart1.AddPoint(new ObservablePoint(value.TrialNumber + 1, objectives.Numbers[0]));
                    _chart2.AddPoint(new ObservablePoint(objectives.Numbers[0], objectives.Numbers[1]));
                }
            });
        }

        private void OptimizeStopButton_Click(object sender, RoutedEventArgs e)
        {
            TLog.MethodStart();
            using (FileStream fs = File.Create(TEnvVariables.QuitFishingPath))
            {
            }
        }

        internal Sampler GetCurrentSettings()
        {
            return new Sampler
            {
                Tpe = _tpePage.ToSettings(),
                GP = _gpOptunaPage.ToSettings(),
                BoTorch = _gpBoTorchPage.ToSettings(),
                NsgaII = _nsgaiiPage.ToSettings(),
                NsgaIII = _nsgaiiiPage.ToSettings(),
                CmaEs = _cmaesPage.ToSettings(),
                Random = _randomPage.ToSettings(),
                QMC = _qmcPage.ToSettings(),
                BruteForce = _bruteForcePage.ToSettings()
            };
        }
    }
}
