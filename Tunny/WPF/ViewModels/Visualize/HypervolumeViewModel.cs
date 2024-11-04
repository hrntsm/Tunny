using Tunny.Core.Settings;

namespace Tunny.WPF.ViewModels.Visualize
{
    class HypervolumeViewModel : PlotSettingsViewModelBase
    {
        private string _referencePoint;
        public string ReferencePoint { get => _referencePoint; set => SetProperty(ref _referencePoint, value); }

        public override PlotSettings GetPlotSettings()
        {
            throw new System.NotImplementedException();
        }
    }
}
