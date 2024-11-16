using System.Windows.Controls;

using Optuna.Study;

using Tunny.WPF.ViewModels.Visualize;

namespace Tunny.WPF.Views.Pages.Visualize
{
    public partial class SlicePage : Page
    {
        public SlicePage()
        {
            InitializeComponent();
        }

        public SlicePage(StudySummary[] summaries)
        {
            InitializeComponent();
            DataContext = new SliceViewModel(summaries);
        }
    }
}
