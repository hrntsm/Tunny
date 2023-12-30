using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;

using Python.Runtime;

using Tunny.Handler;
using Tunny.UI;

namespace Tunny.Solver.HumanInTheLoop
{
    public class HumanInTheLoopBase
    {
        private protected readonly string _basePath;
        private protected readonly dynamic _artifactPath;

        public HumanInTheLoopBase(string path)
        {
            _basePath = path;
            _artifactPath = Path.Combine(_basePath, "artifacts");
        }

        public PyModule ImportBaseLibrary()
        {
            PyModule library = Py.CreateScope();
            SetStdOutErrDirection(_basePath, library);
            library.Import("os");
            library.Import("textwrap");
            library.Import("threading");
            library.Import("time");
            library.Import("optuna");
            library.Import("optuna_dashboard");
            return library;
        }

        public static void SetStdOutErrDirection(string path, PyModule importedLibrary)
        {
            string ioPath = Path.Combine(path, "tmp.out");
            importedLibrary.Import("sys");
            importedLibrary.Exec("path = r'" + ioPath + "'");
            importedLibrary.Exec("sys.stdout = open(path, 'w', encoding='utf-8')");
            importedLibrary.Exec("sys.stderr = open(path, 'w', encoding='utf-8')");
        }

        public void WakeOptunaDashboard(Settings.Storage storage)
        {
            if (File.Exists(storage.Path) == false)
            {
                ResultFileNotExistErrorMessage();
                return;
            }

            CheckExistDashboardProcess();
            string artifactArgument = $"--artifact-dir \"{_artifactPath}\"";
            var dashboard = new Process();
            dashboard.StartInfo.FileName = PythonInstaller.GetEmbeddedPythonPath() + @"\Scripts\optuna-dashboard.exe";
            dashboard.StartInfo.Arguments = storage.GetOptunaStorageCommandLinePathByExtension() + " " + artifactArgument;
            dashboard.StartInfo.UseShellExecute = false;
            dashboard.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;
            dashboard.Start();

            var browser = new Process();
            browser.StartInfo.FileName = @"http://127.0.0.1:8080/dashboard";
            browser.StartInfo.UseShellExecute = true;
            browser.Start();
        }

        public static void CheckExistDashboardProcess()
        {
            Process[] dashboardProcess = Process.GetProcessesByName("optuna-dashboard");
            if (dashboardProcess.Length > 0)
            {
                foreach (Process p in dashboardProcess)
                {
                    p.Kill();
                }
            }
        }

        public static void ResultFileNotExistErrorMessage()
        {
            TunnyMessageBox.Show(
                "Please set exist result file path.",
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            );
        }

        public static int GetRunningTrialNumber(dynamic study)
        {
            PyModule lenModule = Py.CreateScope();
            var pyCode = new StringBuilder();
            pyCode.AppendLine("import optuna");
            pyCode.AppendLine("from optuna.trial import TrialState");
            pyCode.AppendLine("");
            pyCode.AppendLine("def len4cs(study):");
            pyCode.AppendLine("    running_trials = study.get_trials(deepcopy=False, states=(TrialState.RUNNING, ))");
            pyCode.AppendLine("    return len(running_trials)");
            lenModule.Exec(pyCode.ToString());

            dynamic len = lenModule.Get("len4cs");

            return len(study);
        }

        public void CheckDirectoryIsExist()
        {
            string artifactPath = Path.Combine(_basePath, "artifacts");
            string tmpPath = Path.Combine(_basePath, "tmp");
            if (!Directory.Exists(artifactPath))
            {
                Directory.CreateDirectory(artifactPath);
            }
            if (!Directory.Exists(tmpPath))
            {
                Directory.CreateDirectory(tmpPath);
            }
        }
    }
}
