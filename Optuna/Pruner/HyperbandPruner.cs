namespace Optuna.Pruner
{
    public class HyperbandPruner : IPruner
    {
        public int MinResource { get; set; } = 1;
        public string MaxResource { get; set; } = "auto";
        public int ReductionFactor { get; set; } = 3;
        public int BootstrapCount { get; set; }

        public HyperbandPruner()
        {
        }

        public HyperbandPruner(int minResource, string maxResource, int reductionFactor, int bootstrapCount)
        {
            MinResource = minResource;
            MaxResource = maxResource;
            ReductionFactor = reductionFactor;
            BootstrapCount = bootstrapCount;
        }

        public dynamic ToPython(dynamic optuna)
        {
            bool parseResult = int.TryParse(MaxResource, out int intMaxResource);
            return parseResult && intMaxResource > MinResource
                ? optuna.pruners.HyperbandPruner(
                    min_resource: MinResource,
                    max_resource: intMaxResource,
                    reduction_factor: ReductionFactor,
                    bootstrap_count: BootstrapCount)
                : optuna.pruners.HyperbandPruner(
                    min_resource: MinResource,
                    max_resource: "auto",
                    reduction_factor: ReductionFactor,
                    bootstrap_count: BootstrapCount);
        }
    }
}
