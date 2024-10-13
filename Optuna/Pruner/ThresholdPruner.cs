namespace Optuna.Pruner
{
    public class ThresholdPruner : IPruner
    {
        public double? Lower { get; set; }
        public double? Upper { get; set; }
        public int NumWarmupSteps { get; set; }
        public int IntervalSteps { get; set; } = 1;

        public ThresholdPruner()
        {
        }

        public ThresholdPruner(double? lower, double? upper, int numWarmupSteps, int intervalSteps)
        {
            Lower = lower;
            Upper = upper;
            NumWarmupSteps = numWarmupSteps;
            IntervalSteps = intervalSteps;
        }

        public dynamic ToPython(dynamic optuna)
        {
            return optuna.pruners.ThresholdPruner(
                lower: Lower,
                upper: Upper,
                n_warmup_steps: NumWarmupSteps,
                interval_steps: IntervalSteps
            );
        }
    }
}
