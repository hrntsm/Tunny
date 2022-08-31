using System;
using System.Collections.Generic;

using Python.Runtime;

using Tunny.Settings;
using Tunny.Util;

namespace Tunny.Solver.Optuna
{
    public static class Sampler
    {
        internal static dynamic Random(dynamic optuna, TunnySettings settings)
        {
            Settings.Random random = settings.Optimize.Sampler.Random;
            return optuna.samplers.RandomSampler(
                seed: random.Seed
            );
        }

        internal static dynamic CmaEs(dynamic optuna, TunnySettings settings)
        {
            CmaEs cmaEs = settings.Optimize.Sampler.CmaEs;
            return optuna.samplers.CmaEsSampler(
                sigma0: cmaEs.Sigma0,
                n_startup_trials: cmaEs.NStartupTrials,
                warn_independent_sampling: cmaEs.WarnIndependentSampling,
                seed: cmaEs.Seed,
                consider_pruned_trials: cmaEs.ConsiderPrunedTrials,
                restart_strategy: cmaEs.RestartStrategy == string.Empty ? null : cmaEs.RestartStrategy,
                inc_popsize: cmaEs.IncPopsize,
                popsize: cmaEs.PopulationSize,
                use_separable_cma: cmaEs.UseSeparableCma
            );
        }

        internal static dynamic Grid(dynamic optuna, List<Variable> variables, ref int nTrials)
        {
            var searchSpace = new PyDict();
            for (int i = 0; i < variables.Count; i++)
            {
                var numSpace = new PyList();
                for (int j = 0; j < nTrials; j++)
                {
                    numSpace.Append(new PyFloat(variables[i].LowerBond + (variables[i].UpperBond - variables[i].LowerBond) * j / (nTrials - 1)));
                }
                searchSpace.SetItem(new PyString(variables[i].NickName), numSpace);
            }
            nTrials = (int)Math.Pow(nTrials, variables.Count);
            return optuna.samplers.GridSampler(searchSpace);
        }

        internal static dynamic NSGAII(dynamic optuna, TunnySettings settings, bool hasConstraints)
        {
            NSGAII nsga2 = settings.Optimize.Sampler.NsgaII;
            return optuna.samplers.NSGAIISampler(
                population_size: nsga2.PopulationSize,
                mutation_prob: nsga2.MutationProb,
                crossover_prob: nsga2.CrossoverProb,
                swapping_prob: nsga2.SwappingProb,
                seed: nsga2.Seed,
                crossover: SetCrossover(optuna, nsga2.Crossover),
                constraints_func: hasConstraints ? ConstraintFunc() : null
            );
        }

        private static dynamic SetCrossover(dynamic optuna, string crossover)
        {
            switch (crossover)
            {
                case "Uniform":
                    return optuna.samplers.nsgaii.UniformCrossover();
                case "BLXAlpha":
                    return optuna.samplers.nsgaii.BLXAlphaCrossover();
                case "SPX":
                    return optuna.samplers.nsgaii.SPXCrossover();
                case "SBX":
                    return optuna.samplers.nsgaii.SBXCrossover();
                case "VSBX":
                    return optuna.samplers.nsgaii.VSBXCrossover();
                case "UNDX":
                    return optuna.samplers.nsgaii.UNDXCrossover();
                default:
                    throw new ArgumentException("Unexpected crossover setting");
            }
        }

        internal static dynamic TPE(dynamic optuna, TunnySettings settings, bool hasConstraints)
        {
            Tpe tpe = settings.Optimize.Sampler.Tpe;
            return optuna.samplers.TPESampler(
                seed: tpe.Seed,
                consider_prior: tpe.ConsiderPrior,
                prior_weight: 1.0,
                consider_magic_clip: tpe.ConsiderMagicClip,
                consider_endpoints: tpe.ConsiderEndpoints,
                n_startup_trials: tpe.NStartupTrials,
                n_ei_candidates: tpe.NEICandidates,
                multivariate: tpe.Multivariate,
                group: tpe.Group,
                warn_independent_sampling: tpe.WarnIndependentSampling,
                constant_liar: tpe.ConstantLiar,
                constraints_func: hasConstraints ? ConstraintFunc() : null
            );
        }

        internal static dynamic BoTorch(dynamic optuna, TunnySettings settings, bool hasConstraints)
        {
            BoTorch boTorch = settings.Optimize.Sampler.BoTorch;
            return optuna.integration.BoTorchSampler(
                n_startup_trials: boTorch.NStartupTrials,
                constraints_func: hasConstraints ? ConstraintFunc() : null
            );
        }


        internal static dynamic QMC(dynamic optuna, TunnySettings settings)
        {
            QuasiMonteCarlo qmc = settings.Optimize.Sampler.QMC;
            return optuna.samplers.QMCSampler(
                qmc_type: qmc.QmcType,
                scramble: qmc.Scramble,
                seed: qmc.Seed,
                // warn_asynchronous_seeding: qmc.WarnAsynchronousSeeding,
                warn_independent_sampling: qmc.WarnIndependentSampling
            );
        }

        internal static dynamic ConstraintFunc()
        {
            PyModule ps = Py.CreateScope();
            ps.Exec(
                "def constraints(trial):\n" +
                "  return trial.user_attrs[\"Constraint\"]\n"
            );
            return ps.Get("constraints");
        }
    }
}
