using System.IO;
using System.Windows;
using System.Windows.Controls.Ribbon;

using Grasshopper.GUI;

using Tunny.Component.Optimizer;
using Tunny.Core.Settings;
using Tunny.Core.Util;
using Tunny.Process;
using Tunny.WPF.Common;
using Tunny.WPF.Views.Windows;

namespace Tunny.WPF
{
    public partial class MainWindow : RibbonWindow
    {
        internal readonly GH_DocumentEditor DocumentEditor;

        private readonly TSettings _settings;

        public MainWindow(GH_DocumentEditor documentEditor, OptimizeComponentBase component)
        {
            TLog.MethodStart();
            DocumentEditor = documentEditor;
            component.GhInOutInstantiate();
            if (!component.GhInOut.IsLoadCorrectly)
            {
                TunnyMessageBox.Error_ComponentLoadFail();
                Close();
            }
            OptimizeProcess.Component = component;

            if (!TSettings.TryLoadFromJson(out _settings))
            {
                TunnyMessageBox.Warn_SettingsJsonFileLoadFail();
            }
            OptimizeProcess.Settings = _settings;

            InitializeComponent();
            UpdateTitle();
        }

        private void UpdateTitle()
        {
            string storagePath = _settings.Storage.Path;
            Title = $"Tunny v{TEnvVariables.Version.ToString(2)} - {storagePath}";
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
    }
}
