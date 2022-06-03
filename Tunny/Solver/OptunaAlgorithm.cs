using System;
using System.Collections.Generic;
using System.Text;

using Python.Runtime;

using Tunny.Optimization;
using Tunny.Settings;

namespace Tunny.Solver
{
    public class OptunaAlgorithm
    {
        private double[] Lb { get; set; }
        private double[] Ub { get; set; }
        private string[] VarNickName { get; set; }
        private string[] ObjNickName { get; set; }
        private TunnySettings Settings { get; set; }
        private Func<double[], int, EvaluatedGHResult> EvalFunc { get; set; }
        private double[] XOpt { get; set; }
        private double[] FxOpt { get; set; }

        public OptunaAlgorithm(
            double[] lb, double[] ub, string[] varNickName, string[] objNickName,
            TunnySettings settings,
            Func<double[], int, EvaluatedGHResult> evalFunc)
        {
            Lb = lb;
            Ub = ub;
            VarNickName = varNickName;
            ObjNickName = objNickName;
            Settings = settings;
            EvalFunc = evalFunc;
        }

        public void Solve()
        {
            int variableCount = Lb.Length;
            int samplerType = Settings.Optimize.SelectSampler;
            int nTrials = Settings.Optimize.NumberOfTrials;
            int nObjective = ObjNickName.Length;
            string[] directions = new string[nObjective];
            for (int i = 0; i < nObjective; i++)
            {
                directions[i] = "minimize";
            }

            PythonEngine.Initialize();
            using (Py.GIL())
            {
                dynamic optuna = Py.Import("optuna");
                dynamic sampler = SetSamplerSettings(variableCount, samplerType, ref nTrials, optuna);

                dynamic study = optuna.create_study(
                    sampler: sampler,
                    directions: directions,
                    storage: "sqlite:///" + Settings.Storage,
                    study_name: Settings.StudyName,
                    load_if_exists: Settings.Optimize.LoadExistStudy
                );

                var name = new StringBuilder();
                foreach (string objName in ObjNickName)
                {
                    name.Append(objName + ",");
                }
                name.Remove(name.Length - 1, 1);
                study.set_user_attr("objective_names", name.ToString());

                double[] xTest = new double[variableCount];
                var result = new EvaluatedGHResult();
                for (int i = 0; i < nTrials; i++)
                {
                    int progress = i * 100 / nTrials;
                    dynamic trial = study.ask();

                    //TODO: Is this the correct way to handle the case of null?
                    //Other than TPE, the value is returned at random when retrying, so the value will be anything but null.
                    //TPEs, on the other hand, search for a specific location determined by GP,
                    //so the value tends to remain the same even after retries and there is no way to get out.
                    int nullCount = 0;
                    while (nullCount < 10)
                    {
                        for (int j = 0; j < variableCount; j++)
                        {
                            xTest[j] = trial.suggest_uniform(VarNickName[j], Lb[j], Ub[j]);
                        }
                        result = EvalFunc(xTest, progress);

                        if (result.ObjectiveValues.Contains(double.NaN))
                        {
                            trial = study.ask();
                            nullCount++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    var pyJson = new PyList();
                    foreach (string json in result.GeometryJson)
                    {
                        pyJson.Append(new PyString(json));
                    }
                    trial.set_user_attr("geometry", pyJson);
                    try
                    {
                        study.tell(trial, result.ObjectiveValues.ToArray());
                    }
                    catch
                    {
                        break;
                    }
                }

                if (nObjective == 1)
                {
                    double[] values = (double[])study.best_params.values();
                    string[] keys = (string[])study.best_params.keys();
                    double[] opt = new double[VarNickName.Length];

                    for (int i = 0; i < VarNickName.Length; i++)
                    {
                        for (int j = 0; j < keys.Length; j++)
                        {
                            if (keys[j] == VarNickName[i])
                            {
                                opt[i] = values[j];
                            }
                        }
                    }
                    XOpt = opt;
                    FxOpt = new[] { (double)study.best_value };
                }
                else
                {
                    XOpt = xTest;
                    FxOpt = result.ObjectiveValues.ToArray();
                }
            }
            PythonEngine.Shutdown();
        }

        private dynamic SetSamplerSettings(int n, int samplerType, ref int nTrials, dynamic optuna)
        {
            dynamic sampler;
            switch (samplerType)
            {
                case 0:
                    sampler = SetTPESamplerSettings(optuna);
                    break;
                case 1:
                    sampler = SetNSGAIISamplerSettings(optuna);
                    break;
                case 2:
                    sampler = SetCmaEsSamplerSettings(optuna);
                    break;
                case 3:
                    sampler = SetRandomSamplerSettings(optuna);
                    break;
                case 4:
                    sampler = SetGridSamplerSettings(n, ref nTrials, optuna);
                    break;
                default:
                    throw new ArgumentException("Unknown sampler type");
            }
            return sampler;
        }

        private dynamic SetRandomSamplerSettings(dynamic optuna)
        {
            Settings.Random random = Settings.Optimize.Sampler.Random;
            return optuna.samplers.RandomSampler(
                seed: random.Seed
            );
        }

        private dynamic SetCmaEsSamplerSettings(dynamic optuna)
        {
            CmaEs cmaEs = Settings.Optimize.Sampler.CmaEs;
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

        private dynamic SetGridSamplerSettings(int n, ref int nTrials, dynamic optuna)
        {
            var searchSpace = new Dictionary<string, List<double>>();
            for (int i = 0; i < n; i++)
            {
                var numSpace = new List<double>();
                for (int j = 0; j < nTrials; j++)
                {
                    numSpace.Add(Lb[i] + ((Ub[i] - Lb[i]) * j / (nTrials - 1)));
                }
                searchSpace.Add(VarNickName[i], numSpace);
            }
            nTrials = (int)Math.Pow(nTrials, n);
            return optuna.samplers.GridSampler(searchSpace);
        }

        private dynamic SetNSGAIISamplerSettings(dynamic optuna)
        {
            NSGAII nsga2 = Settings.Optimize.Sampler.NsgaII;
            return optuna.samplers.NSGAIISampler(
                population_size: nsga2.PopulationSize,
                mutation_prob: nsga2.MutationProb,
                crossover_prob: nsga2.CrossoverProb,
                swapping_prob: nsga2.SwappingProb,
                seed: nsga2.Seed
            );
        }

        private dynamic SetTPESamplerSettings(dynamic optuna)
        {
            Tpe tpe = Settings.Optimize.Sampler.Tpe;
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

        public double[] Get_XOptimum()
        {
            return XOpt;
        }

        public double[] Get_fxOptimum()
        {
            return FxOpt;
        }
    }
}
