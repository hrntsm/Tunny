using System;
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
    public class Preferential
    {
        private readonly PyModule _importedLibrary;
        private readonly dynamic _artifactBackend;
        private readonly dynamic _artifactPath;
        private readonly string _basePath;
        private string _userAttrKey;

        public Preferential(string path)
        {
            PyModule importedLibrary = Py.CreateScope();
            SetStdOutErrDirection(path, importedLibrary);
            importedLibrary.Import("os");
            importedLibrary.Import("textwrap");
            importedLibrary.Import("threading");
            importedLibrary.Import("time");
            importedLibrary.Import("optuna");
            importedLibrary.Import("optuna_dashboard");
            importedLibrary.Exec("from optuna_dashboard.artifact.file_system import FileSystemBackend");
            importedLibrary.Exec("from optuna_dashboard.artifact import upload_artifact");
            importedLibrary.Exec("from optuna_dashboard import register_preference_feedback_component");
            importedLibrary.Exec("from optuna_dashboard.preferential import create_study");
            importedLibrary.Exec("from optuna_dashboard.preferential.samplers.gp import PreferentialGPSampler");

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

        public dynamic CreateStudy(int nGenerate, string studyName, dynamic storage, string objectiveName)
        {
            dynamic createStudy = _importedLibrary.Get("create_study");
            dynamic preferentialGPSampler = _importedLibrary.Get("PreferentialGPSampler");
            string name = studyName == null || studyName == "" ? "no-name-" + Guid.NewGuid().ToString("D") : studyName;
            dynamic study = createStudy(
                n_generate: nGenerate,
                study_name: name,
                sampler: preferentialGPSampler(),
                storage: storage
            );
            dynamic registerPreferenceFeedbackComponent = _importedLibrary.Get("register_preference_feedback_component");
            _userAttrKey = objectiveName;
            registerPreferenceFeedbackComponent(study, "artifact", objectiveName);
            return study;
        }

        public void UploadArtifact(dynamic study, dynamic trial, Bitmap image)
        {
            dynamic uploadArtifact = _importedLibrary.Get("upload_artifact");
            CheckDirectoryIsExist();
            string path = $"{_basePath}/tmp/image_{trial.number}.png";
            image.Save(path, System.Drawing.Imaging.ImageFormat.Png);
            dynamic artifactId = uploadArtifact(_artifactBackend, trial, path);
            trial.set_user_attr(_userAttrKey, artifactId);
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
