namespace Tunny.Settings.Sampler
{
    /// <summary>
    /// https://optuna.readthedocs.io/en/stable/reference/samplers.html
    /// </summary>
    public class SamplerSettings
    {
        public Random Random { get; set; } = new Random();
        public Tpe Tpe { get; set; } = new Tpe();
        public CmaEs CmaEs { get; set; } = new CmaEs();
        public NSGAII NsgaII { get; set; } = new NSGAII();
        public QuasiMonteCarlo QMC { get; set; } = new QuasiMonteCarlo();
        public BoTorch BoTorch { get; set; } = new BoTorch();
    }
}
