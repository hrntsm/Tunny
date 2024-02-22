namespace Optuna.Sampler
{
    /// <summary>
    /// https://optuna.readthedocs.io/en/stable/reference/generated/optuna.samplers.CmaEsSampler.html
    /// </summary>
    public class CmaEsSampler : SamplerBase
    {
        public double? Sigma0 { get; set; }
        public int NStartupTrials { get; set; } = 1;
        public bool WarnIndependentSampling { get; set; } = true;
        public bool ConsiderPrunedTrials { get; set; }
        public string RestartStrategy { get; set; } = string.Empty;
        public int IncPopsize { get; set; } = 2;
        public int? PopulationSize { get; set; }
        public bool UseSeparableCma { get; set; }
        public bool UseWarmStart { get; set; }
        public string WarmStartStudyName { get; set; } = string.Empty;
        public bool WithMargin { get; set; } = true;

        public dynamic ToPython(dynamic optuna, string storagePath)
        {
            return UseWarmStart
                ? optuna.samplers.CmaEsSampler(
                    n_startup_trials: NStartupTrials,
                    warn_independent_sampling: WarnIndependentSampling,
                    seed: Seed,
                    consider_pruned_trials: ConsiderPrunedTrials,
                    restart_strategy: RestartStrategy == string.Empty ? null : RestartStrategy,
                    inc_popsize: IncPopsize,
                    popsize: PopulationSize,
                    source_trials: optuna.load_study(study_name: WarmStartStudyName, storage: storagePath).get_trials(),
                    with_margin: WithMargin
                )
                : optuna.samplers.CmaEsSampler(
                    sigma0: Sigma0,
                    n_startup_trials: NStartupTrials,
                    warn_independent_sampling: WarnIndependentSampling,
                    seed: Seed,
                    consider_pruned_trials: ConsiderPrunedTrials,
                    restart_strategy: RestartStrategy == string.Empty ? null : RestartStrategy,
                    inc_popsize: IncPopsize,
                    popsize: PopulationSize,
                    use_separable_cma: UseSeparableCma,
                    with_margin: WithMargin
                );
        }
    }
}
