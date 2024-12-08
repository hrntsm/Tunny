using System.Windows;
using System.Windows.Controls;

using Tunny.Core.Input;

namespace Tunny.WPF.Views.Pages.Visualize
{
    public partial class HypervolumePage : Page
    {
        public HypervolumePage()
        {
            InitializeComponent();
        }

        private void ReferencePointTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = (TextBox)sender;
            string value = textBox.Text;
            textBox.Text = InputValidator.IsCommaSeparatedNumbers(value) ? value : "AUTO";
        }
    }
}
