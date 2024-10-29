using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

using LiveChartsCore.Defaults;

using Tunny.Component.Optimizer;
using Tunny.Process;
using Tunny.WPF.Common;
using Tunny.WPF.ViewModels;

namespace Tunny.WPF.Views.Pages.Optimize
{
    public partial class LiveChartPage : Page
    {
        private readonly LiveChartViewModel _viewModel;

        public LiveChartPage(string xAxisName, string yAxisName, ChartType chartType)
        {
            InitializeComponent();
            _viewModel = new LiveChartViewModel(xAxisName, yAxisName, chartType);
            DataContext = _viewModel;

            Loaded += SetTargetComboBoxItems;
        }

        private void SetTargetComboBoxItems(object sender, RoutedEventArgs e)
        {
            OptimizeComponentBase component = OptimizeProcess.Component;
            string[] metricNames = component.GhInOut.Objectives.GetNickNames();
            _viewModel.XTarget = new ObservableCollection<string> { "Trial Number" };
            _viewModel.YTarget = new ObservableCollection<string> { "Trial Number" };
            foreach (string s in metricNames)
            {
                _viewModel.XTarget.Add(s);
                _viewModel.YTarget.Add(s);
            }
            ChartXTargetComboBox.SelectedIndex = 0;
            ChartYTargetComboBox.SelectedIndex = 1;
        }

        public void AddPoint(ObservablePoint point)
        {
            _viewModel.ChartPoints.Add(point);
        }

        internal void ClearPoints()
        {
            _viewModel.ChartPoints.Clear();
        }
    }
}
