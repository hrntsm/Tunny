using System.Windows.Controls;

using Optuna.Study;

using Tunny.WPF.ViewModels.Visualize;
namespace Tunny.WPF.Views.Pages.Visualize
{
    public partial class HypervolumePage : Page
    {
        public HypervolumePage()
        {
            InitializeComponent();
        }

        public HypervolumePage(StudySummary[] summaries)
        {
            InitializeComponent();
            DataContext = new HypervolumeViewModel(summaries);
        }
    }
}
