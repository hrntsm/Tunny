using Python.Runtime;

namespace Tunny.Core.Util
{
    public abstract class PythonInit
    {
        protected static void InitializePythonEngine()
        {
            TLog.MethodStart();
            TLog.Debug("Check PythonEngine status.");
            if (PythonEngine.IsInitialized)
            {
                PythonEngine.Shutdown();
                TLog.Warning("PythonEngine is unintentionally initialized and therefore shut it down.");
            }
            PythonEngine.Initialize();
            TLog.Debug("Initialize PythonEngine.");
        }
    }
}
