using System.Windows;

using Tunny.UI;

namespace Tunny.WPF
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void TunnyLicenseMenuItem_Click(object sender, RoutedEventArgs e)
        {
            TunnyMessageBox.Show("Tunny licensed under the MIT License.", "Tunny License");
        }

        private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
