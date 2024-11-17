using System;
using System.Collections.Generic;

using Optuna.Sampler;
using Optuna.Sampler.OptunaHub;

using Python.Runtime;

using Tunny.Core.TEnum;
using Tunny.Core.Util;

namespace Tunny.Core.Settings
{
    public class Sampler
    {
        public AutoSampler Auto { get; set; } = new AutoSampler();
        public RandomSampler Random { get; set; } = new RandomSampler();
        public TpeSampler Tpe { get; set; } = new TpeSampler();
        public CmaEsSampler CmaEs { get; set; } = new CmaEsSampler();
        public NSGAIISampler NsgaII { get; set; } = new NSGAIISampler();
        public NSGAIIISampler NsgaIII { get; set; } = new NSGAIIISampler();
        public QMCSampler QMC { get; set; } = new QMCSampler();
        public BoTorchSampler BoTorch { get; set; } = new BoTorchSampler();
        public GPSampler GP { get; set; } = new GPSampler();
        public BruteForceSampler BruteForce { get; set; } = new BruteForceSampler();

        public dynamic ToPython(SamplerType type, string storagePath, bool hasConstraints, Dictionary<string, double> firstVariables)
        {
            TLog.MethodStart();
            dynamic optunaSampler;
            dynamic optuna = Py.Import("optuna");
            dynamic optunahub = Py.Import("optunahub");
            switch (type)
            {
                case SamplerType.TPE:
                    optunaSampler = Tpe.ToPython(optuna, hasConstraints);
                    break;
                case SamplerType.GP:
                    optunaSampler = GP.ToPython(optuna);
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
                    optunaSampler = CmaEs.ToPython(optuna, storagePath, firstVariables);
                    break;
                case SamplerType.QMC:
                    optunaSampler = QMC.ToPython(optuna);
                    break;
                case SamplerType.Random:
                    optunaSampler = Random.ToPython(optuna);
                    break;
                case SamplerType.BruteForce:
                    optunaSampler = BruteForce.ToPython(optuna);
                    break;
                case SamplerType.AUTO:
                    optunaSampler = Auto.ToPython(optunahub);
                    break;

                default:
                    throw new ArgumentException("Invalid sampler type.");
            }
            return optunaSampler;
        }
    }
}
