using System.Windows;
using System.Windows.Controls;

using Tunny.Core.Input;
using Tunny.WPF.ViewModels.Optimize;

namespace Tunny.WPF.Views.Pages.Optimize
{
    public partial class OptimizePage : Page
    {
        private OptimizeViewModel _viewModel;

        public OptimizePage()
        {
            InitializeComponent();
            _viewModel = (OptimizeViewModel)DataContext;
        }

        private string CheckSender(object sender)
        {
            var textBox = (TextBox)sender;
            string value = textBox.Text;
            if (_viewModel == null)
            {
                _viewModel = (OptimizeViewModel)DataContext;
            }
            return value;
        }

        private void TrialParam1TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            string value = CheckSender(sender);
            _viewModel.TrialNumberParam1 = InputValidator.IsPositiveInt(value, false) ? value : "100";
        }

        private void TrialParam2TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            string value = CheckSender(sender);
            _viewModel.TrialNumberParam2 = InputValidator.IsPositiveInt(value, false) ? value : "10";
        }

        private void TimeoutTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            string value = CheckSender(sender);
            _viewModel.Timeout = InputValidator.IsPositiveInt(value, true) ? value : "0";
        }

        private void StudyNameTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            string value = CheckSender(sender);
            _viewModel.StudyName = string.IsNullOrWhiteSpace(value) ? "AUTO" : value;
        }
    }
}
