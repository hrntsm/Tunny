using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

using LiveChartsCore.Defaults;

using Tunny.Component.Optimizer;
using Tunny.Process;
using Tunny.WPF.Common;
using Tunny.WPF.ViewModels.Optimize;

namespace Tunny.WPF.Views.Pages.Optimize
{
    public partial class LiveChartPage : Page
    {
        private readonly LiveChartViewModel _viewModel;

        public LiveChartPage()
        {
            InitializeComponent();
            _viewModel = new LiveChartViewModel();
            DataContext = _viewModel;

            Loaded += SetTargetComboBoxItems;
        }

        private void SetTargetComboBoxItems(object sender, RoutedEventArgs e)
        {
            OptimizeComponentBase component = SharedItems.Instance.Component;
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

        public void AddPoint(int trialNumber, double[] objectives)
        {
            if (_viewModel.ChartEnable)
            {
                double x = ChartXTargetComboBox.SelectedIndex == 0
                    ? trialNumber
                    : objectives[ChartXTargetComboBox.SelectedIndex - 1];
                double y = ChartYTargetComboBox.SelectedIndex == 0
                    ? trialNumber
                    : objectives[ChartYTargetComboBox.SelectedIndex - 1];
                _viewModel.ChartPoints.Add(new ObservablePoint(x, y));
            }
        }

        internal void ClearPoints()
        {
            _viewModel.ChartPoints.Clear();
        }
    }
}
