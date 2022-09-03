namespace Tunny.Settings
{
    /// <summary>
    /// https://optuna.readthedocs.io/en/latest/reference/samplers/generated/optuna.samplers.QMCSampler.html
    /// </summary>
    public class QuasiMonteCarlo
    {
        public string QmcType { get; set; } = "sobol";
        public bool Scramble { get; set; }
        public int? Seed { get; set; }
        public bool WarnIndependentSampling { get; set; } = true;
        public bool WarnAsynchronousSeeding { get; set; } = true;
    }
}
