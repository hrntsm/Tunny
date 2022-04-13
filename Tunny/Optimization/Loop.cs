using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

using Tunny.Component;
using Tunny.Solver;
using Tunny.UI;
using Tunny.Util;

namespace Tunny.Optimization
{
    internal static class Loop
    {
        private static BackgroundWorker s_worker;
        private static TunnyComponent s_component;
        public static int NTrials;
        public static bool LoadIfExists;
        public static string SamplerType;
        public static string StudyName;

        internal static void RunOptimizationLoopMultiple(object sender, DoWorkEventArgs e)
        {
            s_worker = sender as BackgroundWorker;
            s_component = e.Argument as TunnyComponent;

            s_component.GhInOutInstantiate();

            double[] result = RunOptimizationLoop(s_worker);
            List<decimal> decimalResults = result.Select(Convert.ToDecimal).ToList();

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

            if (worker.CancellationPending)
            {
                return new[] { double.NaN };
            }

            var solver = new Optuna(s_component.GhInOut.ComponentFolder);
            Dictionary<string, object> settings = new Dictionary<string, object>()
            {
                { "nTrials", NTrials },
                { "loadIfExists", LoadIfExists },
                { "samplerType", SamplerType },
                { "studyName", StudyName },
                { "storage", s_component.GhInOut.ComponentFolder },
                { "nObjective", s_component.GhInOut.GetObjectiveValues().Count }
            };

            bool solverStarted = solver.RunSolver(
                variables, EvaluateFunction, "OptunaTPE", settings, "", "");

            return solverStarted ? solver.XOpt : new[] { double.NaN };
        }

        public static EvaluatedGHResult EvaluateFunction(IList<decimal> values, int progress)
        {
            s_component.OptimizationWindow.GrasshopperStatus = OptimizationWindow.GrasshopperStates.RequestSent;

            s_worker.ReportProgress(progress, values);
            while (s_component.OptimizationWindow.GrasshopperStatus != OptimizationWindow.GrasshopperStates.RequestProcessed)
            { /*just wait*/ }

            var result = new EvaluatedGHResult
            {
                ObjectiveValues = s_component.GhInOut.GetObjectiveValues(),
                ModelDraco = s_component.GhInOut.GetModelDraco()
            };
            return result;
        }
    }
}