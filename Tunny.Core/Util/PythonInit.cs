using System;
using System.IO;

namespace Tunny.Core.Util
{
    public abstract class PythonInit
    {
        protected PythonInit()
        {
            TLog.MethodStart();
            string envPath = Path.Combine(TEnvVariables.TunnyEnvPath, "python", @"python310.dll");
            Environment.SetEnvironmentVariable("PYTHONNET_PYDLL", envPath, EnvironmentVariableTarget.Process);
        }
    }
}
