using System.Windows.Controls;

using Optuna.Study;

using Tunny.WPF.ViewModels.Visualize;

namespace Tunny.WPF.Views.Pages.Visualize
{
    public partial class TimelinePage : Page
    {
        public TimelinePage()
        {
            InitializeComponent();
        }

        public TimelinePage(StudySummary[] summaries)
        {
            InitializeComponent();
            DataContext = new TimelineViewModel(summaries);
        }
    }
}
