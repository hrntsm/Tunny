using System.Windows;
using System.Windows.Controls.Ribbon;

using CefSharp;

using Tunny.Core.TEnum;
using Tunny.Core.Util;
using Tunny.WPF.Views.Pages;

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

        private void OptimizeTpeRibbonButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindowFrame.Content = new OptimizePage(SamplerType.TPE);
        }

        private void OptimizeGpOptunaRibbonButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindowFrame.Content = new OptimizePage(SamplerType.GP);
        }

        private void OptimizeGpBoTorchRibbonButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindowFrame.Content = new OptimizePage(SamplerType.BoTorch);
        }

        private void OptimizeNsgaiiRibbonButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindowFrame.Content = new OptimizePage(SamplerType.NSGAII);
        }

        private void OptimizeNsgaiiiRibbonButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindowFrame.Content = new OptimizePage(SamplerType.NSGAIII);
        }

        private void OptimizeCmaEsRibbonButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindowFrame.Content = new OptimizePage(SamplerType.CmaEs);
        }

        private void OptimizeRandomRibbonButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindowFrame.Content = new OptimizePage(SamplerType.Random);
        }

        private void OptimizeQmcRibbonButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindowFrame.Content = new OptimizePage(SamplerType.QMC);
        }

        private void OptimizeBruteForceRibbonButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindowFrame.Content = new OptimizePage(SamplerType.BruteForce);
        }

        private void RibbonButton_Click(object sender, RoutedEventArgs e)
        {

            var browser = new CefSharp.Wpf.ChromiumWebBrowser(@"C:\Users\dev\Desktop\my_plot.html");
            MainWindowFrame.Content = browser;
            int a = 0;
        }
    }
}
