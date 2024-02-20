using System;
using System.Drawing;

using Python.Runtime;

using Tunny.Core.Util;

namespace Tunny.Solver.HumanInTheLoop
{
    public class Preferential : HumanInTheLoopBase
    {
        private readonly PyModule _importedLibrary;
        private readonly dynamic _artifactBackend;
        private string _userAttrKey;

        public Preferential(string path) : base(path)
        {
            TLog.MethodStart();
            PyModule importedLibrary = ImportBaseLibrary();
            importedLibrary.Exec("from optuna_dashboard.artifact.file_system import FileSystemBackend");
            importedLibrary.Exec("from optuna_dashboard.artifact import upload_artifact");
            importedLibrary.Exec("from optuna_dashboard import register_preference_feedback_component");
            importedLibrary.Exec("from optuna_dashboard.preferential import create_study");
            importedLibrary.Exec("from optuna_dashboard.preferential.samplers.gp import PreferentialGPSampler");

            dynamic fileSystemBackend = importedLibrary.Get("FileSystemBackend");
            _artifactBackend = fileSystemBackend(base_path: _artifactPath);
            _importedLibrary = importedLibrary;
        }

        public dynamic CreateStudy(int nGenerate, string studyName, dynamic storage, string objectiveName)
        {
            TLog.MethodStart();
            dynamic createStudy = _importedLibrary.Get("create_study");
            dynamic preferentialGPSampler = _importedLibrary.Get("PreferentialGPSampler");
            string name = studyName == null || studyName == "" ? "no-name-" + Guid.NewGuid().ToString("D") : studyName;
            dynamic study = createStudy(
                n_generate: nGenerate,
                study_name: name,
                sampler: preferentialGPSampler(),
                storage: storage,
                load_if_exists: true
            );
            dynamic registerPreferenceFeedbackComponent = _importedLibrary.Get("register_preference_feedback_component");
            _userAttrKey = objectiveName;
            registerPreferenceFeedbackComponent(study, "artifact", objectiveName);
            return study;
        }

        public void UploadArtifact(dynamic trial, Bitmap image)
        {
            TLog.MethodStart();
            dynamic uploadArtifact = _importedLibrary.Get("upload_artifact");
            CheckDirectoryIsExist();
            string path = $"{_basePath}/tmp/image_{trial.number}.png";
            image.Save(path, System.Drawing.Imaging.ImageFormat.Png);
            dynamic artifactId = uploadArtifact(_artifactBackend, trial, path);
            trial.set_user_attr(_userAttrKey, artifactId);
        }
    }
}
