namespace Tunny.Core.TEnum
{
    public enum PrunerType
    {
        Hyperband,
        Median,
        Nop,
        Patient,
        Percentile,
        SuccessiveHalving,
        Threshold,
        Wilcoxon,
        None = -1
    }
}
