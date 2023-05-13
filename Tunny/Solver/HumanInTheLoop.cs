using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

using Python.Runtime;

using Tunny.Handler;
using Tunny.UI;

namespace Tunny.Solver
{
    public class HumanInTheLoop
    {
        private readonly PyModule _importedLibrary;
        private readonly dynamic _artifactBackend;
        private readonly dynamic _artifactPath;
        private readonly string _basePath;

        public HumanInTheLoop(string path)
        {
            PyModule importedLibrary = Py.CreateScope();
            importedLibrary.Exec(@"
import sys
path = 'C:/Users/hiroa/Desktop/temp_stdio.txt'
sys.stdout = open(path, 'w', encoding='utf-8')
sys.stderr = open(path, 'w', encoding='utf-8')"
            );
            importedLibrary.Import("os");
            importedLibrary.Import("textwrap");
            importedLibrary.Import("threading");
            importedLibrary.Import("time");
            importedLibrary.Import("optuna");
            importedLibrary.Import("optuna_dashboard");
            importedLibrary.Exec("from optuna_dashboard import save_note");
            importedLibrary.Exec("from optuna_dashboard import ObjectiveChoiceWidget");
            importedLibrary.Exec("from optuna_dashboard import ObjectiveSliderWidget");
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

        public static dynamic FixStudyCachedStorage(dynamic study)
        {
            PyModule def = Py.CreateScope();
            def.Exec(@"
import optuna
def fix_cached_storage(study):
    if isinstance(study._storage, optuna.storages._CachedStorage):
        study._storage = study._storage._backend"
        );
            dynamic fixCachedStorage = def.Get("fix_cached_storage");
            fixCachedStorage(study);
            return study;
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
            browser.StartInfo.FileName = @"http://127.0.0.1:8080/dashboard/beta";
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
            pyNameList.Append(new PyString("Check above image: How do you like the look of this model?"));
            foreach (string objectiveName in objectiveNames)
            {
                pyNameList.Append(new PyString(objectiveName));
            }
            setObjectiveNames(study, pyNameList);
        }

        public void SetWidgets(dynamic study, string[] objectiveNames)
        {
            dynamic registerObjectiveFromWidgets = _importedLibrary.Get("register_objective_form_widgets");
            dynamic objectiveSliderWidget = _importedLibrary.Get("ObjectiveSliderWidget");
            dynamic objectiveUserAttrRef = _importedLibrary.Get("ObjectiveUserAttrRef");

            var widgets = new dynamic[objectiveNames.Length + 1];
            widgets[0] = objectiveSliderWidget(min: 1, max: 10, step: 1, description: "Smaller is better");
            for (int i = 1; i < objectiveNames.Length + 1; i++)
            {
                string objectiveName = objectiveNames[i - 1];
                var key = new PyString("result_" + objectiveName);
                widgets[i] = objectiveUserAttrRef(key: key);
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
