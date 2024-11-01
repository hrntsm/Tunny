using Prism.Mvvm;

using Tunny.Core.Settings;
using Tunny.WPF.Common;

namespace Tunny.WPF.ViewModels.Visualize
{
    class ParamImportancesViewModel : BindableBase, IPlotSettings
    {
        private System.Collections.IEnumerable _studyNameItems;
        public System.Collections.IEnumerable StudyNameItems { get => _studyNameItems; set => SetProperty(ref _studyNameItems, value); }
        private object _selectedStudyName;
        public object SelectedStudyName { get => _selectedStudyName; set => SetProperty(ref _selectedStudyName, value); }
        private System.Collections.IEnumerable _evaluatorItems;
        public System.Collections.IEnumerable EvaluatorItems { get => _evaluatorItems; set => SetProperty(ref _evaluatorItems, value); }
        private object _selectedEvaluator;
        public object SelectedEvaluator { get => _selectedEvaluator; set => SetProperty(ref _selectedEvaluator, value); }
        private System.Collections.IEnumerable _paramItems;
        public System.Collections.IEnumerable ParamItems { get => _paramItems; set => SetProperty(ref _paramItems, value); }

        public Plot GetPlotSettings()
        {
            throw new System.NotImplementedException();
        }
    }
}
