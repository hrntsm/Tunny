using System.Windows;
using System.Windows.Controls.Ribbon;

using Grasshopper.GUI;

using Tunny.Component.Optimizer;
using Tunny.Core.Settings;
using Tunny.Core.TEnum;
using Tunny.Core.Util;
using Tunny.WPF.Common;
using Tunny.WPF.Views.Pages;
using Tunny.WPF.Views.Pages.Optimize;

namespace Tunny.WPF
{
    public partial class MainWindow : RibbonWindow
    {
        internal readonly GH_DocumentEditor DocumentEditor;
        internal readonly OptimizeComponentBase Component;

        private readonly TSettings _settings;
        private readonly OptimizePage _optimizePage;

        public MainWindow(GH_DocumentEditor documentEditor, OptimizeComponentBase component)
        {
            TLog.MethodStart();
            InitializeComponent();

            DocumentEditor = documentEditor;
            Component = component;
            Component.GhInOutInstantiate();
            if (!Component.GhInOut.IsLoadCorrectly)
            {
                Close();
            }
            _settings = TSettings.LoadFromJson();
            _optimizePage = new OptimizePage(_settings);
            MainWindowFrame.Content = _optimizePage;

            UpdateTitle();
        }

        private void UpdateTitle()
        {
            string storagePath = _settings.Storage.Path;
            Title = $"Tunny v{TEnvVariables.Version.ToString(2)} - {storagePath}";
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
            SetSamplerType(SamplerType.TPE);
        }

        private void OptimizeGpOptunaRibbonButton_Click(object sender, RoutedEventArgs e)
        {
            SetSamplerType(SamplerType.GP);
        }

        private void OptimizeGpBoTorchRibbonButton_Click(object sender, RoutedEventArgs e)
        {
            SetSamplerType(SamplerType.BoTorch);
        }

        private void OptimizeGpPreferentialRibbonButton_Click(object sender, RoutedEventArgs e)
        {
            SetSamplerType(SamplerType.GpPreferential);
        }

        private void OptimizeNsgaiiRibbonButton_Click(object sender, RoutedEventArgs e)
        {
            SetSamplerType(SamplerType.NSGAII);
        }

        private void OptimizeNsgaiiiRibbonButton_Click(object sender, RoutedEventArgs e)
        {
            SetSamplerType(SamplerType.NSGAIII);
        }

        private void OptimizeCmaEsRibbonButton_Click(object sender, RoutedEventArgs e)
        {
            SetSamplerType(SamplerType.CmaEs);
        }

        private void OptimizeRandomRibbonButton_Click(object sender, RoutedEventArgs e)
        {
            SetSamplerType(SamplerType.Random);
        }

        private void OptimizeQmcRibbonButton_Click(object sender, RoutedEventArgs e)
        {
            SetSamplerType(SamplerType.QMC);
        }

        private void OptimizeBruteForceRibbonButton_Click(object sender, RoutedEventArgs e)
        {
            SetSamplerType(SamplerType.BruteForce);
        }

        private void SetSamplerType(SamplerType samplerType)
        {
            _optimizePage.ChangeTargetSampler(samplerType);
            _settings.Optimize.SelectSampler = samplerType;
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
                _settings.Storage.Path = dialog.FileName;
                UpdateTitle();
            }
        }

        private void QuickAccessFileSaveRibbonButton_Click(object sender, RoutedEventArgs e)
        {
            _settings.Optimize.Sampler = _optimizePage.GetCurrentSettings();
            _settings.Serialize(TEnvVariables.OptimizeSettingsPath);
        }
    }
}
