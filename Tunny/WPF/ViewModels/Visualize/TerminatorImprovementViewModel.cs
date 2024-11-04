using Tunny.Core.Settings;

namespace Tunny.WPF.ViewModels.Visualize
{
    class TerminatorImprovementViewModel : PlotSettingsViewModelBase
    {
        private System.Collections.IEnumerable _improvementEvaluatorItems;
        public System.Collections.IEnumerable ImprovementEvaluatorItems { get => _improvementEvaluatorItems; set => SetProperty(ref _improvementEvaluatorItems, value); }
        private object _selectedImprovementEvaluator;
        public object SelectedImprovementEvaluator { get => _selectedImprovementEvaluator; set => SetProperty(ref _selectedImprovementEvaluator, value); }
        private System.Collections.IEnumerable _errorEvaluatorItems;
        public System.Collections.IEnumerable ErrorEvaluatorItems { get => _errorEvaluatorItems; set => SetProperty(ref _errorEvaluatorItems, value); }
        private object _selectedErrorEvaluator;
        public object SelectedErrorEvaluator { get => _selectedErrorEvaluator; set => SetProperty(ref _selectedErrorEvaluator, value); }
        private string _minNumberOfTrials;
        public string MinNumberOfTrials { get => _minNumberOfTrials; set => SetProperty(ref _minNumberOfTrials, value); }
        private bool? _plotError;
        public bool? PlotError { get => _plotError; set => SetProperty(ref _plotError, value); }

        public TerminatorImprovementViewModel()
        {
            MinNumberOfTrials = "20";
            PlotError = false;
        }

        public override PlotSettings GetPlotSettings()
        {
            throw new System.NotImplementedException();
        }
    }
}
