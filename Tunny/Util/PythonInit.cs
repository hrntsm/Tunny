using System;

using Tunny.Handler;

namespace Tunny.Util
{
    public abstract class PythonInit
    {
        protected PythonInit()
        {
            TLog.MethodStart();
            string envPath = PythonInstaller.GetEmbeddedPythonPath() + @"\python310.dll";
            Environment.SetEnvironmentVariable("PYTHONNET_PYDLL", envPath, EnvironmentVariableTarget.Process);
        }
    }
}
