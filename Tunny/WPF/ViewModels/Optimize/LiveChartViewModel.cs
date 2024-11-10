using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;

using SkiaSharp;

using Tunny.WPF.Common;

namespace Tunny.WPF.ViewModels.Optimize
{
    public class LiveChartViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<string> _xTarget;
        public ObservableCollection<string> XTarget { get => _xTarget; set => SetProperty(ref _xTarget, value); }

        private ObservableCollection<string> _yTarget;
        public ObservableCollection<string> YTarget { get => _yTarget; set => SetProperty(ref _yTarget, value); }

        private ObservableCollection<ObservablePoint> _chartPoints;
        public ObservableCollection<ObservablePoint> ChartPoints { get => _chartPoints; set => SetProperty(ref _chartPoints, value); }

        private ObservableCollection<ISeries> _chartSeries;
        public ObservableCollection<ISeries> ChartSeries { get => _chartSeries; set => SetProperty(ref _chartSeries, value); }

        private IEnumerable<ICartesianAxis> _chartXAxes;
        public IEnumerable<ICartesianAxis> ChartXAxes { get => _chartXAxes; set => SetProperty(ref _chartXAxes, value); }

        private IEnumerable<ICartesianAxis> _chartYAxes;
        public IEnumerable<ICartesianAxis> ChartYAxes { get => _chartYAxes; set => SetProperty(ref _chartYAxes, value); }

        public LiveChartViewModel()
        {
        }

        public LiveChartViewModel(string xAxisName, string yAxisName, ChartType chartType)
        {
            _chartPoints = new ObservableCollection<ObservablePoint>();
            if (chartType == ChartType.Line)
            {
                ChartSeries = new ObservableCollection<ISeries>
                {
                    new LineSeries<ObservablePolarPoint>
                    {
                        Values = _chartPoints,
                        Fill = null,
                        LineSmoothness = 0,
                        GeometrySize = 2.5,
                        Stroke = new SolidColorPaint
                        {
                            Color = SKColors.LightBlue,
                            StrokeThickness = 1
                        }
                    }
                };
            }
            else if (chartType == ChartType.Scatter)
            {
                ChartSeries = new ObservableCollection<ISeries>
                {
                    new ScatterSeries<ObservablePolarPoint>
                    {
                        Values = _chartPoints,
                        GeometrySize =5
                    }
                };
            }

            ChartXAxes = new Axis[]
            {
                new Axis
                {
                    Name = xAxisName,
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

            ChartYAxes = new Axis[]
            {
                new Axis
                {
                    Name = yAxisName,
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

        public void AddPoint(double x, double y)
        {
            _chartPoints.Add(new ObservablePoint(x, y));
            OnPropertyChanged(nameof(ChartPoints));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T field, T newValue, [CallerMemberName] string propertyName = null)
        {
            if (!Equals(field, newValue))
            {
                field = newValue;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
                return true;
            }

            return false;
        }
    }
}
