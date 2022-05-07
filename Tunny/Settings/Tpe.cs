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
        public bool ConsiderEndpoints { get; set; } = false;
        public int NStartupTrials { get; set; } = 10;
        public int NEICandidates { get; set; } = 24;
        public bool Multivariate { get; set; } = false;
        public bool Group { get; set; } = false;
        public bool WarnIndependentSampling { get; set; } = true;
        public bool ConstantLiar { get; set; } = false;
    }
}