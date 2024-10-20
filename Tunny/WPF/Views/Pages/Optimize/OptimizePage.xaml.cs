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
        private ProgressBar _progressBar;
        private LiveChartPage _chart1;
        private LiveChartPage _chart2;

        public OptimizePage(SamplerType samplerType)
        {
            InitializeComponent();
            InitializeChart();
            ChangeFrameContent(samplerType);
        }

        private void InitializeChart()
        {
            _chart1 = new LiveChartPage("Trial Number", "Objective 1", ChartType.Line);
            OptimizeLiveChart1.Content = _chart1;
            _chart2 = new LiveChartPage("Objective 1", "Objective 2", ChartType.Scatter);
            OptimizeLiveChart2.Content = _chart2;
        }

        private void ChangeFrameContent(SamplerType samplerType)
        {
            ITrialNumberParam param;
            switch (samplerType)
            {
                case SamplerType.TPE:
                    var tpePage = new TPESettingsPage();
                    param = tpePage;
                    OptimizeSettingsPage.Content = tpePage;
                    break;
                case SamplerType.GP:
                    var gpOptunaPage = new GPOptunaSettingsPage();
                    param = gpOptunaPage;
                    OptimizeSettingsPage.Content = gpOptunaPage;
                    break;
                case SamplerType.BoTorch:
                    var gpBoTorchPage = new GPBoTorchSettingsPage();
                    param = gpBoTorchPage;
                    OptimizeSettingsPage.Content = gpBoTorchPage;
                    break;
                case SamplerType.NSGAII:
                    var nsgaiiPage = new NSGAIISettingsPage();
                    param = nsgaiiPage;
                    OptimizeSettingsPage.Content = nsgaiiPage;
                    break;
                case SamplerType.NSGAIII:
                    OptimizeSettingsPage.Content = new NSGAIIISettingsPage();
                    var nsgaiiiPage = new NSGAIIISettingsPage();
                    param = nsgaiiiPage;
                    OptimizeSettingsPage.Content = nsgaiiiPage;
                    break;
                case SamplerType.CmaEs:
                    var cmaesPage = new CmaEsSettingsPage();
                    param = cmaesPage;
                    OptimizeSettingsPage.Content = cmaesPage;
                    break;
                case SamplerType.Random:
                    var randomPage = new RandomSettingsPage();
                    param = randomPage;
                    OptimizeSettingsPage.Content = randomPage;
                    break;
                case SamplerType.QMC:
                    var qmcPage = new QmcSettingsPage();
                    param = qmcPage;
                    OptimizeSettingsPage.Content = qmcPage;
                    break;
                case SamplerType.BruteForce:
                    var bruteForcePage = new BruteForceSettingsPage();
                    param = bruteForcePage;
                    OptimizeSettingsPage.Content = bruteForcePage;
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
            TSettings settings = parentWindow.Settings;
            ghCanvas?.DisableUI();

            OptimizeRunButton.IsEnabled = false;
            OptimizeStopButton.IsEnabled = true;
            RhinoView.EnableDrawing = !settings.Optimize.DisableViewportDrawing;

            _progressBar = parentWindow.StatusBarProgressBar;
            _progressBar.Value = 0;

            InitializeOptimizeProcess(parentWindow, settings);

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
    }
}
