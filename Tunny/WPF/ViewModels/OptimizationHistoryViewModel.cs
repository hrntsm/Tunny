using Prism.Mvvm;

using Tunny.Core.Settings;
using Tunny.WPF.Common;

namespace Tunny.WPF.ViewModels
{
    internal class OptimizationHistoryViewModel : BindableBase, IPlotSettings
    {
        public Plot GetPlotSettings()
        {
            throw new System.NotImplementedException();
        }

        private System.Collections.IEnumerable studyNameItems;

        public System.Collections.IEnumerable StudyNameItems { get => studyNameItems; set => SetProperty(ref studyNameItems, value); }

        private object selectedStudyName;

        public object SelectedStudyName { get => selectedStudyName; set => SetProperty(ref selectedStudyName, value); }

        private System.Collections.IEnumerable objectiveItems;

        public System.Collections.IEnumerable ObjectiveItems { get => objectiveItems; set => SetProperty(ref objectiveItems, value); }

        private object selectedObjective;

        public object SelectedObjective { get => selectedObjective; set => SetProperty(ref selectedObjective, value); }

        private System.Collections.IEnumerable compareStudyNameItems;

        public System.Collections.IEnumerable CompareStudyNameItems { get => compareStudyNameItems; set => SetProperty(ref compareStudyNameItems, value); }

        private bool? showErrorBar;

        public bool? ShowErrorBar { get => showErrorBar; set => SetProperty(ref showErrorBar, value); }
    }
}
