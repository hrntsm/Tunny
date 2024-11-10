using Tunny.Core.Settings;

namespace Tunny.WPF.ViewModels.Visualize
{
    class TimelineViewModel : PlotSettingsViewModelBase
    {
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
