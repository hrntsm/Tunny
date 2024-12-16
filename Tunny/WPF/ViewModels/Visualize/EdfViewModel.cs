using System.Linq;

using Tunny.Core.Settings;

namespace Tunny.WPF.ViewModels.Visualize
{
    internal sealed class EdfViewModel : PlotSettingsViewModelBase
    {
        public EdfViewModel() : base()
        {
        }

        public override PlotSettings GetPlotSettings()
        {
            return new PlotSettings()
            {
                TargetStudyName = SelectedStudyName.Name,
                PlotTypeName = "EDF",
                TargetObjectiveName = ObjectiveItems.Where(o => o.IsSelected).Select(o => o.Name).ToArray(),
                TargetObjectiveIndex = ObjectiveItems.Where(o => o.IsSelected).Select(o => ObjectiveItems.IndexOf(o)).ToArray()
            };
        }
    }
}
