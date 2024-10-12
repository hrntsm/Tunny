using System;
using System.Collections.Generic;
using System.ComponentModel;

using Tunny.Component.Optimizer;
using Tunny.Core.Handler;
using Tunny.Core.Input;
using Tunny.Core.Settings;
using Tunny.Core.TEnum;
using Tunny.Core.Util;
using Tunny.Input;
using Tunny.PostProcess;
using Tunny.Type;

namespace Tunny.Handler
{
    internal static class OptimizeLoop
    {
        private static BackgroundWorker s_worker;
        public static OptimizeComponentBase Component;
        public static TSettings Settings;
        public static bool IsForcedStopOptimize { get; set; }

        internal static void RunMultiple(object sender, DoWorkEventArgs e)
        {
            TLog.MethodStart();
            s_worker = sender as BackgroundWorker;
            Component = e.Argument as OptimizeComponentBase;
            Component?.GhInOutInstantiate();

            ProgressState progressState = RunOptimizationLoop(s_worker);
            if (progressState.Parameter == null || progressState.Parameter.Count == 0)
            {
                return;
            }

            if (Component != null)
            {
                Component.GrasshopperStatus = GrasshopperStates.RequestSent;
                s_worker.ReportProgress(100, progressState);
                while (Component.GrasshopperStatus != GrasshopperStates.RequestProcessed)
                { /* just wait until the cows come home */ }
            }

            s_worker?.CancelAsync();
        }

        private static ProgressState RunOptimizationLoop(BackgroundWorker worker)
        {
            TLog.MethodStart();
            List<VariableBase> variables = Component.GhInOut.Variables;
            Objective objectives = Component.GhInOut.Objectives;
            Dictionary<string, FishEgg> enqueueItems = Component.GhInOut.EnqueueItems;
            bool hasConstraint = Component.GhInOut.HasConstraint;
            var progressState = new ProgressState(Array.Empty<Parameter>());

            if (worker.CancellationPending)
            {
                return progressState;
            }

            var optunaSolver = new Solver.Solver(Settings, hasConstraint);
            bool reinstateResult = optunaSolver.RunSolver(variables, objectives, enqueueItems, EvaluateFunction);

            return reinstateResult
                ? new ProgressState(optunaSolver.OptimalParameters)
                : new ProgressState(optunaSolver.OptimalParameters, true);
        }

        private static TrialGrasshopperItems EvaluateFunction(ProgressState pState, int progress)
        {
            TLog.MethodStart();
            s_worker.ReportProgress(progress, pState);
            if (pState.IsReportOnly)
            {
                return null;
            }

            Component.GrasshopperStatus = GrasshopperStates.RequestSent;

            int j = 0;
            while (Component.GrasshopperStatus != GrasshopperStates.RequestProcessed)
            {
                if (DateTime.Now.Second % 10 == 0 && DateTime.Now.Millisecond < 10)
                {
                    ReportPruner(pState.OptunaTrial, j++, pState.Pruner);
                }
            }

            return new TrialGrasshopperItems
            {
                Objectives = Component.GhInOut.Objectives,
                GeometryJson = Component.GhInOut.GetGeometryJson(),
                Attribute = Component.GhInOut.GetAttributes(),
                Artifacts = Component.GhInOut.Artifacts,
            };
        }

        private static void ReportPruner(dynamic optunaTrial, int step, Pruner pruner)
        {
            PrunerReport report = pruner.Evaluate();
            optunaTrial.report(report.Value, step);

            if (optunaTrial.should_prune())
            {
                pruner.Stop();
            }
        }
    }
}
