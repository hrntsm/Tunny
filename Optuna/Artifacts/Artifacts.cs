using Python.Runtime;

namespace Optuna.Artifacts
{
    public class Artifacts
    {
        public static string UploadArtifact(dynamic trial, string filePath, dynamic artifactStore)
        {
            dynamic optuna = Py.Import("optuna");
            return optuna.artifacts.upload_artifact(trial, filePath, artifactStore);
        }
    }
}
