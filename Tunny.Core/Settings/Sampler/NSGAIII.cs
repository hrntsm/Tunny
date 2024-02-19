using Tunny.Core.Util;

namespace Tunny.Settings.Sampler
{
    ///  <summary>
    /// https://optuna.readthedocs.io/en/stable/reference/generated/optuna.samplers.NSGAIIISampler.html
    /// </summary>
    public class NSGAIII : NSGAII
    {
        public double[] ReferencePoints { get; set; }
        public int DividingParameter { get; set; } = 3;

        public new dynamic ToOptuna(dynamic optuna, bool hasConstraints)
        {
            TLog.MethodStart();
            return optuna.samplers.NSGAIIISampler(
                population_size: PopulationSize,
                mutation_prob: MutationProb,
                crossover_prob: CrossoverProb,
                swapping_prob: SwappingProb,
                seed: Seed,
                crossover: SetCrossover(optuna, Crossover),
                constraints_func: hasConstraints ? SamplerSettings.ConstraintFunc() : null
            );
        }
    }
}
