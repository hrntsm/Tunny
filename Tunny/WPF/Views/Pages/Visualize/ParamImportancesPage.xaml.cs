using System.Windows.Controls;

using Optuna.Study;

using Tunny.WPF.ViewModels.Visualize;

namespace Tunny.WPF.Views.Pages.Visualize
{
    public partial class ParamImportancesPage : Page
    {
        public ParamImportancesPage()
        {
            InitializeComponent();
        }

        public ParamImportancesPage(StudySummary[] summaries)
        {
            InitializeComponent();
            DataContext = new ParamImportancesViewModel(summaries);
        }
    }
}
