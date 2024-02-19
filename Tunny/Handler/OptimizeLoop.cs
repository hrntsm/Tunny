using System;
using System.Collections.Generic;
using System.ComponentModel;

using Tunny.Component.Optimizer;
using Tunny.Core.Enum;
using Tunny.Core.Handler;
using Tunny.Core.Input;
using Tunny.Core.Settings;
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
        public static TunnySettings Settings;
        public static bool IsForcedStopOptimize { get; set; }

        internal static void RunMultiple(object sender, DoWorkEventArgs e)
        {
            TLog.MethodStart();
            s_worker = sender as BackgroundWorker;
            Component = e.Argument as OptimizeComponentBase;
            Component?.GhInOutInstantiate();

            Parameter[] optimalParams = RunOptimizationLoop(s_worker);
            if (optimalParams == null || optimalParams.Length == 0)
            {
                return;
            }
            var pState = new ProgressState
            {
                Parameter = optimalParams
            };

            if (Component != null)
            {
                Component.GrasshopperStatus = GrasshopperStates.RequestSent;
                s_worker.ReportProgress(100, pState);
                while (Component.GrasshopperStatus != GrasshopperStates.RequestProcessed)
                { /* just wait until the cows come home */ }
            }

            s_worker?.CancelAsync();
        }

        private static Parameter[] RunOptimizationLoop(BackgroundWorker worker)
        {
            TLog.MethodStart();
            List<VariableBase> variables = Component.GhInOut.Variables;
            Objective objectives = Component.GhInOut.Objectives;
            Dictionary<string, FishEgg> enqueueItems = Component.GhInOut.EnqueueItems;
            bool hasConstraint = Component.GhInOut.HasConstraint;

            if (worker.CancellationPending)
            {
                return Array.Empty<Parameter>();
            }

            var optunaSolver = new Solver.Optuna(Settings, hasConstraint);
            bool solverStarted = optunaSolver.RunSolver(variables, objectives, enqueueItems, EvaluateFunction);

            return solverStarted ? optunaSolver.OptimalParameters : Array.Empty<Parameter>();
        }

        private static TrialGrasshopperItems EvaluateFunction(ProgressState pState, int progress)
        {
            TLog.MethodStart();
            Component.GrasshopperStatus = GrasshopperStates.RequestSent;

            s_worker.ReportProgress(progress, pState);
            while (Component.GrasshopperStatus != GrasshopperStates.RequestProcessed)
            { /*just wait*/ }

            return new TrialGrasshopperItems
            {
                Objectives = Component.GhInOut.Objectives,
                GeometryJson = Component.GhInOut.GetGeometryJson(),
                Attribute = Component.GhInOut.GetAttributes(),
                Artifacts = Component.GhInOut.Artifacts,
            };
        }
    }
}
