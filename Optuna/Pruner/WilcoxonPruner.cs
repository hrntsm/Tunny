namespace Optuna.Pruner
{
    public class WilcoxonPruner : IPruner
    {
        public double PThreshold { get; set; } = 0.1;
        public int NumStartupTrials { get; set; }

        public WilcoxonPruner()
        {
        }

        public WilcoxonPruner(double pThreshold, int nStartupTrials)
        {
            PThreshold = pThreshold;
            NumStartupTrials = nStartupTrials;
        }

        public dynamic ToPython(dynamic optuna)
        {
            return optuna.pruners.WilcoxonPruner(
                p_threshold: PThreshold,
                n_startup_trials: NumStartupTrials
            );
        }
    }
}
