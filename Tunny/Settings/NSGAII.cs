namespace Tunny.Settings
{
    ///  <summary>
    /// https://optuna.readthedocs.io/en/stable/reference/generated/optuna.samplers.NSGAIISampler.html
    /// </summary>
    public class NSGAII
    {
        public int? Seed { get; set; }
        public double? MutationProb { get; set; }
        public int PopulationSize { get; set; } = 50;
        public double CrossoverProb { get; set; } = 0.9;
        public double SwappingProb { get; set; } = 0.5;
    }
}
