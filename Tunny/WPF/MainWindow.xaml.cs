using System;
using System.IO;
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
using Tunny.WPF.Views.Windows;

namespace Tunny.WPF
{
    public partial class MainWindow : RibbonWindow, IDisposable
    {
        internal readonly GH_DocumentEditor DocumentEditor;
        internal readonly OptimizeComponentBase Component;

        private readonly TSettings _settings;
        private readonly OptimizePage _optimizePage;
        private readonly HelpPage _helpPage;

        public MainWindow(GH_DocumentEditor documentEditor, OptimizeComponentBase component)
        {
            TLog.MethodStart();
            InitializeComponent();
            _helpPage = new HelpPage();

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
            OpenHelpPage(HelpType.TunnyAbout);
        }

        private void TunnyLicenseHelpButton_Click(object sender, RoutedEventArgs e)
        {
            OpenHelpPage(HelpType.TunnyLicense);
        }

        private void PythonPackagesLicenseHelpButton_Click(object sender, RoutedEventArgs e)
        {
            OpenHelpPage(HelpType.PythonPackagesLicense);
        }

        private void OtherLicenseHelpButton_Click(object sender, RoutedEventArgs e)
        {
            OpenHelpPage(HelpType.OtherLicense);
        }

        private void TunnyDocumentHelpButton_Click(object sender, RoutedEventArgs e)
        {
            OpenHelpPage(HelpType.TunnyDocument);
        }

        private void OptunaSamplerHelpButton_Click(object sender, RoutedEventArgs e)
        {
            OpenHelpPage(HelpType.OptunaSampler);
        }

        private void OptunaHubHelpButton_Click(object sender, RoutedEventArgs e)
        {
            OpenHelpPage(HelpType.OptunaHub);
        }

        private void OpenHelpPage(HelpType helpType)
        {
            _helpPage.OpenSite(helpType);
            MainWindowFrame.Content = _helpPage;
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
            _settings.Optimize.SamplerType = samplerType;
            MainWindowFrame.Content = _optimizePage;
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
                Filter = @"Journal Storage(*.log)|*.log|SQLite Storage(*.db,*.sqlite)|*.db;*.sqlite|All Files (*.*)|*.*",
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
            _settings.Optimize = _optimizePage.GetCurrentSettings();
            _settings.Serialize(TEnvVariables.OptimizeSettingsPath);
        }

        private void VisualizeRunOptunaDashboardRibbonButton_Click(object sender, RoutedEventArgs e)
        {
            TLog.MethodStart();
            if (File.Exists(_settings.Storage.Path) == false)
            {
                TunnyMessageBox.Error_ResultFileNotExist();
                return;
            }
            string dashboardPath = Path.Combine(TEnvVariables.TunnyEnvPath, "python", "Scripts", "optuna-dashboard.exe");
            string storagePath = _settings.Storage.Path;

            var dashboard = new Optuna.Dashboard.Handler(dashboardPath, storagePath);
            dashboard.Run(true);
        }

        private void VisualizeRunDesignExplorerRibbonButton_Click(object sender, RoutedEventArgs e)
        {
            TLog.MethodStart();
            var selector = new TargetStudyNameSelector(_settings);
            selector.Show();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _helpPage?.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
