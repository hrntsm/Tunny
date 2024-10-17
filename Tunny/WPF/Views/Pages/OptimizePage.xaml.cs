using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;

using Grasshopper.GUI;

using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;

using Rhino.Display;

using SkiaSharp;

using Tunny.Core.Handler;
using Tunny.Core.Settings;
using Tunny.Core.TEnum;
using Tunny.Core.Util;
using Tunny.Process;
using Tunny.WPF.Common;
using Tunny.WPF.Views.Pages.Settings.Sampler;

namespace Tunny.WPF.Views.Pages
{
    public partial class OptimizePage : Page
    {
        public ObservableCollection<ISeries> Chart1Series { get; set; }
        public ObservableCollection<ObservablePoint> Chart1Points { get; set; }
        public Axis[] Chart1XAxes { get; set; }
        public Axis[] Chart1YAxes { get; set; }

        public ObservableCollection<ISeries> Chart2Series { get; set; }
        public ObservableCollection<ObservablePoint> Chart2Points { get; set; }
        public Axis[] Chart2XAxes { get; set; }
        public Axis[] Chart2YAxes { get; set; }

        public static DrawMarginFrame ChartDrawMarginFrame => new DrawMarginFrame()
        {
            Fill = new SolidColorPaint(new SKColor(220, 220, 220)),
            Stroke = new SolidColorPaint(new SKColor(180, 180, 180), 1)
        };

        private const string TrialNumberLabelPrefix = "Trial: ";
        private ProgressBar _progressBar;

        public OptimizePage()
        {
            InitializeComponent();
            ChangeFrameContent(SamplerType.TPE);
            InitializeChart1();
        }

        public OptimizePage(SamplerType samplerType)
        {
            InitializeComponent();
            ChangeFrameContent(samplerType);
            InitializeChart1();
        }

        private void InitializeChart1()
        {
            Chart1Points = new ObservableCollection<ObservablePoint>();
            Chart1Series = new ObservableCollection<ISeries>
            {
                new LineSeries<ObservablePoint>
                {
                    Values = Chart1Points,
                    Fill = null,
                    LineSmoothness = 0,
                    GeometrySize = 2.5,
                    Stroke = new SolidColorPaint(SKColors.LightBlue) { StrokeThickness = 1 }
                }
            };

            Chart1XAxes = new Axis[]
            {
                new Axis
                {
                    Name = "Trials",
                    NameTextSize = 15,
                    NamePaint = new SolidColorPaint(SKColors.Black),
                    LabelsPaint = new SolidColorPaint(SKColors.Black),
                    TextSize = 12,
                    SeparatorsPaint = new SolidColorPaint
                    {
                        Color = SKColors.Black.WithAlpha(100),
                        StrokeThickness = 1,
                    },
                    SubseparatorsPaint = new SolidColorPaint
                    {
                        Color = SKColors.Black.WithAlpha(50),
                        StrokeThickness = 0.5f
                    },
                }
            };

            Chart1YAxes = new Axis[]
            {
                new Axis
                {
                    Name = "Objective",
                    NameTextSize = 15,
                    NamePaint = new SolidColorPaint(SKColors.Black),
                    LabelsPaint = new SolidColorPaint(SKColors.Black),
                    TextSize = 12,
                    SeparatorsPaint = new SolidColorPaint(SKColors.LightGray)
                    {
                        Color = SKColors.Black.WithAlpha(100),
                        StrokeThickness = 1,
                    },
                    SubseparatorsPaint = new SolidColorPaint
                    {
                        Color = SKColors.Black.WithAlpha(50),
                        StrokeThickness = 0.5f
                    },
                }
            };

            Chart2Points = new ObservableCollection<ObservablePoint>();
            Chart2Series = new ObservableCollection<ISeries>
            {
                new ScatterSeries<ObservablePoint>
                {
                    Values = Chart2Points,
                    GeometrySize = 5,
                }
            };

            Chart2XAxes = new Axis[]
            {
                new Axis
                {
                    Name = "Objective 1",
                    NameTextSize = 15,
                    NamePaint = new SolidColorPaint(SKColors.Black),
                    LabelsPaint = new SolidColorPaint(SKColors.Black),
                    TextSize = 12,
                    SeparatorsPaint = new SolidColorPaint
                    {
                        Color = SKColors.Black.WithAlpha(100),
                        StrokeThickness = 1,
                    },
                    SubseparatorsPaint = new SolidColorPaint
                    {
                        Color = SKColors.Black.WithAlpha(50),
                        StrokeThickness = 0.5f
                    },
                }
            };

            Chart2YAxes = new Axis[]
            {
                new Axis
                {
                    Name = "Objective 2",
                    NameTextSize = 15,
                    NamePaint = new SolidColorPaint(SKColors.Black),
                    LabelsPaint = new SolidColorPaint(SKColors.Black),
                    TextSize = 12,
                    SeparatorsPaint = new SolidColorPaint(SKColors.LightGray)
                    {
                        Color = SKColors.Black.WithAlpha(100),
                        StrokeThickness = 1,
                    },
                    SubseparatorsPaint = new SolidColorPaint
                    {
                        Color = SKColors.Black.WithAlpha(50),
                        StrokeThickness = 0.5f
                    },
                }
            };

            DataContext = this;
        }

        private void ChangeFrameContent(SamplerType samplerType)
        {
            ITrialNumberParam param;
            switch (samplerType)
            {
                case SamplerType.TPE:
                    var tpePage = new TPESettingsPage();
                    param = tpePage;
                    OptimizeDynamicFrame.Content = tpePage;
                    break;
                case SamplerType.GP:
                    var gpOptunaPage = new GPOptunaSettingsPage();
                    param = gpOptunaPage;
                    OptimizeDynamicFrame.Content = gpOptunaPage;
                    break;
                case SamplerType.BoTorch:
                    var gpBoTorchPage = new GPBoTorchSettingsPage();
                    param = gpBoTorchPage;
                    OptimizeDynamicFrame.Content = gpBoTorchPage;
                    break;
                case SamplerType.NSGAII:
                    var nsgaiiPage = new NSGAIISettingsPage();
                    param = nsgaiiPage;
                    OptimizeDynamicFrame.Content = nsgaiiPage;
                    break;
                case SamplerType.NSGAIII:
                    OptimizeDynamicFrame.Content = new NSGAIIISettingsPage();
                    var nsgaiiiPage = new NSGAIIISettingsPage();
                    param = nsgaiiiPage;
                    OptimizeDynamicFrame.Content = nsgaiiiPage;
                    break;
                case SamplerType.CmaEs:
                    var cmaesPage = new CmaEsSettingsPage();
                    param = cmaesPage;
                    OptimizeDynamicFrame.Content = cmaesPage;
                    break;
                case SamplerType.Random:
                    var randomPage = new RandomSettingsPage();
                    param = randomPage;
                    OptimizeDynamicFrame.Content = randomPage;
                    break;
                case SamplerType.QMC:
                    var qmcPage = new QmcSettingsPage();
                    param = qmcPage;
                    OptimizeDynamicFrame.Content = qmcPage;
                    break;
                case SamplerType.BruteForce:
                    var bruteForcePage = new BruteForceSettingsPage();
                    param = bruteForcePage;
                    OptimizeDynamicFrame.Content = bruteForcePage;
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
                    Chart1Points.Add(new ObservablePoint(value.TrialNumber + 1, objectives.Numbers[0]));
                    Chart2Points.Add(new ObservablePoint(objectives.Numbers[0], objectives.Numbers[1]));
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
