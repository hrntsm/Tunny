using System.Windows.Controls;

using Optuna.Study;

using Tunny.WPF.ViewModels.Visualize;

namespace Tunny.WPF.Views.Pages.Visualize
{
    public partial class ParallelCoordinatePage : Page
    {
        public ParallelCoordinatePage()
        {
            InitializeComponent();
        }

        public ParallelCoordinatePage(StudySummary[] summaries)
        {
            InitializeComponent();
            DataContext = new ParallelCoordinateViewModel(summaries);
        }
    }
}
