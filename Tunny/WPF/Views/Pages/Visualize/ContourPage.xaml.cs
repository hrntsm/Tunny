using System.Windows.Controls;

using Optuna.Study;

using Tunny.WPF.ViewModels.Visualize;

namespace Tunny.WPF.Views.Pages.Visualize
{
    public partial class ContourPage : Page
    {
        public ContourPage()
        {
            InitializeComponent();
        }

        public ContourPage(StudySummary[] summaries)
        {
            InitializeComponent();
            DataContext = new ContourViewModel(summaries);
        }
    }
}
