namespace Optuna.Pruner
{
    /// <summary>
    /// https://optuna.readthedocs.io/en/stable/reference/generated/optuna.pruners.MedianPruner.html#optuna.pruners.MedianPruner
    /// </summary>
    public class MedianPruner : IPruner
    {
        public int NumStartupTrials { get; set; } = 5;
        public int NumWarmupSteps { get; set; }
        public int IntervalSteps { get; set; } = 1;
        public int NumMinTrials { get; set; } = 1;

        public MedianPruner()
        {
        }

        public MedianPruner(int numStartupTrials, int numWarmupSteps, int intervalSteps, int numMinTrials)
        {
            NumStartupTrials = numStartupTrials;
            NumWarmupSteps = numWarmupSteps;
            IntervalSteps = intervalSteps;
            NumMinTrials = numMinTrials;
        }

        public dynamic ToPython(dynamic optuna)
        {
            return optuna.pruners.MedianPruner(
                n_startup_trials: NumStartupTrials,
                n_warmup_steps: NumWarmupSteps,
                interval_steps: IntervalSteps,
                min_trials: NumMinTrials);
        }
    }
}
