using Tunny.Core.Settings;

namespace Tunny.WPF.Common
{
    public interface IPlotSettings
    {
        bool TryGetPlotSettings(out PlotSettings plotSettings);
    }
}
