using System;

using Python.Runtime;

namespace Optuna.Sampler
{
    ///  <summary>
    /// https://optuna.readthedocs.io/en/stable/reference/generated/optuna.samplers.NSGAIISampler.html
    /// </summary>
    public class NSGAIISampler : GASamplerBase
    {
        private const string Package = "samplers/nsgaii_with_initial_trials";
        public double? MutationProb { get; set; }
        public int PopulationSize { get; set; } = 50;
        public string Crossover { get; set; } = "BLXAlpha";
        public double CrossoverProb { get; set; } = 0.9;
        public double SwappingProb { get; set; } = 0.5;

        public dynamic ToPython(bool hasConstraints)
        {
            dynamic optuna = Py.Import("optuna");
            dynamic optunahub = Py.Import("optunahub");
            dynamic module = optunahub.load_module(package: Package);
            return module.NSGAIIwITSampler(
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
