namespace Optuna.Sampler
{
    ///  <summary>
    /// https://optuna.readthedocs.io/en/stable/reference/generated/optuna.samplers.NSGAIIISampler.html
    /// </summary>
    public class NSGAIIISampler : NSGAIISampler
    {
        public double[] ReferencePoints { get; set; }
        public int DividingParameter { get; set; } = 3;

        public dynamic ToPython(dynamic optuna, bool hasConstraints)
        {
            return optuna.samplers.NSGAIIISampler(
                population_size: PopulationSize,
                mutation_prob: MutationProb,
                crossover_prob: CrossoverProb,
                swapping_prob: SwappingProb,
                seed: Seed,
                crossover: SetCrossover(optuna, Crossover),
                constraints_func: hasConstraints ? ConstraintFunc() : null
            );
        }
    }
}
