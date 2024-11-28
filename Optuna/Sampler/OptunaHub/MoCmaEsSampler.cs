using Python.Runtime;

namespace Optuna.Sampler.OptunaHub
{
    public class MoCmaEsSampler : SamplerBase
    {
        private const string Package = "samplers/mocma";
        public int? PopulationSize { get; set; }

        public dynamic ToPython()
        {
            dynamic optunahub = Py.Import("optunahub");
            dynamic module = optunahub.load_module(package: Package);
            return module.MoCmaSampler(
                popsize: PopulationSize,
                seed: Seed
            );
        }
    }
}
