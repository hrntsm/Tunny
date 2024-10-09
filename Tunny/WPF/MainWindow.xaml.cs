using System.Windows;
using System.Windows.Controls.Ribbon;

using Tunny.Core.TEnum;
using Tunny.Type;
using Tunny.WPF.Common;
using Tunny.WPF.Views.Pages;

namespace Tunny.WPF
{
    public partial class MainWindow : RibbonWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void TunnyAboutButton_Click(object sender, RoutedEventArgs e)
        {
            var page = new HelpPage(HelpType.TunnyAbout);
            MainWindowFrame.Content = page;
        }

        private void TunnyLicenseHelpButton_Click(object sender, RoutedEventArgs e)
        {
            var page = new HelpPage(HelpType.TunnyLicense);
            MainWindowFrame.Content = page;
        }

        private void PythonPackagesLicenseHelpButton_Click(object sender, RoutedEventArgs e)
        {
            var page = new HelpPage(HelpType.PythonPackagesLicense);
            MainWindowFrame.Content = page;
        }

        private void OtherLicenseHelpButton_Click(object sender, RoutedEventArgs e)
        {
            var page = new HelpPage(HelpType.OtherLicense);
            MainWindowFrame.Content = page;
        }

        private void TunnyDocumentHelpButton_Click(object sender, RoutedEventArgs e)
        {
            var page = new HelpPage(HelpType.TunnyDocument);
            MainWindowFrame.Content = page;
        }

        private void OptunaSamplerHelpButton_Click(object sender, RoutedEventArgs e)
        {
            var page = new HelpPage(HelpType.OptunaSampler);
            MainWindowFrame.Content = page;
        }

        private void OptunaHubHelpButton_Click(object sender, RoutedEventArgs e)
        {
            var page = new HelpPage(HelpType.OptunaHub);
            MainWindowFrame.Content = page;
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

        private void VisualizeParetoFrontRibbonButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindowFrame.Content = new VisualizePage();
        }

        private void QuickAccessFileOpenRibbonButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog
            {
                FileName = "fish.log",
                DefaultExt = "log",
                Filter = @"Journal Storage(*.log)|*.log|SQLite Storage(*.db,*.sqlite)|*.db;*.sqlite",
                Title = @"Set Tunny Result File Path",
            };

            bool? result = dialog.ShowDialog();
            if (result == true)
            {
                Title = "Tunny v1.0 - " + dialog.FileName;
            }
        }

        private void QuickAccessFileSaveRibbonButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
