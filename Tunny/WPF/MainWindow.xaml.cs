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

        private void TunnyLicenseMenuItem_Click(object sender, RoutedEventArgs e)
        {
            OpenBrowser.TunnyLicense();
        }

        private void PythonPackagesLicenseMenuItem_Click(object sender, RoutedEventArgs e)
        {
            OpenBrowser.PythonPackagesLicense();
        }

        private void TTDesignExplorerLicenseMenuItem_Click(object sender, RoutedEventArgs e)
        {
            OpenBrowser.TTDesignExplorerLicense();
        }

        private void TunnyDocumentMenuItem_Click(object sender, RoutedEventArgs e)
        {
            OpenBrowser.TunnyDocumentPage();
        }

        private void OptunaSamplerMenuItem_Click(object sender, RoutedEventArgs e)
        {
            OpenBrowser.OptunaSamplerPage();
        }

        private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
