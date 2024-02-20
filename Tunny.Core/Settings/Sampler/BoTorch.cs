using Tunny.Core.Util;

namespace Tunny.Settings.Sampler
{
    /// <summary>
    /// https://optuna.readthedocs.io/en/stable/reference/generated/optuna.integration.BoTorchSampler.html
    /// </summary>
    public class BoTorch
    {
        public int? Seed { get; set; }
        public int NStartupTrials { get; set; } = 10;

        public dynamic ToOptuna(dynamic optuna, bool hasConstraints)
        {
            TLog.MethodStart();
            return optuna.integration.BoTorchSampler(
                seed: Seed,
                n_startup_trials: NStartupTrials,
                constraints_func: hasConstraints ? SamplerSettings.ConstraintFunc() : null
            );
        }
    }
}
