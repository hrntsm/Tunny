namespace Optuna.Sampler
{
    /// <summary>
    /// https://optuna.readthedocs.io/en/stable/reference/samplers/generated/optuna.samplers.BruteForceSampler.html
    /// </summary>
    public class BruteForceSampler : SamplerBase
    {
        public dynamic ToPython(dynamic optuna)
        {
            return optuna.samplers.BruteForceSampler(
                seed: Seed
            );
        }
    }
}
