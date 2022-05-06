namespace Tunny.Settings
{
    class NSGAII
    {
        public int Seed { get; set; }
        public int PopulationSize { get; set; } = 50;
        public double MutationProb { get; set; } = 0;
        public double CrossoverProb { get; set; } = 0.9;
        public double SwappingProb { get; set; } = 0.5;
    }
}