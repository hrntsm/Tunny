using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

using Tunny.Component.Optimizer;
using Tunny.Enum;
using Tunny.Input;
using Tunny.PostProcess;
using Tunny.Settings;
using Tunny.Solver;
using Tunny.Type;

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
            s_worker = sender as BackgroundWorker;
            s_component = e.Argument as FishingComponent;
            s_component?.GhInOutInstantiate();

            double[] result = RunOptimizationLoop(s_worker);
            if (result == null || double.IsNaN(result[0]))
            {
                return;
            }
            var decimalResults = result.Select(Convert.ToDecimal).ToList();
            var pState = new ProgressState
            {
                Values = decimalResults
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

        private static double[] RunOptimizationLoop(BackgroundWorker worker)
        {
            List<VariableBase> variables = s_component.GhInOut.Variables;
            Objective objectives = s_component.GhInOut.Objectives;
            Dictionary<string, FishEgg> enqueueItems = s_component.GhInOut.EnqueueItems;
            bool hasConstraint = s_component.GhInOut.HasConstraint;

            if (worker.CancellationPending)
            {
                return new[] { double.NaN };
            }

            var optunaSolver = new Solver.Optuna(s_component.GhInOut.ComponentFolder, Settings, hasConstraint);
            bool solverStarted = optunaSolver.RunSolver(variables, objectives, enqueueItems, EvaluateFunction);

            return solverStarted ? optunaSolver.XOpt : new[] { double.NaN };
        }

        private static TrialGrasshopperItems EvaluateFunction(ProgressState pState, int progress)
        {
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
