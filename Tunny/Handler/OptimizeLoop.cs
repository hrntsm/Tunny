using System;
using System.Collections.Generic;
using System.ComponentModel;

using Tunny.Component.Optimizer;
using Tunny.Enum;
using Tunny.Input;
using Tunny.PostProcess;
using Tunny.Settings;
using Tunny.Type;
using Tunny.Util;

namespace Tunny.Handler
{
    internal static class OptimizeLoop
    {
        private static BackgroundWorker s_worker;
        private static FishingComponent s_component;
        public static TunnySettings Settings;
        public static bool IsForcedStopOptimize { get; set; }

        internal static void RunMultiple(object sender, DoWorkEventArgs e)
        {
            TLog.MethodStart();
            s_worker = sender as BackgroundWorker;
            s_component = e.Argument as FishingComponent;
            s_component?.GhInOutInstantiate();

            Parameter[] optimalParams = RunOptimizationLoop(s_worker);
            if (optimalParams == null || optimalParams.Length == 0)
            {
                return;
            }
            var pState = new ProgressState
            {
                Parameter = optimalParams
            };

            if (s_component != null)
            {
                s_component.OptimizationWindow.GrasshopperStatus = GrasshopperStates.RequestSent;
                s_worker?.ReportProgress(100, pState);
                while (s_component.OptimizationWindow.GrasshopperStatus != GrasshopperStates.RequestProcessed)
                { /* just wait until the cows come home */ }
            }

            s_worker?.CancelAsync();
        }

        private static Parameter[] RunOptimizationLoop(BackgroundWorker worker)
        {
            TLog.MethodStart();
            List<VariableBase> variables = s_component.GhInOut.Variables;
            Objective objectives = s_component.GhInOut.Objectives;
            Dictionary<string, FishEgg> enqueueItems = s_component.GhInOut.EnqueueItems;
            bool hasConstraint = s_component.GhInOut.HasConstraint;

            if (worker.CancellationPending)
            {
                return Array.Empty<Parameter>();
            }

            var optunaSolver = new Solver.Optuna(s_component.GhInOut.ComponentFolder, Settings, hasConstraint);
            bool solverStarted = optunaSolver.RunSolver(variables, objectives, enqueueItems, EvaluateFunction);

            return solverStarted ? optunaSolver.OptimalParameters : Array.Empty<Parameter>();
        }

        private static TrialGrasshopperItems EvaluateFunction(ProgressState pState, int progress)
        {
            TLog.MethodStart();
            s_component.OptimizationWindow.GrasshopperStatus = GrasshopperStates.RequestSent;

            s_worker.ReportProgress(progress, pState);
            while (s_component.OptimizationWindow.GrasshopperStatus != GrasshopperStates.RequestProcessed)
            { /*just wait*/ }

            return new TrialGrasshopperItems
            {
                Objectives = s_component.GhInOut.Objectives,
                GeometryJson = s_component.GhInOut.GetGeometryJson(),
                Attribute = s_component.GhInOut.GetAttributes(),
                Artifacts = s_component.GhInOut.Artifacts,
            };
        }
    }
}
