using System.Windows.Controls;

using Optuna.Study;

using Tunny.WPF.ViewModels.Visualize;

namespace Tunny.WPF.Views.Pages.Visualize
{
    public partial class ParetoFrontPage : Page
    {
        public ParetoFrontPage()
        {
            InitializeComponent();
        }

        public ParetoFrontPage(StudySummary[] summaries)
        {
            InitializeComponent();
            DataContext = new ParetoFrontViewModel(summaries);
        }
    }
}
