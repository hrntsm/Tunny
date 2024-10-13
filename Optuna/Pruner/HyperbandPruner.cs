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
            return optuna.pruners.HyperbandPruner(
                min_resource: MinResource,
                max_resource: MaxResource,
                reduction_factor: ReductionFactor,
                bootstrap_count: BootstrapCount);
        }
    }
}
