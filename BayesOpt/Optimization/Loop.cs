using System.Collections.Generic;
using System.ComponentModel;

using BayesOpt.Component;
using BayesOpt.Solver;

namespace BayesOpt.Util
{
    internal static class Loop
    {
        private static BackgroundWorker s_worker;
        private static WithUI s_component;
        public static int Runs;

        internal static void RunOptimizationLoopMultiple(object sender, DoWorkEventArgs e)
        {
            s_worker = sender as BackgroundWorker;
            s_component = e.Argument as WithUI;

            s_component.GhInOutInstantiate();
            s_component.GhInOut.SetVariables();
            s_component.GhInOut.SetObjectives();

            int runCount = 0;

            while (runCount < Runs)
            {
                if (s_worker == null || s_worker.CancellationPending)
                {
                    break;
                }

                RunOptimizationLoop(s_worker);

                runCount++;
            }
        }

        private static void RunOptimizationLoop(BackgroundWorker worker)
        {
            List<Variable> variables = s_component.GhInOut.Variables;

            if (worker.CancellationPending)
            {
                return;
            }

            var solver = new OptunaTPE();

            bool solverStarted = solver.RunSolver(
                variables, EvaluateFunction, "OptunaTPE", "", "", "");
        }

        public static List<double> EvaluateFunction(IList<decimal> values)
        {
            s_worker.ReportProgress(0, values);
            var objectiveValues = s_component.GhInOut.GetObjectiveValues();
            return objectiveValues;
        }
    }
}