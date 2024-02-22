using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;

using Python.Runtime;

using Tunny.Core.Settings;
using Tunny.Core.Util;
using Tunny.UI;

namespace Tunny.Solver.HumanInTheLoop
{
    public class HumanInTheLoopBase
    {
        private protected readonly string _basePath;
        private protected readonly dynamic _artifactPath;

        public HumanInTheLoopBase(string path)
        {
            TLog.MethodStart();
            _basePath = path;
            _artifactPath = Path.Combine(_basePath, "artifacts");
        }

        public static PyModule ImportBaseLibrary()
        {
            TLog.MethodStart();
            PyModule library = Py.CreateScope();
            SetStdOutErrDirection(library);
            library.Import("os");
            library.Import("textwrap");
            library.Import("threading");
            library.Import("time");
            library.Import("optuna");
            library.Import("optuna_dashboard");
            return library;
        }

        public static void SetStdOutErrDirection(PyModule importedLibrary)
        {
            TLog.MethodStart();
            string ioPath = Path.Combine(TEnvVariables.TmpDirPath, "hitl.tmp");
            importedLibrary.Import("sys");
            importedLibrary.Exec("path = r'" + ioPath + "'");
            importedLibrary.Exec("sys.stdout = open(path, 'w', encoding='utf-8')");
            importedLibrary.Exec("sys.stderr = open(path, 'w', encoding='utf-8')");
        }

        public void WakeOptunaDashboard(Storage storage)
        {
            TLog.MethodStart();
            if (File.Exists(storage.Path) == false)
            {
                ResultFileNotExistErrorMessage();
                return;
            }

            var dashboard = new Optuna.Dashboard.Handler(
                Path.Combine(TEnvVariables.PythonPath, "Scripts", "optuna-dashboard.exe"),
                storage.Path, _artifactPath
            );
            dashboard.Run();
        }

        public static void ResultFileNotExistErrorMessage()
        {
            TLog.MethodStart();
            TunnyMessageBox.Show(
                "Please set exist result file path.",
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            );
        }

        public static int GetRunningTrialNumber(dynamic study)
        {
            TLog.MethodStart();
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
            TLog.MethodStart();
            string artifactPath = Path.Combine(_basePath, "artifacts");
            string tmpPath = TEnvVariables.TmpDirPath;
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
