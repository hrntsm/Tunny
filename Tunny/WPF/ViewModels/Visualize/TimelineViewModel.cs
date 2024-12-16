using Tunny.Core.Settings;

namespace Tunny.WPF.ViewModels.Visualize
{
    internal sealed class TimelineViewModel : PlotSettingsViewModelBase
    {
        public TimelineViewModel() : base()
        {
        }

        public override PlotSettings GetPlotSettings()
        {
            return new PlotSettings
            {
                PlotTypeName = "timeline",
                TargetStudyName = SelectedStudyName.Name,
            };
        }
    }
}
