using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

using Grasshopper.Kernel;

using Tunny.Component;
using Tunny.Settings;
using Tunny.Solver.Optuna;
using Tunny.UI;
using Tunny.Util;

namespace Tunny.Optimization
{
    internal static class OptimizeLoop
    {
        private static BackgroundWorker s_worker;
        private static TunnyComponent s_component;
        public static TunnySettings Settings;
        public static bool IsForcedStopOptimize { get; set; }

        internal static void RunMultiple(object sender, DoWorkEventArgs e)
        {
            s_worker = sender as BackgroundWorker;
            s_component = e.Argument as TunnyComponent;
            s_component.GhInOutInstantiate();

            double[] result = RunOptimizationLoop(s_worker);
            if (result == null || double.IsNaN(result[0]))
            {
                return;
            }
            var decimalResults = result.Select(Convert.ToDecimal).ToList();

            s_component.OptimizationWindow.GrasshopperStatus = OptimizationWindow.GrasshopperStates.RequestSent;
            s_worker.ReportProgress(100, decimalResults);
            while (s_component.OptimizationWindow.GrasshopperStatus != OptimizationWindow.GrasshopperStates.RequestProcessed)
            { /* just wait until the cows come home */}

            if (s_worker != null)
            {
                s_worker.CancelAsync();
            }
        }

        private static double[] RunOptimizationLoop(BackgroundWorker worker)
        {
            List<Variable> variables = s_component.GhInOut.Variables;
            List<IGH_Param> objectives = s_component.GhInOut.Objectives;
            bool hasConstraint = s_component.GhInOut.HasConstraint;

            if (worker.CancellationPending)
            {
                return new[] { double.NaN };
            }

            var optunaSolver = new Optuna(s_component.GhInOut.ComponentFolder, Settings, s_component.GhInOut.HasConstraint);
            bool solverStarted = optunaSolver.RunSolver(variables, objectives, EvaluateFunction);

            return solverStarted ? optunaSolver.XOpt : new[] { double.NaN };
        }

        private static EvaluatedGHResult EvaluateFunction(IList<decimal> values, int progress)
        {
            s_component.OptimizationWindow.GrasshopperStatus = OptimizationWindow.GrasshopperStates.RequestSent;

            s_worker.ReportProgress(progress, values);
            while (s_component.OptimizationWindow.GrasshopperStatus != OptimizationWindow.GrasshopperStates.RequestProcessed)
            { /*just wait*/ }

            var result = new EvaluatedGHResult
            {
                ObjectiveValues = s_component.GhInOut.GetObjectiveValues(),
                GeometryJson = s_component.GhInOut.GetGeometryJson(),
                Attribute = s_component.GhInOut.GetAttributes()
            };
            return result;
        }
    }
}
