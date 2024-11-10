using System.Linq;

using Tunny.Core.Settings;

namespace Tunny.WPF.ViewModels.Visualize
{
    internal class ParetoFrontViewModel : PlotSettingsViewModelBase
    {
        private bool? _includeDominatedTrials;
        public bool? IncludeDominatedTrials { get => _includeDominatedTrials; set => SetProperty(ref _includeDominatedTrials, value); }

        public ParetoFrontViewModel()
        {
            IncludeDominatedTrials = true;
        }

        public override PlotSettings GetPlotSettings()
        {
            return new PlotSettings()
            {
                TargetStudyName = SelectedStudyName.Name,
                PlotTypeName = "pareto front",
                IncludeDominatedTrials = IncludeDominatedTrials.Value,
                TargetObjectiveName = ObjectiveItems.Where(o => o.IsSelected).Select(o => o.Name).ToArray(),
                TargetObjectiveIndex = ObjectiveItems.Where(o => o.IsSelected).Select(o => ObjectiveItems.IndexOf(o)).ToArray()
            };
        }
    }
}
