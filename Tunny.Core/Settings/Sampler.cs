using System;

using Optuna.Sampler;

using Tunny.Core.TEnum;
using Tunny.Core.Util;

namespace Tunny.Settings.Sampler
{
    public class Sampler
    {
        public RandomSampler Random { get; } = new RandomSampler();
        public TpeSampler Tpe { get; set; } = new TpeSampler();
        public CmaEsSampler CmaEs { get; set; } = new CmaEsSampler();
        public NSGAIISampler NsgaII { get; set; } = new NSGAIISampler();
        public NSGAIIISampler NsgaIII { get; set; } = new NSGAIIISampler();
        public QMCSampler QMC { get; set; } = new QMCSampler();
        public BoTorchSampler BoTorch { get; set; } = new BoTorchSampler();

        public dynamic ToPython(dynamic optuna, SamplerType type, string storagePath, bool hasConstraints)
        {
            TLog.MethodStart();
            dynamic optunaSampler;
            switch (type)
            {
                case SamplerType.TPE:
                    optunaSampler = Tpe.ToPython(optuna, hasConstraints);
                    break;
                case SamplerType.BoTorch:
                    optunaSampler = BoTorch.ToPython(optuna, hasConstraints);
                    break;
                case SamplerType.NSGAII:
                    optunaSampler = NsgaII.ToPython(optuna, hasConstraints);
                    break;
                case SamplerType.NSGAIII:
                    optunaSampler = NsgaIII.ToPython(optuna, hasConstraints);
                    break;
                case SamplerType.CmaEs:
                    optunaSampler = CmaEs.ToPython(optuna, storagePath);
                    break;
                case SamplerType.QMC:
                    optunaSampler = QMC.ToPython(optuna);
                    break;
                case SamplerType.Random:
                    optunaSampler = Random.ToPython(optuna);
                    break;
                default:
                    throw new ArgumentException("Invalid sampler type.");
            }
            return optunaSampler;
        }
    }
}
