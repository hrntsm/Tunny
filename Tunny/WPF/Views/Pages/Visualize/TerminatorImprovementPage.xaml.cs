using System.Windows.Controls;

using Optuna.Study;

using Tunny.WPF.ViewModels.Visualize;

namespace Tunny.WPF.Views.Pages.Visualize
{
    public partial class TerminatorImprovementPage : Page
    {
        public TerminatorImprovementPage()
        {
            InitializeComponent();
        }

        public TerminatorImprovementPage(StudySummary[] summaries)
        {
            InitializeComponent();
            DataContext = new TerminatorImprovementViewModel(summaries);
        }
    }
}
