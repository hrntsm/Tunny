using System;

using CefSharp;

using Grasshopper.Kernel;

using Tunny.Core.Util;

namespace Tunny.Component.LoadingInstruction
{
    public class InitializeCef_Tunny : GH_AssemblyPriority
    {
        public override GH_LoadingInstruction PriorityLoad()
        {
            InitializeCefRuntimeResolver();

            return GH_LoadingInstruction.Proceed;
        }

        private static void InitializeCefRuntimeResolver()
        {
            try
            {
                CefRuntime.SubscribeAnyCpuAssemblyResolver(TEnvVariables.ComponentFolder);
            }
            catch (Exception e)
            {
                TLog.Error($"CefSharp Assembly Resolver error: {e.Message}: {e.StackTrace}");
            }
        }
    }
}
