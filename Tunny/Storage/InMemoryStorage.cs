using Python.Runtime;

using Tunny.Util;

namespace Tunny.Storage
{
    public class InMemoryStorage : PythonInit, ICreateStorage
    {
        public dynamic Storage { get; set; }

        public InMemoryStorage()
        {
        }

        public dynamic CreateNewStorage(bool useInnerPythonEngine, string storagePath)
        {
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
            dynamic optuna = Py.Import("optuna");
            Storage = optuna.storages.InMemoryStorage();
        }
    }
}
