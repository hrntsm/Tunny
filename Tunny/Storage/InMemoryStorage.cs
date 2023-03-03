using Python.Runtime;

using Tunny.Util;

namespace Tunny.Storage
{
    public class InMemoryStorage : PythonInit, ICreateStorage
    {
        public dynamic Storage { get; set; }

        public InMemoryStorage() : base()
        {
        }

        public dynamic CreateNewStorage(string storagePath = null)
        {
            PythonEngine.Initialize();
            using (Py.GIL())
            {
                dynamic optuna = Py.Import("optuna");
                Storage = optuna.storages.InMemoryStorage();
            }
            PythonEngine.Shutdown();

            return Storage;
        }
    }
}
