using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

using Python.Runtime;

using Tunny.Handler;
using Tunny.UI;

namespace Tunny.Solver.HumanInTheLoop
{
    public class Slider
    {
        private readonly PyModule _importedLibrary;
        private readonly dynamic _artifactBackend;
        private readonly dynamic _artifactPath;
        private readonly string _basePath;

        public Slider(string path)
        {
            PyModule importedLibrary = Py.CreateScope();
            SetStdOutErrDirection(path, importedLibrary);
            importedLibrary.Import("os");
            importedLibrary.Import("textwrap");
            importedLibrary.Import("threading");
            importedLibrary.Import("time");
            importedLibrary.Import("optuna");
            importedLibrary.Import("optuna_dashboard");
            importedLibrary.Exec("from optuna_dashboard import save_note");
            importedLibrary.Exec("from optuna_dashboard import SliderWidget");
            importedLibrary.Exec("from optuna_dashboard import ObjectiveUserAttrRef");
            importedLibrary.Exec("from optuna_dashboard import register_objective_form_widgets");
            importedLibrary.Exec("from optuna_dashboard import set_objective_names");
            importedLibrary.Exec("from optuna_dashboard.artifact import upload_artifact");
            importedLibrary.Exec("from optuna_dashboard.artifact.file_system import FileSystemBackend");

            _basePath = path;
            _artifactPath = Path.Combine(_basePath, "artifacts");
            dynamic fileSystemBackend = importedLibrary.Get("FileSystemBackend");
            _artifactBackend = fileSystemBackend(base_path: _artifactPath);
            _importedLibrary = importedLibrary;
        }

        private static void SetStdOutErrDirection(string path, PyModule importedLibrary)
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

        private static void CheckExistDashboardProcess()
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

        private static void ResultFileNotExistErrorMessage()
        {
            TunnyMessageBox.Show(
                "Please set exist result file path.",
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            );
        }

        public void SetObjective(dynamic study, string[] objectiveNames)
        {
            dynamic setObjectiveNames = _importedLibrary.Get("set_objective_names");
            var pyNameList = new PyList();
            foreach (string objectiveName in objectiveNames)
            {
                pyNameList.Append(new PyString(objectiveName.Replace("Human-in-the-Loop", "HITL")));
            }
            setObjectiveNames(study, pyNameList);
        }

        public void SetWidgets(dynamic study, string[] objectiveNames)
        {
            dynamic registerObjectiveFromWidgets = _importedLibrary.Get("register_objective_form_widgets");
            dynamic sliderWidget = _importedLibrary.Get("SliderWidget");
            dynamic objectiveUserAttrRef = _importedLibrary.Get("ObjectiveUserAttrRef");

            var widgets = new dynamic[objectiveNames.Length];
            for (int i = 0; i < objectiveNames.Length; i++)
            {
                if (objectiveNames[i].Contains("Human-in-the-Loop"))
                {
                    widgets[i] = sliderWidget(min: 1, max: 5, step: 1, description: "Smaller is better");
                }
                else
                {
                    string objectiveName = objectiveNames[i];
                    var key = new PyString("result_" + objectiveName);
                    widgets[i] = objectiveUserAttrRef(key: key);
                }
            }
            registerObjectiveFromWidgets(study, widgets: widgets);
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

        public void SaveNote(dynamic study, dynamic trial, Bitmap[] bitmaps)
        {
            dynamic uploadArtifact = _importedLibrary.Get("upload_artifact");
            var noteText = new StringBuilder();
            noteText.AppendLine("# Image");
            noteText.AppendLine("");

            CheckDirectoryIsExist();
            for (int i = 0; i < bitmaps.Length; i++)
            {
                Bitmap bitmap = bitmaps[i];
                string path = $"{_basePath}/tmp/image_{study._study_id}_{trial._trial_id}.png";
                bitmap?.Save(path, System.Drawing.Imaging.ImageFormat.Png);
                dynamic artifactId = uploadArtifact(_artifactBackend, trial, path);
                noteText.AppendLine($"![](/artifacts/{study._study_id}/{trial._trial_id}/{artifactId})");
            }

            dynamic textWrap = _importedLibrary.Get("textwrap");
            dynamic note = textWrap.dedent(noteText.ToString());
            dynamic saveNote = _importedLibrary.Get("save_note");
            saveNote(trial, note);
        }

        private void CheckDirectoryIsExist()
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
