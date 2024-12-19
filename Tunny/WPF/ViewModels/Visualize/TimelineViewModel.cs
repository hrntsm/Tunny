using Tunny.Core.Settings;

namespace Tunny.WPF.ViewModels.Visualize
{
    internal sealed class TimelineViewModel : PlotSettingsViewModelBase
    {
        public TimelineViewModel() : base()
        {
        }

        public override bool TryGetPlotSettings(out PlotSettings plotSettings)
        {
            if (StudyNameItems.Count == 0 || SelectedStudyName == null)
            {
                plotSettings = null;
                return false;
            }

            plotSettings = new PlotSettings
            {
                PlotTypeName = "timeline",
                TargetStudyName = SelectedStudyName.Name,
            };
            return true;
        }
    }
}
