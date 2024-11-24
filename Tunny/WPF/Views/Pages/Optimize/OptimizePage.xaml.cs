using System.Windows;
using System.Windows.Controls;

using Tunny.Core.Input;
using Tunny.WPF.ViewModels.Optimize;

namespace Tunny.WPF.Views.Pages.Optimize
{
    public partial class OptimizePage : Page
    {
        private readonly OptimizeViewModel _viewModel;
        public OptimizePage()
        {
            InitializeComponent();
            _viewModel = (OptimizeViewModel)DataContext;
        }

        private void TrialParam1TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = (TextBox)sender;
            string value = textBox.Text;
            _viewModel.TrialNumberParam1 = InputValidator.IsPositiveInt(value, false) ? value : "100";
        }

        private void TrialParam2TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = (TextBox)sender;
            string value = textBox.Text;
            _viewModel.TrialNumberParam2 = InputValidator.IsPositiveInt(value, false) ? value : "10";
        }

        private void TimeoutTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = (TextBox)sender;
            string value = textBox.Text;
            _viewModel.Timeout = InputValidator.IsPositiveInt(value, true) ? value : "0";
        }
    }
}
