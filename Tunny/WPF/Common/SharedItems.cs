using System;
using System.Collections.Generic;
using System.Linq;

using Grasshopper.GUI;

using Optuna.Study;
using Optuna.Trial;

using Tunny.Component.Optimizer;
using Tunny.Core.Handler;
using Tunny.Core.Settings;
using Tunny.Core.Solver;
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
        internal Dictionary<int, Trial[]> Trials { get; set; }
        private StudySummary[] _studySummaries;
        internal StudySummary[] StudySummaries
        {
            get => _studySummaries;
            set
            {
                TLog.MethodStart();
                if (value == null)
                {
                    return;
                }
                _studySummaries = value;
                var output = new Output(Settings.Storage.Path);
                Trials = output.GetAllTrial();
            }
        }
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

        internal OutputTrialItem GetOutputTrial(int studyId, int trialId)
        {
            return new OutputTrialItem
            {
                Id = trialId,
                IsSelected = false,
                Objectives = string.Join(", ", Trials[studyId][trialId].Values),
                Variables = string.Join(", ", Trials[studyId][trialId].Params.Select(p => $"{p.Key}:{p.Value}")),
            };
        }
    }
}
