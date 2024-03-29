namespace Optuna.Sampler
{
    /// <summary>
    /// https://optuna.readthedocs.io/en/latest/reference/samplers/generated/optuna.samplers.QMCSampler.html
    /// </summary>
    public class QMCSampler : SamplerBase
    {
        public string QmcType { get; set; } = "sobol";
        public bool Scramble { get; set; }
        public bool WarnIndependentSampling { get; set; } = true;
        public bool WarnAsynchronousSeeding { get; set; } = true;

        public dynamic ToPython(dynamic optuna)
        {
            return optuna.samplers.QMCSampler(
                qmc_type: QmcType,
                scramble: Scramble,
                seed: Seed,
                warn_independent_sampling: WarnIndependentSampling
            );
        }
    }
}
