using System;
using System.IO;

namespace Optuna.Artifacts
{
    public class Artifacts
    {
        private dynamic _artifactStore;

        public void CreateFileSystemArtifactStore(dynamic optuna, string backendPath)
        {
            if (string.IsNullOrEmpty(backendPath))
            {
                throw new ArgumentException("backendPath is null or empty");
            }
            if (!Directory.Exists(backendPath))
            {
                Directory.CreateDirectory(backendPath);
            }
            _artifactStore = optuna.artifacts.FileSystemArtifactStore(base_path: backendPath);
        }

        public string UploadArtifact(dynamic optuna, dynamic trial, string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentException("filePath is null or empty");
            }
            if (!File.Exists(filePath))
            {
                throw new ArgumentException("filePath does not exist");
            }
            if (_artifactStore == null)
            {
                throw new ArgumentException("artifactStore is null. please call CreateFileSystemArtifactStore first.");
            }
            return optuna.artifacts.upload_artifact(trial, filePath, _artifactStore);
        }
    }
}
