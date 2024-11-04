using Tunny.Core.Settings;

namespace Tunny.WPF.ViewModels.Visualize
{
    class ParamImportancesViewModel : PlotSettingsViewModelBase
    {
        private System.Collections.IEnumerable _evaluatorItems;
        public System.Collections.IEnumerable EvaluatorItems { get => _evaluatorItems; set => SetProperty(ref _evaluatorItems, value); }
        private object _selectedEvaluator;
        public object SelectedEvaluator { get => _selectedEvaluator; set => SetProperty(ref _selectedEvaluator, value); }

        public override PlotSettings GetPlotSettings()
        {
            throw new System.NotImplementedException();
        }
    }
}
