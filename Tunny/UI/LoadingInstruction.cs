using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

using Grasshopper.GUI;
using Grasshopper.GUI.Canvas;
using Grasshopper.Kernel;

using Serilog;

using Tunny.Resources;
using Tunny.Settings;
using Tunny.Util;

namespace Tunny.UI
{
    public class LoadingInstruction : GH_AssemblyPriority, IDisposable
    {
        private ToolStripMenuItem _optunaDashboardToolStripMenuItem;

        public override GH_LoadingInstruction PriorityLoad()
        {
            Grasshopper.Instances.ComponentServer.AddCategoryIcon("Tunny", Resource.TunnyIcon);
            Grasshopper.Instances.ComponentServer.AddCategorySymbolName("Tunny", 'T');
            Grasshopper.Instances.CanvasCreated += RegisterTunnyMenuItems;
            InitializeLogger();
            return GH_LoadingInstruction.Proceed;
        }

        private static void InitializeLogger()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.Console(formatProvider: CultureInfo.InvariantCulture)
                .WriteTo.File(path: TunnyVariables.LogPath + "/log_.txt", rollingInterval: RollingInterval.Day, formatProvider: CultureInfo.InvariantCulture)
                .CreateLogger();
            Log.Information("Tunny is loaded.");
            CheckAndDeleteOldLogFiles();
        }

        private static void CheckAndDeleteOldLogFiles()
        {
            string logDirectory = TunnyVariables.LogPath;
            string logFilePattern = "*.txt";

            DateTime threshold = DateTime.Now.AddDays(-7);

            var directory = new DirectoryInfo(logDirectory);
            FileInfo[] logFiles = directory.GetFiles(logFilePattern);
            foreach (FileInfo file in logFiles)
            {
                if (file.LastWriteTime < threshold)
                {
                    file.Delete();
                }
            }
        }

        void RegisterTunnyMenuItems(GH_Canvas canvas)
        {
            Grasshopper.Instances.CanvasCreated -= RegisterTunnyMenuItems;

            GH_DocumentEditor docEditor = Grasshopper.Instances.DocumentEditor;
            if (docEditor != null)
            {
                SetupTunnyMenu(docEditor);
            }
        }
        private void SetupTunnyMenu(GH_DocumentEditor docEditor)
        {
            ToolStripMenuItem tunnyToolStripMenuItem;
            tunnyToolStripMenuItem = new ToolStripMenuItem();

            docEditor.MainMenuStrip.SuspendLayout();

            docEditor.MainMenuStrip.Items.AddRange(new ToolStripItem[] {
                tunnyToolStripMenuItem
            });

            tunnyToolStripMenuItem.DropDownItems.AddRange(TunnyMenuItems.ToArray());
            tunnyToolStripMenuItem.Name = "TunnyToolStripMenuItem";
            tunnyToolStripMenuItem.Size = new Size(125, 29);
            tunnyToolStripMenuItem.Text = "Tunny";

            docEditor.MainMenuStrip.ResumeLayout(false);
            docEditor.MainMenuStrip.PerformLayout();

            GH_DocumentEditor.AggregateShortcutMenuItems += GH_DocumentEditor_AggregateShortcutMenuItems;
        }

        void GH_DocumentEditor_AggregateShortcutMenuItems(object sender, GH_MenuShortcutEventArgs e)
        {
            e.AppendItem(_optunaDashboardToolStripMenuItem);
        }

        private List<ToolStripMenuItem> TunnyMenuItems
        {
            get
            {
                var list = new List<ToolStripMenuItem>();

                _optunaDashboardToolStripMenuItem = new ToolStripMenuItem
                {
                    Name = "OptunaDashboardToolStripMenuItem",
                    Size = new Size(265, 30),
                    Text = "Run optuna-dashboard",
                };
                _optunaDashboardToolStripMenuItem.Click += OptunaDashboardToolStripMenuItem_Click;

                list.Add(_optunaDashboardToolStripMenuItem);
                return list;
            }
        }

        private void OptunaDashboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string pythonDirectory = Path.Combine(TunnyVariables.TunnyEnvPath, "python");
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
            string settingsPath = TunnyVariables.OptimizeSettingsPath;
            string storagePath = string.Empty;
            if (File.Exists(settingsPath))
            {
                var settings = TunnySettings.Deserialize(File.ReadAllText(settingsPath));
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
            _optunaDashboardToolStripMenuItem.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
