using System;

namespace Optuna.Sampler.OptunaHub
{
    /// <summary>
    /// https://hub.optuna.org/samplers/moead/
    /// </summary>
    public class MOEADSampler : SamplerBase
    {
        private const string Package = "samplers/moead";
        public double? MutationProb { get; set; }
        public int PopulationSize { get; set; } = 50;
        public string Crossover { get; set; } = "BLXAlpha";
        public double CrossoverProb { get; set; } = 0.9;
        public double SwappingProb { get; set; } = 0.5;
        public ScalarAggregationType ScalarAggregation { get; set; } = ScalarAggregationType.tchebycheff;
        public int NumNeighbors { get; set; } = 5;

        public dynamic ToPython(dynamic optuna, dynamic optunahub)
        {
            if (NumNeighbors <= 0)
            {
                NumNeighbors = PopulationSize / 10;
            }
            dynamic module = optunahub.load_module(package: Package);
            return module.MOEADSampler(
                population_size: PopulationSize,
                mutation_prob: MutationProb,
                crossover_prob: CrossoverProb,
                swapping_prob: SwappingProb,
                seed: Seed,
                crossover: SetCrossover(optuna, Crossover),
                scalar_aggregation_func: ScalarAggregation.ToString(),
                n_neighbors: NumNeighbors
            );
        }

        protected static dynamic SetCrossover(dynamic optuna, string crossover)
        {
            switch (crossover)
            {
                case "Uniform":
                    return optuna.samplers.nsgaii.UniformCrossover();
                case "BLXAlpha":
                    return optuna.samplers.nsgaii.BLXAlphaCrossover();
                case "SPX":
                    return optuna.samplers.nsgaii.SPXCrossover();
                case "SBX":
                    return optuna.samplers.nsgaii.SBXCrossover();
                case "VSBX":
                    return optuna.samplers.nsgaii.VSBXCrossover();
                case "UNDX":
                    return optuna.samplers.nsgaii.UNDXCrossover();
                case "":
                    return null;
                default:
                    throw new ArgumentException("Unexpected crossover setting");
            }
        }
    }

    public enum ScalarAggregationType
    {
        weighted_sum,
        tchebycheff
    }
}
