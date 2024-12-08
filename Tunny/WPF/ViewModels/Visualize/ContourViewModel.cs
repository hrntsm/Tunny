using System.Linq;

using Optuna.Study;

using Tunny.Core.Settings;

namespace Tunny.WPF.ViewModels.Visualize
{
    internal sealed class ContourViewModel : PlotSettingsViewModelBase
    {
        public ContourViewModel() : base()
        {
        }

        public override PlotSettings GetPlotSettings()
        {
            return new PlotSettings()
            {
                TargetStudyName = SelectedStudyName.Name,
                PlotTypeName = "contour",
                TargetObjectiveName = new string[] { SelectedObjective.Name },
                TargetObjectiveIndex = new int[] { ObjectiveItems.IndexOf(SelectedObjective) },
                TargetVariableName = VariableItems.Where(v => v.IsSelected).Select(v => v.Name).ToArray(),
            };
        }
    }
}
