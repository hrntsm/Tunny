using System.Windows.Controls;

using Optuna.Study;

using Tunny.WPF.ViewModels.Visualize;

namespace Tunny.WPF.Views.Pages.Visualize
{
    public partial class OptimizationHistoryPage : Page
    {
        public OptimizationHistoryPage()
        {
            InitializeComponent();
        }

        public OptimizationHistoryPage(StudySummary[] summaries)
        {
            InitializeComponent();
            DataContext = new OptimizationHistoryViewModel(summaries);
        }
    }
}
