using System;
using System.Collections.Generic;

using Python.Runtime;

using Tunny.Optimization;

namespace Tunny.Solver
{
    public class OptunaAlgorithm
    {
        private double[] Lb { get; set; }
        private double[] Ub { get; set; }
        private string[] NickName { get; set; }
        private Dictionary<string, object> Settings { get; set; }
        private Func<double[], int, EvaluatedGHResult> EvalFunc { get; set; }
        private double[] XOpt { get; set; }
        private double[] FxOpt { get; set; }

        public OptunaAlgorithm(
            double[] lb, double[] ub, string[] nickName,
            Dictionary<string, object> settings,
            Func<double[], int, EvaluatedGHResult> evalFunc)
        {
            Lb = lb;
            Ub = ub;
            NickName = nickName;
            Settings = settings;
            EvalFunc = evalFunc;
        }

        public void Solve()
        {
            int n = Lb.Length;
            string samplerType = (string)Settings["samplerType"];
            int nTrials = (int)Settings["nTrials"];
            bool loadIfExists = (bool)Settings["loadIfExists"];
            string studyName = (string)Settings["studyName"];
            string storage = "sqlite:///" + (string)Settings["storage"] + "/Tunny_Opt_Result.db";

            int nObjective = (int)Settings["nObjective"];
            string[] directions = new string[nObjective];
            for (int i = 0; i < nObjective; i++)
            {
                directions[i] = "minimize";
            }

            PythonEngine.Initialize();
            using (Py.GIL())
            {
                dynamic optuna = Py.Import("optuna");
                dynamic sampler;
                switch (samplerType)
                {
                    case "Random":
                        sampler = optuna.samplers.RandomSampler();
                        break;
                    case "CMA-ES":
                        sampler = optuna.samplers.CmaEsSampler();
                        break;
                    case "NSGA-II":
                        sampler = optuna.samplers.NSGAIISampler();
                        break;
                    default:
                        sampler = optuna.samplers.TPESampler();
                        break;
                }

                dynamic study = optuna.create_study(
                    sampler: sampler,
                    storage: storage,
                    study_name: studyName,
                    load_if_exists: loadIfExists,
                    directions: directions
                );

                var xTest = new double[n];
                var result = new EvaluatedGHResult();
                for (int i = 0; i < nTrials; i++)
                {
                    int progress = i * 100 / nTrials;
                    dynamic trial = study.ask();
                    for (int j = 0; j < n; j++)
                    {
                        xTest[j] = trial.suggest_uniform(NickName[j], Lb[j], Ub[j]);
                    }
                    result = EvalFunc(xTest, progress);
                    trial.set_user_attr("geometry", result.ModelDraco);
                    study.tell(trial, result.ObjectiveValues.ToArray());
                }

                if (nObjective == 1)
                {
                    XOpt = (double[])study.best_params.values();
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