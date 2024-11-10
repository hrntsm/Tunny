using System.Collections.ObjectModel;

using Tunny.Core.Settings;
using Tunny.WPF.Models;

namespace Tunny.WPF.ViewModels.Visualize
{
    internal class OptimizationHistoryViewModel : PlotSettingsViewModelBase
    {
        private ObservableCollection<NameComboBoxItem> _compareStudyNameItems;
        public ObservableCollection<NameComboBoxItem> CompareStudyNameItems { get => _compareStudyNameItems; set => SetProperty(ref _compareStudyNameItems, value); }
        private bool? _showErrorBar;
        public bool? ShowErrorBar { get => _showErrorBar; set => SetProperty(ref _showErrorBar, value); }

        public OptimizationHistoryViewModel()
        {
            ShowErrorBar = false;
        }

        public override PlotSettings GetPlotSettings()
        {
            return new PlotSettings
            {
                TargetStudyName = SelectedStudyName.Name,
                PlotTypeName = "optimization history",
                TargetObjectiveName = new string[] { SelectedObjective.Name },
                TargetObjectiveIndex = new int[] { ObjectiveItems.IndexOf(SelectedObjective) },
            };
        }
    }
}
