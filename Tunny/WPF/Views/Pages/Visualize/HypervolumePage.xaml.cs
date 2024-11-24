using System.Windows;
using System.Windows.Controls;

using Optuna.Study;

using Tunny.Core.Input;
using Tunny.WPF.ViewModels.Visualize;
namespace Tunny.WPF.Views.Pages.Visualize
{
    public partial class HypervolumePage : Page
    {
        public HypervolumePage()
        {
            InitializeComponent();
        }

        public HypervolumePage(StudySummary[] summaries)
        {
            InitializeComponent();
            DataContext = new HypervolumeViewModel(summaries);
        }

        private void ReferencePointTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = (TextBox)sender;
            string value = textBox.Text;
            textBox.Text = InputValidator.IsCommaSeparatedNumbers(value) ? value : "AUTO";
        }
    }
}
