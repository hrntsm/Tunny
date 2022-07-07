using System;
using System.Collections.Generic;

using Tunny.Settings;
using Tunny.Util;

namespace Tunny.Solver.Optuna
{
    public class Sampler
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
                restart_strategy: cmaEs.RestartStrategy,
                inc_popsize: cmaEs.IncPopsize,
                use_separable_cma: cmaEs.UseSeparableCma
            );
        }

        internal static dynamic Grid(dynamic optuna, List<Variable> variables, ref int nTrials)
        {
            var searchSpace = new Dictionary<string, List<double>>();
            for (int i = 0; i < variables.Count; i++)
            {
                var numSpace = new List<double>();
                for (int j = 0; j < nTrials; j++)
                {
                    numSpace.Add(variables[i].LowerBond + (variables[i].UpperBond - variables[i].LowerBond) * j / (nTrials - 1));
                }
                searchSpace.Add(variables[i].NickName, numSpace);
            }
            nTrials = (int)Math.Pow(nTrials, variables.Count);
            return optuna.samplers.GridSampler(searchSpace);
        }

        internal static dynamic NSGAII(dynamic optuna, TunnySettings settings)
        {
            NSGAII nsga2 = settings.Optimize.Sampler.NsgaII;
            return optuna.samplers.NSGAIISampler(
                population_size: nsga2.PopulationSize,
                mutation_prob: nsga2.MutationProb,
                crossover_prob: nsga2.CrossoverProb,
                swapping_prob: nsga2.SwappingProb,
                seed: nsga2.Seed
            );
        }

        internal static dynamic TPE(dynamic optuna, TunnySettings settings)
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
                constant_liar: tpe.ConstantLiar
            );
        }
    }
}
