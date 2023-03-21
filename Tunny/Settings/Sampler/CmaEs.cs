namespace Tunny.Settings.Sampler
{
    /// <summary>
    /// https://optuna.readthedocs.io/en/stable/reference/generated/optuna.samplers.CmaEsSampler.html
    /// </summary>
    public class CmaEs
    {
        public int? Seed { get; set; }
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
        public bool WithMargin { get; set; }
    }
}
