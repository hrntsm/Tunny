using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Grasshopper.GUI;

using Tunny.Component.Optimizer;
using Tunny.Core.Handler;
using Tunny.Core.Input;
using Tunny.Core.Settings;
using Tunny.Core.TEnum;
using Tunny.Core.Util;
using Tunny.Input;
using Tunny.PostProcess;
using Tunny.WPF;
using Tunny.WPF.Models;
using Tunny.WPF.ViewModels.Optimize;

namespace Tunny.Process
{
    internal static class OptimizeProcess
    {
        private const string IntermediateValueKey = "intermediate_value_step_";
        internal const string PrunedTrialReportValueKey = "pruned_trial_report_value";

        public static OptimizeComponentBase Component;
        public static TSettings Settings;
        public static bool IsForcedStopOptimize { get; set; }
        public static GH_DocumentEditor GH_DocumentEditor { get; internal set; }
        public static MainWindow TunnyWindow { get; internal set; }

        private static IProgress<ProgressState> s_progress;
        private static OptimizeViewModel s_optimizeViewModel;

        internal static void AddProgress(IProgress<ProgressState> progress)
        {
            TLog.MethodStart();
            s_progress = progress;
        }

        internal async static Task RunAsync(OptimizeViewModel optimizeViewModel)
        {
            TLog.MethodStart();
            Component?.GhInOutInstantiate();
            s_optimizeViewModel = optimizeViewModel;

            ProgressState progressState = await RunOptimizationLoopAsync();
            if (progressState.Parameter == null || progressState.Parameter.Count == 0)
            {
                return;
            }

            if (Component != null)
            {
                var tcs = new TaskCompletionSource<bool>();
                void EventHandler(object sender, GrasshopperStates status)
                {
                    if (status == GrasshopperStates.RequestProcessed)
                    {
                        tcs.SetResult(true);
                    }
                }
                Component.GrasshopperStatusChanged += EventHandler;
                try
                {
                    Component.GrasshopperStatus = GrasshopperStates.RequestSent;
                    await ReportAsync(progressState);
                    await tcs.Task;
                }
                finally
                {
                    Component.GrasshopperStatusChanged -= EventHandler;
                }
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
            Objective objectives = SetObjectives();
            List<VariableBase> variables = SetVariables();
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

        private static List<VariableBase> SetVariables()
        {
            List<VariableBase> variables = Component.GhInOut.Variables;
            int count = 0;
            foreach (VariableBase variable in variables)
            {
                if (variable is NumberVariable numberVariable)
                {
                    numberVariable.IsLogScale = s_optimizeViewModel.VariableSettingItems[count].IsLogScale;
                    count++;
                }
            }

            return variables;
        }

        private static Objective SetObjectives()
        {
            Objective objectives = Component.GhInOut.Objectives;
            IEnumerable<ObjectiveSettingItem> settingItems = s_optimizeViewModel.ObjectiveSettingItems;
            objectives.SetDirections(settingItems);
            return objectives;
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
            if (Settings.Pruner.IsEnabled
                && pState.Pruner.GetPrunerStatus() == PrunerStatus.Runnable
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
                optunaTrial.report(report.IntermediateValue, step);
                if (!string.IsNullOrEmpty(report.Attribute))
                {
                    optunaTrial.set_user_attr(IntermediateValueKey + step, report.Attribute);
                }

                if (optunaTrial.should_prune())
                {
                    pruner.RunStopperProcess();
                    if (report.TrialTellValue.HasValue)
                    {
                        optunaTrial.set_user_attr(PrunedTrialReportValueKey, report.TrialTellValue.Value);
                    }
                }

                return step + 1;
            }
        }
    }
}
