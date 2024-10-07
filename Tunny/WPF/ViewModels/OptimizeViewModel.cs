using System.Collections.ObjectModel;
using System.ComponentModel;

using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;

using SkiaSharp;

namespace Tunny.WPF.ViewModels
{
    public class OptimizeViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<string> _samplers;
        public ObservableCollection<string> Samplers
        {
            get { return _samplers; }
            set
            {
                _samplers = value;
                OnPropertyChanged(nameof(Samplers));
            }
        }

        private string _selectedSampler;
        public string SelectedSampler
        {
            get { return _selectedSampler; }
            set
            {
                _selectedSampler = value;
                OnPropertyChanged(nameof(SelectedSampler));
            }
        }

        public ObservableCollection<ISeries> ChartSeries { get; set; }
        public Axis[] ChartXAxes { get; set; }
        public Axis[] ChartYAxes { get; set; }
        public static DrawMarginFrame ChartDrawMarginFrame => new DrawMarginFrame()
        {
            Fill = new SolidColorPaint(new SKColor(220, 220, 220)),
            Stroke = new SolidColorPaint(new SKColor(180, 180, 180), 1)
        };


        public OptimizeViewModel()
        {
            Samplers = new ObservableCollection<string>
            {
                "BayesianOptimization(TPE)",
                "BayesianOptimization(GP:Optuna)",
                "BayesianOptimization(GP:Botorch)",
                "GeneticAlgorithm(NSGA-II)",
                "GeneticAlgorithm(NSGA-III)",
                "EvolutionStrategy(CMA-ES)",
                "Quasi-MonteCarlo",
                "Random",
                "BruteForce"
            };

            SelectedSampler = Samplers[0];

            ChartSeries = new ObservableCollection<ISeries>
            {
                new LineSeries<double>
                {
                    Values = new double[] { 6, 7, 5, 4 ,4, 6, 3, 2, 1, 1, 1, 1 },
                    Fill = null,
                    LineSmoothness = 0,
                    GeometrySize = 10,
                    Stroke = new SolidColorPaint(SKColors.LightBlue) { StrokeThickness = 3 }
                }
            };

            ChartXAxes = new Axis[]
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
                }
            };

            ChartYAxes = new Axis[]
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
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
