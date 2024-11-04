using Python.Runtime;

using Tunny.Core.Util;

namespace Tunny.Core.Storage
{
    public class InMemoryStorage : PythonInit, ICreateTStorage
    {
        public dynamic Storage { get; set; }

        public dynamic CreateNewTStorage(bool useInnerPythonEngine, Settings.Storage storageSetting)
        {
            TLog.MethodStart();
            if (useInnerPythonEngine)
            {
                InitializePythonEngine();
                using (Py.GIL())
                {
                    CreateTStorageProcess();
                }
                PythonEngine.Shutdown();
            }
            else
            {
                CreateTStorageProcess();
            }

            return Storage;
        }

        private void CreateTStorageProcess()
        {
            TLog.MethodStart();
            dynamic optuna = Py.Import("optuna");
            Storage = optuna.storages.InMemoryStorage();
        }
    }
}
