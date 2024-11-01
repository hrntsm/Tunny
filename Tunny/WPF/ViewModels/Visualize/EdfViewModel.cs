using Prism.Mvvm;

using Tunny.Core.Settings;
using Tunny.WPF.Common;

namespace Tunny.WPF.ViewModels.Visualize
{
    class EdfViewModel : BindableBase, IPlotSettings
    {
        private System.Collections.IEnumerable _studyNameItems;
        public System.Collections.IEnumerable StudyNameItems { get => _studyNameItems; set => SetProperty(ref _studyNameItems, value); }
        private object _selectedStudyName;
        public object SelectedStudyName { get => _selectedStudyName; set => SetProperty(ref _selectedStudyName, value); }
        private System.Collections.IEnumerable _objectiveItems;
        public System.Collections.IEnumerable ObjectiveItems { get => _objectiveItems; set => SetProperty(ref _objectiveItems, value); }
        private object _selectedObjective;
        public object SelectedObjective { get => _selectedObjective; set => SetProperty(ref _selectedObjective, value); }

        public Plot GetPlotSettings()
        {
            throw new System.NotImplementedException();
        }

    }
}
