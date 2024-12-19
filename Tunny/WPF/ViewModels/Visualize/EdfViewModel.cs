using Tunny.Core.Settings;

namespace Tunny.WPF.ViewModels.Visualize
{
    internal sealed class EdfViewModel : PlotSettingsViewModelBase
    {
        public EdfViewModel() : base()
        {
        }

        public override bool TryGetPlotSettings(out PlotSettings plotSettings)
        {
            if (StudyNameItems.Count == 0 || SelectedStudyName == null ||
                ObjectiveItems.Count == 0)
            {
                plotSettings = null;
                return false;
            }

            plotSettings = new PlotSettings
            {
                TargetStudyName = SelectedStudyName.Name,
                PlotTypeName = "EDF",
                TargetObjectiveName = new[] { SelectedObjective.Name },
                TargetObjectiveIndex = new[] { ObjectiveItems.IndexOf(SelectedObjective) }
            };
            return true;
        }
    }
}
