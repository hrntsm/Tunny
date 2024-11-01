using Prism.Mvvm;

using Tunny.Core.Settings;
using Tunny.WPF.Common;

namespace Tunny.WPF.ViewModels.Visualize
{
    class TimelineViewModel : BindableBase, IPlotSettings
    {
        private System.Collections.IEnumerable _studyNameItems;
        public System.Collections.IEnumerable StudyNameItems { get => _studyNameItems; set => SetProperty(ref _studyNameItems, value); }
        private object _selectedStudyName;
        public object SelectedStudyName { get => _selectedStudyName; set => SetProperty(ref _selectedStudyName, value); }

        public Plot GetPlotSettings()
        {
            throw new System.NotImplementedException();
        }
    }
}
