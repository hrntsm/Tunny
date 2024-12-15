using System.Windows.Controls;

using Tunny.WPF.ViewModels.Output;

namespace Tunny.WPF.Views.Pages.Output
{
    public partial class AnalysisChartPage : Page
    {
        public AnalysisChartPage()
        {
            InitializeComponent();
            DataContext = new AnalysisChartViewModel();
        }
    }
}
