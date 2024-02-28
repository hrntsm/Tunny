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
        private protected readonly string _tmpPath;
        private protected readonly dynamic _artifactPath;

        public HumanInTheLoopBase(string tmpPath, string storagePath)
        {
            TLog.MethodStart();
            _tmpPath = tmpPath;
            _artifactPath = Path.Combine(Path.GetDirectoryName(storagePath), "artifacts");
        }

        public PyModule ImportBaseLibrary()
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

        public void SetStdOutErrDirection(PyModule importedLibrary)
        {
            TLog.MethodStart();
            string ioPath = Path.Combine(_tmpPath, "hitl.tmp");
            importedLibrary.Import("sys");
            importedLibrary.Exec("path = r'" + ioPath + "'");
            importedLibrary.Exec("sys.stdout = open(path, 'w', encoding='utf-8')");
            importedLibrary.Exec("sys.stderr = open(path, 'w', encoding='utf-8')");
        }

        public static void WakeOptunaDashboard(Storage storage)
        {
            TLog.MethodStart();
            if (File.Exists(storage.Path) == false)
            {
                ResultFileNotExistErrorMessage();
                return;
            }

            var dashboard = new Optuna.Dashboard.Handler(
                Path.Combine(TEnvVariables.PythonPath, "Scripts", "optuna-dashboard.exe"),
                storage.Path
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
            if (!Directory.Exists(_artifactPath))
            {
                Directory.CreateDirectory(_artifactPath);
            }
            if (!Directory.Exists(_tmpPath))
            {
                Directory.CreateDirectory(_tmpPath);
            }
        }
    }
}
