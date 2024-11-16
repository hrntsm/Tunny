using System.Windows.Controls;

using Optuna.Study;

using Tunny.WPF.ViewModels.Visualize;

namespace Tunny.WPF.Views.Pages.Visualize
{
    public partial class RankPage : Page
    {
        public RankPage()
        {
            InitializeComponent();
        }

        public RankPage(StudySummary[] summaries)
        {
            InitializeComponent();
            DataContext = new RankViewModel(summaries);
        }
    }
}
