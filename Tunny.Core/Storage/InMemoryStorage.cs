using Python.Runtime;

using Tunny.Core.Util;

namespace Tunny.Core.Storage
{
    public class InMemoryStorage : PythonInit, ICreateStorage
    {
        public dynamic Storage { get; set; }

        public dynamic CreateNewStorage(bool useInnerPythonEngine, Settings.Storage storageSetting)
        {
            TLog.MethodStart();
            if (useInnerPythonEngine)
            {
                PythonEngine.Initialize();
                using (Py.GIL())
                {
                    CreateStorageProcess();
                }
                PythonEngine.Shutdown();
            }
            else
            {
                CreateStorageProcess();
            }

            return Storage;
        }

        private void CreateStorageProcess()
        {
            TLog.MethodStart();
            dynamic optuna = Py.Import("optuna");
            Storage = optuna.storages.InMemoryStorage();
        }
    }
}
