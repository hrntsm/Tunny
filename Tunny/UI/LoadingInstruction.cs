using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
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
        private ToolStripMenuItem _tunnyHelpStripMenuItem;
        private ToolStripMenuItem _optunaDashboardToolStripMenuItem;
        private ToolStripMenuItem _ttDesignExplorerToolStripMenuItem;
        private ToolStripMenuItem _pythonInstallStripMenuItem;
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
            tunnyToolStripMenuItem.Size = new Size(125, 29);
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
            _tunnyHelpStripMenuItem = new ToolStripMenuItem("Help", null, TunnyHelpStripMenuItem_Click, "TunnyHelpStripMenuItem");
            _optunaDashboardToolStripMenuItem = new ToolStripMenuItem("Run optuna-dashboard...", Resource.optuna_dashboard, OptunaDashboardToolStripMenuItem_Click, "OptunaDashboardToolStripMenuItem");
            _ttDesignExplorerToolStripMenuItem = new ToolStripMenuItem("Run TT DesignExplorer...", Resource.TTDesignExplorer, TTDesignExplorerToolStripMenuItem_Click, "TTDesignExplorerToolStripMenuItem");
            _pythonInstallStripMenuItem = new ToolStripMenuItem("Install Python...", null, PythonInstallStripMenuItem_Click, "PythonInstallStripMenuItem");
            _aboutTunnyStripMenuItem = new ToolStripMenuItem("About...", Resource.TunnyIcon, AboutTunnyStripMenuItem_Click, "AboutTunnyStripMenuItem");

            dropDownItems.AddRange(new ToolStripItem[] {
                _tunnyHelpStripMenuItem,
                _optunaDashboardToolStripMenuItem,
                _ttDesignExplorerToolStripMenuItem,
                new ToolStripSeparator(),
                _pythonInstallStripMenuItem,
                new ToolStripSeparator(),
                _aboutTunnyStripMenuItem
            });
        }

        private void AboutTunnyStripMenuItem_Click(object sender, EventArgs e)
        {
            TLog.MethodStart();
            TLog.Debug("AboutTunnyStripMenuItem Clicked");
            TunnyMessageBox.Show(
                "Tunny\nVersion: " + TEnvVariables.Version + "\n\n🐟Tunny🐟 is Grasshopper's optimization component using Optuna, an open source hyperparameter auto-optimization framework.\n\nTunny is developed by hrntsm.\nFor more information, visit https://tunny-docs.deno.dev/",
                "About Tunny",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void PythonInstallStripMenuItem_Click(object sender, EventArgs e)
        {
            TLog.MethodStart();
            TLog.Debug("PythonInstallStripMenuItem Clicked");
            if (Directory.Exists(TEnvVariables.PythonPath))
            {
                DialogResult result = TunnyMessageBox.Show(
                    "It appears that the Tunny Python environment is already installed.\nWould you like to reinstall it?",
                    "Python is already installed",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Information);
                if (result == DialogResult.Cancel)
                {
                    TLog.Info("From menu item Python installation canceled by user.");
                    return;
                }
            }
            TLog.Info("From menu item Python installation started.");
            var pythonInstallDialog = new PythonInstallDialog();
            pythonInstallDialog.ShowDialog();
        }

        private void TunnyHelpStripMenuItem_Click(object sender, EventArgs e)
        {
            TLog.MethodStart();
            TLog.Debug("TunnyHelpStripMenuItem Clicked");
            var browser = new Process();
            browser.StartInfo.FileName = $@"https://tunny-docs.deno.dev/";
            browser.StartInfo.UseShellExecute = true;
            browser.Start();
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
                                     MessageBoxButtons.OK,
                                     MessageBoxIcon.Error);
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
                var settings = TSettings.Deserialize(File.ReadAllText(settingsPath));
                storagePath = settings.Storage.Path;
            }
            var ofd = new OpenFileDialog
            {
                FileName = Path.GetFileName(storagePath),
                Filter = @"Journal Storage(*.log)|*.log|SQLite Storage(*.db,*.sqlite)|*.db;*.sqlite",
                Title = @"Set Tunny result file path",
            };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                var dashboard = new Optuna.Dashboard.Handler(dashboardPath, ofd.FileName);
                dashboard.Run();
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
                settings = TSettings.Deserialize(File.ReadAllText(settingsPath));
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
            _optunaDashboardToolStripMenuItem.Dispose();
            _pythonInstallStripMenuItem.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
