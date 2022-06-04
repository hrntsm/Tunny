namespace Tunny.Settings
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
        public bool ConsiderPrunedTrials { get; set; } = false;
        public string RestartStrategy { get; set; }
        public int IncPopsize { get; set; } = 2;
        public bool UseSeparableCma { get; set; } = false;
    }
}
