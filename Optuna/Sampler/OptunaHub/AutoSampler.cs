namespace Optuna.Sampler.OptunaHub
{
    /// <summary>
    /// https://hub.optuna.org/samplers/auto_sampler/
    /// </summary>
    public class AutoSampler : SamplerBase
    {
        private const string Package = "samplers/auto_sampler";

        public dynamic ToPython(dynamic optunahub)
        {
            dynamic module = optunahub.load_module(package: Package);
            return module.AutoSampler(
                seed: Seed
            );
        }
    }
}
