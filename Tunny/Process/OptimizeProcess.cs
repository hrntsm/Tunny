using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Tunny.Component.Optimizer;
using Tunny.Core.Settings;
using Tunny.Core.Util;

namespace Tunny.Process
{
    internal static class OptimizeProcess
    {
        public static OptimizeComponentBase Component;
        public static TSettings Settings;
        public static bool IsForcedStopOptimize { get; set; }

        private static IProgress<int> s_progress;

        internal static void AddProgressBar(IProgress<int> progress)
        {
            TLog.MethodStart();
            s_progress = progress;
        }

        internal async static Task RunMultipleAsync()
        {
            TLog.MethodStart();
        }
    }
}
