using Tunny.Core.Settings;

namespace Tunny.WPF.ViewModels.Visualize
{
    internal class OptimizationHistoryViewModel : PlotSettingsViewModelBase
    {
        private System.Collections.IEnumerable _compareStudyNameItems;
        public System.Collections.IEnumerable CompareStudyNameItems { get => _compareStudyNameItems; set => SetProperty(ref _compareStudyNameItems, value); }
        private bool? _showErrorBar;
        public bool? ShowErrorBar { get => _showErrorBar; set => SetProperty(ref _showErrorBar, value); }

        public OptimizationHistoryViewModel()
        {
            ShowErrorBar = false;
        }

        public override PlotSettings GetPlotSettings()
        {
            throw new System.NotImplementedException();
        }
    }
}
