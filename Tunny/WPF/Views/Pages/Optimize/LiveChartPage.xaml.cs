using System.Collections.ObjectModel;
using System.Windows.Controls;

using LiveChartsCore.Defaults;

using Tunny.WPF.Common;
using Tunny.WPF.ViewModels;

namespace Tunny.WPF.Views.Pages.Optimize
{
    public partial class LiveChartPage : Page
    {
        public ObservableCollection<ObservablePoint> Points { get; set; }
        private readonly LiveChartViewModel _viewModel;

        public LiveChartPage(string xAxisName, string yAxisName, ChartType chartType)
        {
            InitializeComponent();
            _viewModel = new LiveChartViewModel(xAxisName, yAxisName, chartType);
            DataContext = _viewModel;
        }

        public void AddPoint(ObservablePoint point)
        {
            _viewModel.ChartPoints.Add(point);
        }
    }
}
