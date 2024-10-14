﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Tunny.Component.Optimizer;
using Tunny.Core.Handler;
using Tunny.Core.Input;
using Tunny.Core.Settings;
using Tunny.Core.TEnum;
using Tunny.Core.Util;
using Tunny.PostProcess;

namespace Tunny.Process
{
    internal static class OptimizeProcess
    {
        public static OptimizeComponentBase Component;
        public static TSettings Settings;
        public static bool IsForcedStopOptimize { get; set; }

        private static IProgress<ProgressState> s_progress;

        internal static void AddProgress(IProgress<ProgressState> progress)
        {
            TLog.MethodStart();
            s_progress = progress;
        }

        internal async static Task RunAsync()
        {
            TLog.MethodStart();
            Component?.GhInOutInstantiate();

            ProgressState progressState = await RunOptimizationLoopAsync();
            if (progressState.Parameter == null || progressState.Parameter.Count == 0)
            {
                return;
            }

            if (Component != null)
            {
                Component.GrasshopperStatus = GrasshopperStates.RequestSent;
                await ReportAsync(progressState);
                while (Component.GrasshopperStatus != GrasshopperStates.RequestProcessed)
                { /* just wait until the cows come home */ }
            }
        }

        private static async Task ReportAsync(ProgressState progressState)
        {
            TLog.MethodStart();
            await Task.Run(() => s_progress.Report(progressState));
        }

        private static async Task<ProgressState> RunOptimizationLoopAsync()
        {
            TLog.MethodStart();
            List<VariableBase> variables = Component.GhInOut.Variables;
            Input.Objective objectives = Component.GhInOut.Objectives;
            Dictionary<string, Type.FishEgg> fishEggs = Component.GhInOut.FishEggs;
            bool hasConstraint = Component.GhInOut.HasConstraint;
            var progressState = new ProgressState(Array.Empty<Parameter>());

            var optunaSolver = new Solver.Solver(Settings, hasConstraint);
            bool reinstateResult = await Task.Run(() =>
                optunaSolver.RunSolver(variables, objectives, fishEggs, EvaluateFunction)
            );

            return reinstateResult
                ? new ProgressState(optunaSolver.OptimalParameters)
                : new ProgressState(optunaSolver.OptimalParameters, true);
        }

        private static TrialGrasshopperItems EvaluateFunction(ProgressState pState, int progress)
        {
            TLog.MethodStart();
            s_progress.Report(pState);
            if (pState.IsReportOnly)
            {
                return null;
            }

            Component.GrasshopperStatus = GrasshopperStates.RequestSent;

            int step = 0;
            DateTime timer = DateTime.Now;
            while (Component.GrasshopperStatus != GrasshopperStates.RequestProcessed)
            {
                PrunerProgress(pState, ref step, ref timer);
            }
            pState.Pruner.ClearReporter();

            return new TrialGrasshopperItems
            {
                Objectives = Component.GhInOut.Objectives,
                GeometryJson = Component.GhInOut.GetGeometryJson(),
                Attribute = Component.GhInOut.GetAttributes(),
                Artifacts = Component.GhInOut.Artifacts,
            };
        }

        private static void PrunerProgress(ProgressState pState, ref int step, ref DateTime timer)
        {
            if (pState.Pruner.GetPrunerStatus() == PrunerStatus.Runnable
                && DateTime.Now - timer > TimeSpan.FromSeconds(pState.Pruner.EvaluateIntervalSeconds))
            {
                step = ReportPruner(pState.OptunaTrial, step, pState.Pruner);
                timer = DateTime.Now;
            }
        }

        private static int ReportPruner(dynamic optunaTrial, int step, Pruner pruner)
        {
            PrunerReport report = pruner.Evaluate();
            if (report == null)
            {
                return step;
            }
            else
            {
                optunaTrial.report(report.Value, step);
                if (string.IsNullOrEmpty(report.Attribute))
                {
                    optunaTrial.set_user_attr("intermediate_value_step" + step, report.Attribute);
                }

                if (optunaTrial.should_prune())
                {
                    pruner.RunStopperProcess();
                }

                return step + 1;
            }
        }
    }
}
