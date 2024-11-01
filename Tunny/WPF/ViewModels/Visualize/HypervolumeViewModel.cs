using Prism.Mvvm;

using Tunny.Core.Settings;
using Tunny.WPF.Common;

namespace Tunny.WPF.ViewModels.Visualize
{
    class HypervolumeViewModel : BindableBase, IPlotSettings
    {
        private System.Collections.IEnumerable _studyNameItems;
        public System.Collections.IEnumerable StudyNameItems { get => _studyNameItems; set => SetProperty(ref _studyNameItems, value); }
        private object _selectedStudyName;
        public object SelectedStudyName { get => _selectedStudyName; set => SetProperty(ref _selectedStudyName, value); }
        private string _referencePoint;
        public string ReferencePoint { get => _referencePoint; set => SetProperty(ref _referencePoint, value); }

        public Plot GetPlotSettings()
        {
            throw new System.NotImplementedException();
        }

    }
}
