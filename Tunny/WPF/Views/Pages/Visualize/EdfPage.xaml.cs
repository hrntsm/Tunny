using System.Windows.Controls;

using Optuna.Study;

using Tunny.WPF.ViewModels.Visualize;

namespace Tunny.WPF.Views.Pages.Visualize
{
    public partial class EdfPage : Page
    {
        public EdfPage()
        {
            InitializeComponent();
        }

        public EdfPage(StudySummary[] summaries)
        {
            InitializeComponent();
            DataContext = new EdfViewModel(summaries);
        }
    }
}
