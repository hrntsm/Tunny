using System;

using Grasshopper.GUI;

using Tunny.Component.Optimizer;
using Tunny.Core.Handler;
using Tunny.Core.Settings;
using Tunny.Core.Util;
using Tunny.WPF.ViewModels.Optimize;

namespace Tunny.WPF.Common
{
    internal class SharedItems
    {
        private static SharedItems s_instance;
        internal static SharedItems Instance => s_instance ?? (s_instance = new SharedItems());

        private SharedItems()
        {
        }

        internal OptimizeComponentBase Component { get; set; }
        internal TSettings Settings { get; set; }
        internal GH_DocumentEditor GH_DocumentEditor { get; set; }
        internal MainWindow TunnyWindow { get; set; }
        internal OptimizeViewModel OptimizeViewModel { get; set; }

        private IProgress<ProgressState> _progress;

        internal void AddProgress(IProgress<ProgressState> progress)
        {
            TLog.MethodStart();
            _progress = progress;
        }

        internal void ReportProgress(ProgressState progressState)
        {
            TLog.MethodStart();
            _progress?.Report(progressState);
        }
    }
}
