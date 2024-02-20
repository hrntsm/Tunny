using Tunny.Core.Util;

namespace Tunny.Settings.Sampler
{
    /// <summary>
    /// https://optuna.readthedocs.io/en/stable/reference/generated/optuna.samplers.RandomSampler.html#optuna.samplers.RandomSampler
    /// </summary>
    public class Random
    {
        public int? Seed { get; set; }

        public dynamic ToOptuna(dynamic optuna)
        {
            TLog.MethodStart();
            return optuna.samplers.RandomSampler(
                seed: Seed
            );
        }
    }
}
