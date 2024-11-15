using System.Collections.ObjectModel;

using Optuna.Study;

using Tunny.Core.Settings;

namespace Tunny.WPF.ViewModels.Visualize
{
    internal class TerminatorImprovementViewModel : PlotSettingsViewModelBase
    {
        private ObservableCollection<string> _improvementEvaluatorItems;
        public ObservableCollection<string> ImprovementEvaluatorItems { get => _improvementEvaluatorItems; set => SetProperty(ref _improvementEvaluatorItems, value); }
        private string _selectedImprovementEvaluator;
        public string SelectedImprovementEvaluator { get => _selectedImprovementEvaluator; set => SetProperty(ref _selectedImprovementEvaluator, value); }
        private ObservableCollection<string> _errorEvaluatorItems;
        public ObservableCollection<string> ErrorEvaluatorItems { get => _errorEvaluatorItems; set => SetProperty(ref _errorEvaluatorItems, value); }
        private object _selectedErrorEvaluator;
        public object SelectedErrorEvaluator { get => _selectedErrorEvaluator; set => SetProperty(ref _selectedErrorEvaluator, value); }
        private string _minNumberOfTrials;
        public string MinNumberOfTrials { get => _minNumberOfTrials; set => SetProperty(ref _minNumberOfTrials, value); }
        private bool? _plotError;
        public bool? PlotError { get => _plotError; set => SetProperty(ref _plotError, value); }

        public TerminatorImprovementViewModel() : base()
        {
        }

        public TerminatorImprovementViewModel(StudySummary[] summaries) : base(summaries)
        {
            MinNumberOfTrials = "20";
            PlotError = false;

            ImprovementEvaluatorItems = new ObservableCollection<string>()
            {
                "AUTO",
                "RegretBound",
                "BestValueStagnation",
                "EMMR"
            };
            SelectedImprovementEvaluator = ImprovementEvaluatorItems[0];

            ErrorEvaluatorItems = new ObservableCollection<string>()
            {
                "AUTO",
                "StaticError",
                "MedianError",
            };
            SelectedErrorEvaluator = ErrorEvaluatorItems[0];
        }

        public override PlotSettings GetPlotSettings()
        {
            throw new System.NotImplementedException();
        }
    }
}
