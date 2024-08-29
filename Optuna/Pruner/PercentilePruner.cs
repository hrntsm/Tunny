namespace Optuna.Pruner
{
    public class PercentilePruner : IPruner
    {
        public double Percentile { get; set; } = 25.0;
        public int NumStartupTrials { get; set; } = 5;
        public int NumWarmupSteps { get; set; }
        public int IntervalSteps { get; set; } = 1;
        public int NumMinTrials { get; set; } = 1;

        public PercentilePruner()
        {
        }

        public PercentilePruner(double percentile, int nStartupTrials, int nWarmupSteps, int intervalSteps, int minTrials)
        {
            Percentile = percentile;
            NumStartupTrials = nStartupTrials;
            NumWarmupSteps = nWarmupSteps;
            IntervalSteps = intervalSteps;
            NumMinTrials = minTrials;
        }

        public dynamic ToPython(dynamic optuna)
        {
            return optuna.pruners.PercentilePruner(
                percentile: Percentile,
                n_startup_trials: NumStartupTrials,
                n_warmup_steps: NumWarmupSteps,
                interval_steps: IntervalSteps,
                n_min_trials: NumMinTrials
            );
        }
    }
}
