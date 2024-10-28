using System.Windows.Controls;

using Optuna.Study;

using Tunny.Core.Settings;
using Tunny.WPF.ViewModels;

namespace Tunny.WPF.Views.Pages.Visualize
{
    public partial class ParetoFrontPage : Page
    {
        private readonly ParetoFrontViewModel _viewModel;

        public ParetoFrontPage()
        {
            InitializeComponent();
            _viewModel = new ParetoFrontViewModel();
            DataContext = _viewModel;
        }

        public void SetTargetStudy(StudySummary targetStudySummary)
        {
            _viewModel.SetTargetStudy(targetStudySummary);
        }

        internal Plot GetPlotSettings()
        {
            return _viewModel.GetPlotSettings();
        }
    }
}
