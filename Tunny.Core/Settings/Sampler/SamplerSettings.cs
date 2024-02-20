using System;

using Python.Runtime;

using Tunny.Core.TEnum;
using Tunny.Core.Util;

namespace Tunny.Settings.Sampler
{
    /// <summary>
    /// https://optuna.readthedocs.io/en/stable/reference/samplers.html
    /// </summary>
    public class SamplerSettings
    {
        public Random Random { get; } = new Random();
        public Tpe Tpe { get; set; } = new Tpe();
        public CmaEs CmaEs { get; set; } = new CmaEs();
        public NSGAII NsgaII { get; set; } = new NSGAII();
        public NSGAIII NsgaIII { get; set; } = new NSGAIII();
        public QuasiMonteCarlo QMC { get; set; } = new QuasiMonteCarlo();
        public BoTorch BoTorch { get; set; } = new BoTorch();
        private static readonly string ConstraintKey = "Constraint";

        public static dynamic ConstraintFunc()
        {
            TLog.MethodStart();
            PyModule ps = Py.CreateScope();
            ps.Exec(
                "def constraints(trial):\n" +
                $"  return trial.user_attrs[\"{ConstraintKey}\"]\n"
            );
            return ps.Get("constraints");
        }

        public dynamic ToOptuna(dynamic optuna, SamplerType type, string storagePath, bool hasConstraints)
        {
            TLog.MethodStart();
            dynamic optunaSampler;
            switch (type)
            {
                case SamplerType.TPE:
                    optunaSampler = Tpe.ToOptuna(optuna, hasConstraints);
                    break;
                case SamplerType.BoTorch:
                    optunaSampler = BoTorch.ToOptuna(optuna, hasConstraints);
                    break;
                case SamplerType.NSGAII:
                    optunaSampler = NsgaII.ToOptuna(optuna, hasConstraints);
                    break;
                case SamplerType.NSGAIII:
                    optunaSampler = NsgaIII.ToOptuna(optuna, hasConstraints);
                    break;
                case SamplerType.CmaEs:
                    optunaSampler = CmaEs.ToOptuna(optuna, storagePath);
                    break;
                case SamplerType.QMC:
                    optunaSampler = QMC.ToOptuna(optuna);
                    break;
                case SamplerType.Random:
                    optunaSampler = Random.ToOptuna(optuna);
                    break;
                default:
                    throw new ArgumentException("Invalid sampler type.");
            }
            return optunaSampler;
        }
    }
}
