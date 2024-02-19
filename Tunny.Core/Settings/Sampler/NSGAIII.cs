namespace Tunny.Settings.Sampler
{
    ///  <summary>
    /// https://optuna.readthedocs.io/en/stable/reference/generated/optuna.samplers.NSGAIIISampler.html
    /// </summary>
    public class NSGAIII : NSGAII
    {
        public double[] ReferencePoints { get; set; }
        public int DividingParameter { get; set; } = 3;
    }
}
