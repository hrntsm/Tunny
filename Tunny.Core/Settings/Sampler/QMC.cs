using Tunny.Core.Util;

namespace Tunny.Settings.Sampler
{
    /// <summary>
    /// https://optuna.readthedocs.io/en/latest/reference/samplers/generated/optuna.samplers.QMCSampler.html
    /// </summary>
    public class QuasiMonteCarlo
    {
        public string QmcType { get; set; } = "sobol";
        public bool Scramble { get; set; }
        public int? Seed { get; set; }
        public bool WarnIndependentSampling { get; set; } = true;
        public bool WarnAsynchronousSeeding { get; set; } = true;

        public dynamic ToOptuna(dynamic optuna)
        {
            TLog.MethodStart();
            return optuna.samplers.QMCSampler(
                qmc_type: QmcType,
                scramble: Scramble,
                seed: Seed,
                warn_independent_sampling: WarnIndependentSampling
            );
        }
    }
}
