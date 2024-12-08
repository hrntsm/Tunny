using System;
using System.Collections.Generic;

using Grasshopper.GUI;

using Optuna.Study;

using Tunny.Component.Optimizer;
using Tunny.Core.Handler;
using Tunny.Core.Settings;
using Tunny.Core.Util;
using Tunny.WPF.Models;
using Tunny.WPF.ViewModels.Optimize;

namespace Tunny.WPF.Common
{
    internal sealed class SharedItems
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
        internal StudySummary[] StudySummaries { get; set; }
        internal Dictionary<int, List<OutputTrialItem>> OutputTrialDict { get; set; } = new Dictionary<int, List<OutputTrialItem>>();

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

        private void ClearProgress()
        {
            TLog.MethodStart();
            _progress = null;
        }

        internal void Clear()
        {
            TLog.MethodStart();
            Component = null;
            Settings = null;
            GH_DocumentEditor = null;
            TunnyWindow = null;
            OptimizeViewModel = null;
            StudySummaries = null;
            OutputTrialDict.Clear();
            ClearProgress();
        }
    }
}
