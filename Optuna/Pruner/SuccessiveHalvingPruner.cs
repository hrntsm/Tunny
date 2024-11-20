namespace Optuna.Pruner
{
    public class SuccessiveHalvingPruner : IPruner
    {
        public string MinResource { get; set; } = "auto";
        public int ReductionFactor { get; set; } = 4;
        public int MinEarlyStoppingRate { get; set; }
        public int BootstrapCount { get; set; }

        public SuccessiveHalvingPruner()
        {
        }

        public SuccessiveHalvingPruner(string minResource, int reductionFactor, int minEarlyStoppingRate, int bootstrapCount)
        {
            MinResource = minResource;
            ReductionFactor = reductionFactor;
            MinEarlyStoppingRate = minEarlyStoppingRate;
            BootstrapCount = bootstrapCount;
        }

        public dynamic ToPython(dynamic optuna)
        {
            bool parseResult = int.TryParse(MinResource, out int intMinResource);
            return parseResult && intMinResource > 0
                ? optuna.pruners.SuccessiveHalvingPruner(
                    min_resource: intMinResource,
                    reduction_factor: ReductionFactor,
                    min_early_stopping_rate: MinEarlyStoppingRate,
                    bootstrap_count: BootstrapCount)
                : optuna.pruners.SuccessiveHalvingPruner(
                    min_resource: "auto",
                    reduction_factor: ReductionFactor,
                    min_early_stopping_rate: MinEarlyStoppingRate,
                    bootstrap_count: BootstrapCount);
        }
    }
}
