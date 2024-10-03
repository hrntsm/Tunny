using System.Windows;
using System.Windows.Controls.Ribbon;

using Tunny.Core.Util;

namespace Tunny.WPF
{
    public partial class MainWindow : RibbonWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void TunnyLicenseHelpButton_Click(object sender, RoutedEventArgs e)
        {
            OpenBrowser.TunnyLicense();
        }

        private void PythonPackagesLicenseHelpButton_Click(object sender, RoutedEventArgs e)
        {
            OpenBrowser.PythonPackagesLicense();
        }

        private void OtherLicenseHelpButton_Click(object sender, RoutedEventArgs e)
        {
            OpenBrowser.OtherLicense();
        }

        private void TunnyDocumentHelpButton_Click(object sender, RoutedEventArgs e)
        {
            OpenBrowser.TunnyDocumentPage();
        }

        private void OptunaSamplerHelpButton_Click(object sender, RoutedEventArgs e)
        {
            OpenBrowser.OptunaSamplerPage();
        }

        private void OptunaHubHelpButton_Click(object sender, RoutedEventArgs e)
        {
            OpenBrowser.OptunaHubPage();
        }

        private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
