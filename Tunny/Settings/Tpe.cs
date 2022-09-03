namespace Tunny.Settings
{
    /// <summary>
    /// https://optuna.readthedocs.io/en/stable/reference/generated/optuna.samplers.TPESampler.html
    /// </summary>
    public class Tpe
    {
        public int? Seed { get; set; }
        public bool ConsiderPrior { get; set; } = true;
        public double PriorWeight { get; set; } = 1.0;
        public bool ConsiderMagicClip { get; set; } = true;
        public bool ConsiderEndpoints { get; set; }
        public int NStartupTrials { get; set; } = 10;
        public int NEICandidates { get; set; } = 24;
        public bool Multivariate { get; set; } = true;
        public bool Group { get; set; }
        public bool WarnIndependentSampling { get; set; } = true;
        public bool ConstantLiar { get; set; }
    }
}
