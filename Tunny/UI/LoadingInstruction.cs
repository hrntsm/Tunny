using System;
using System.IO;
using System.Windows;
using System.Windows.Forms;

using Grasshopper.GUI;
using Grasshopper.GUI.Canvas;
using Grasshopper.Kernel;

using Tunny.Core.Settings;
using Tunny.Core.Util;
using Tunny.Resources;

namespace Tunny.UI
{
    public class LoadingInstruction : GH_AssemblyPriority, IDisposable
    {
        private ToolStripMenuItem _tutorialStripMenuItem;
        private ToolStripMenuItem _tunnyHelpStripMenuItem;
        private ToolStripMenuItem _optunaDashboardToolStripMenuItem;
        private ToolStripMenuItem _pythonInstallStripMenuItem;
        private ToolStripMenuItem _ttDesignExplorerToolStripMenuItem;
        private ToolStripMenuItem _aboutTunnyStripMenuItem;

        public override GH_LoadingInstruction PriorityLoad()
        {
            TLog.InitializeLogger();
            Grasshopper.Instances.ComponentServer.AddCategoryIcon("Tunny", Resource.TunnyIcon);
            Grasshopper.Instances.ComponentServer.AddCategorySymbolName("Tunny", 'T');
            Grasshopper.Instances.CanvasCreated += RegisterTunnyMenuItems;
            return GH_LoadingInstruction.Proceed;
        }

        void RegisterTunnyMenuItems(GH_Canvas canvas)
        {
            TLog.MethodStart();
            Grasshopper.Instances.CanvasCreated -= RegisterTunnyMenuItems;

            GH_DocumentEditor docEditor = Grasshopper.Instances.DocumentEditor;
            if (docEditor != null)
            {
                SetupTunnyMenu(docEditor);
            }
        }

        private void SetupTunnyMenu(GH_DocumentEditor docEditor)
        {
            TLog.MethodStart();
            ToolStripMenuItem tunnyToolStripMenuItem;
            tunnyToolStripMenuItem = new ToolStripMenuItem();

            docEditor.MainMenuStrip.SuspendLayout();

            docEditor.MainMenuStrip.Items.AddRange(new ToolStripItem[] {
                tunnyToolStripMenuItem
            });

            tunnyToolStripMenuItem.Name = "TunnyToolStripMenuItem";
            tunnyToolStripMenuItem.Size = new System.Drawing.Size(125, 29);
            tunnyToolStripMenuItem.Text = "Tunny";
            AddTunnyMenuItems(tunnyToolStripMenuItem.DropDownItems);

            docEditor.MainMenuStrip.ResumeLayout(false);
            docEditor.MainMenuStrip.PerformLayout();

            GH_DocumentEditor.AggregateShortcutMenuItems += GH_DocumentEditor_AggregateShortcutMenuItems;
        }

        void GH_DocumentEditor_AggregateShortcutMenuItems(object sender, GH_MenuShortcutEventArgs e)
        {
            e.AppendItem(_optunaDashboardToolStripMenuItem);
        }

        private void AddTunnyMenuItems(ToolStripItemCollection dropDownItems)
        {
            TLog.MethodStart();
            _tunnyHelpStripMenuItem = new ToolStripMenuItem("Help", null, null, "TunnyHelpStripMenuItem");
            _tutorialStripMenuItem = new ToolStripMenuItem("Tutorial Files", null, null, "TutorialStripMenuItem");
            _optunaDashboardToolStripMenuItem = new ToolStripMenuItem("Run Optuna Dashboard...", Resource.optuna_dashboard, OptunaDashboardToolStripMenuItem_Click, "OptunaDashboardToolStripMenuItem");
            _pythonInstallStripMenuItem = new ToolStripMenuItem("Install Python...", null, PythonInstallStripMenuItem_Click, "PythonInstallStripMenuItem");
            _ttDesignExplorerToolStripMenuItem = new ToolStripMenuItem("Run TT DesignExplorer...", Resource.TTDesignExplorer, TTDesignExplorerToolStripMenuItem_Click, "TTDesignExplorerToolStripMenuItem");
            _aboutTunnyStripMenuItem = new ToolStripMenuItem("About...", Resource.TunnyIcon, AboutTunnyStripMenuItem_Click, "AboutTunnyStripMenuItem");

            SetHelpDropDownItems();
            SetTutorialDropDownItems();

            dropDownItems.AddRange(new ToolStripItem[] {
                _tunnyHelpStripMenuItem,
                _tutorialStripMenuItem,
                new ToolStripSeparator(),
                _optunaDashboardToolStripMenuItem,
                _ttDesignExplorerToolStripMenuItem,
                new ToolStripSeparator(),
                _pythonInstallStripMenuItem,
                new ToolStripSeparator(),
                _aboutTunnyStripMenuItem
            });
        }

        private void SetTutorialDropDownItems()
        {
            TLog.MethodStart();
            var optExample = new ToolStripMenuItem("Optimization", null, null, "TutorialOptimizationStripMenuItem");
            var hitlExample = new ToolStripMenuItem("Human-in-the-loop", null, null, "TutorialHITLStripMenuItem");
            string[] optFiles = Directory.GetFiles(Path.Combine(TEnvVariables.ExampleDirPath, "Optimization"), "*.gh");
            string[] hitlFiles = Directory.GetFiles(Path.Combine(TEnvVariables.ExampleDirPath, "Human-in-the-loop"), "*.gh");

            SetMenuItemsFromFilePath(optExample, optFiles);
            SetMenuItemsFromFilePath(hitlExample, hitlFiles);

            _tutorialStripMenuItem.DropDownItems.AddRange(new[] { optExample, hitlExample });
        }

        private static void SetMenuItemsFromFilePath(ToolStripMenuItem menuItem, string[] filePaths)
        {
            TLog.MethodStart();
            for (int i = 0; i < filePaths.Length; i++)
            {
                string file = filePaths[i];
                string fileName = Path.GetFileNameWithoutExtension(file);
                var optItem = new ToolStripMenuItem("0" + i + " " + fileName, null, (sender, e) =>
                {
                    Grasshopper.Instances.DocumentServer.AddDocument(file, makeActive: true);
                }, fileName);
                menuItem.DropDownItems.Add(optItem);
            }
        }

        private void SetHelpDropDownItems()
        {
            TLog.MethodStart();
            _tunnyHelpStripMenuItem.DropDownItems.AddRange(new[]{
                new ToolStripMenuItem("Tunny Document", null, TunnyDocumentPageStripMenuItem_Click, "TunnyDocumentStripMenuItem"),
                new ToolStripMenuItem("Optuna Sampler Document", null, OptunaSamplerPageStripMenuItem_Click, "OptunaTutorialStripMenuItem")
            });
        }

        private void AboutTunnyStripMenuItem_Click(object sender, EventArgs e)
        {
            TLog.MethodStart();
            TLog.Debug("AboutTunnyStripMenuItem Clicked");
            TunnyMessageBox.Show(
                "Tunny\nVersion: " + TEnvVariables.Version + "\n\n🐟Tunny🐟 is Grasshopper's optimization component using Optuna, an open source hyperparameter auto-optimization framework.\n\nTunny is developed by hrntsm.\nFor more information, visit https://tunny-docs.deno.dev/",
                "About Tunny",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        private void PythonInstallStripMenuItem_Click(object sender, EventArgs e)
        {
            TLog.MethodStart();
            TLog.Debug("PythonInstallStripMenuItem Clicked");
            if (Directory.Exists(TEnvVariables.PythonPath))
            {
                MessageBoxResult result = TunnyMessageBox.Show(
                    "It appears that the Tunny Python environment is already installed.\nWould you like to reinstall it?",
                    "Python is already installed",
                    MessageBoxButton.OKCancel,
                    MessageBoxImage.Information);
                if (result == MessageBoxResult.Cancel)
                {
                    TLog.Info("From menu item Python installation canceled by user.");
                    return;
                }
            }
            TLog.Info("From menu item Python installation started.");
            var pythonInstallDialog = new PythonInstallDialog();
            pythonInstallDialog.ShowDialog();
            var settings = TSettings.LoadFromJson();
            settings.CheckPythonLibraries = false;
            settings.Serialize(TEnvVariables.OptimizeSettingsPath);
        }

        private void TunnyDocumentPageStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenBrowser.TunnyDocumentPage();
        }

        private void OptunaSamplerPageStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenBrowser.OptunaSamplerPage();
        }

        private void OptunaDashboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TLog.MethodStart();
            TLog.Debug("OptunaDashboardToolStripMenuItem Clicked");
            string pythonDirectory = Path.Combine(TEnvVariables.TunnyEnvPath, "python");
            string dashboardPath = Path.Combine(pythonDirectory, "Scripts", "optuna-dashboard.exe");

            if (!Directory.Exists(pythonDirectory) && !File.Exists(dashboardPath))
            {
                TunnyMessageBox.Show("optuna-dashboard is not installed.\nFirst install optuna-dashboard from the Tunny component.",
                                     "optuna-dashboard is not installed",
                                     MessageBoxButton.OK,
                                     MessageBoxImage.Error);
            }
            else
            {
                RunOptunaDashboard(dashboardPath);
            }
        }

        private static void RunOptunaDashboard(string dashboardPath)
        {
            TLog.MethodStart();
            string settingsPath = TEnvVariables.OptimizeSettingsPath;
            string storagePath = string.Empty;
            if (File.Exists(settingsPath))
            {
                var settings = TSettings.Deserialize(settingsPath);
                storagePath = settings.Storage.Path;
            }
            var ofd = new Microsoft.Win32.OpenFileDialog
            {
                FileName = Path.GetFileName(storagePath),
                Filter = @"Journal Storage(*.log)|*.log|SQLite Storage(*.db,*.sqlite)|*.db;*.sqlite",
                Title = @"Set Tunny result file path",
            };
            if (ofd.ShowDialog() == true)
            {
                var dashboard = new Optuna.Dashboard.Handler(dashboardPath, ofd.FileName);
                dashboard.Run(true);
            }
        }

        private void TTDesignExplorerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TLog.MethodStart();
            string settingsPath = TEnvVariables.OptimizeSettingsPath;
            string storagePath = string.Empty;
            TSettings settings;
            if (File.Exists(settingsPath))
            {
                settings = TSettings.Deserialize(settingsPath);
                storagePath = settings.Storage.Path;
            }
            else
            {
                settings = new TSettings();
            }
            var ofd = new OpenFileDialog
            {
                FileName = Path.GetFileName(storagePath),
                Filter = @"Journal Storage(*.log)|*.log|SQLite Storage(*.db,*.sqlite)|*.db;*.sqlite",
                Title = @"Set Tunny result file path",
            };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                settings.Storage.Path = ofd.FileName;
                var deStudyNameSelector = new DEStudyNameSelector(settings.Storage);
                deStudyNameSelector.ShowDialog();
            }
        }

        public void Dispose()
        {
            TLog.MethodStart();
            _tunnyHelpStripMenuItem.Dispose();
            _tutorialStripMenuItem.Dispose();
            _optunaDashboardToolStripMenuItem.Dispose();
            _pythonInstallStripMenuItem.Dispose();
            _ttDesignExplorerToolStripMenuItem.Dispose();
            _aboutTunnyStripMenuItem.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
