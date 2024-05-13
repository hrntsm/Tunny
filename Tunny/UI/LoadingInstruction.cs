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
        private ToolStripMenuItem _pythonInstallStripMenuItem;

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
            var size = new Size(265, 30);
            _tunnyHelpStripMenuItem = new ToolStripMenuItem
            {
                Name = "TunnyHelpStripMenuItem",
                Size = size,
                Text = "Help",
            };
            _optunaDashboardToolStripMenuItem = new ToolStripMenuItem
            {
                Name = "OptunaDashboardToolStripMenuItem",
                Size = size,
                Text = "Run optuna-dashboard...",
            };
            _pythonInstallStripMenuItem = new ToolStripMenuItem
            {
                Name = "PythonInstallStripMenuItem",
                Size = size,
                Text = "Install Python...",
            };

            _tunnyHelpStripMenuItem.Click += TunnyHelpStripMenuItem_Click;
            _optunaDashboardToolStripMenuItem.Click += OptunaDashboardToolStripMenuItem_Click;
            _pythonInstallStripMenuItem.Click += PythonInstallStripMenuItem_Click;

            dropDownItems.AddRange(new ToolStripItem[] {
                _tunnyHelpStripMenuItem,
                _optunaDashboardToolStripMenuItem,
                new ToolStripSeparator(),
                _pythonInstallStripMenuItem
            });
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
