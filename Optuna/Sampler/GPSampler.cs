namespace Optuna.Sampler
{
    /// <summary>
    /// https://optuna.readthedocs.io/en/stable/reference/generated/optuna.samplers.GPSampler.html
    /// </summary>
    public class GPSampler : SamplerBase
    {
        public int NStartupTrials { get; set; } = 10;
        public bool DeterministicObjective { get; set; }

        public dynamic ToPython(dynamic optuna)
        {
            return optuna.samplers.GPSampler(
                seed: Seed,
                n_startup_trials: NStartupTrials,
                deterministic_objective: DeterministicObjective
            );
        }
    }
}
