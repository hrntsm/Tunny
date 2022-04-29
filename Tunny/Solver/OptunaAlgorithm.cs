using System;
using System.Collections.Generic;
using System.Text;

using Python.Runtime;

using Tunny.Optimization;

namespace Tunny.Solver
{
    public class OptunaAlgorithm
    {
        private double[] Lb { get; set; }
        private double[] Ub { get; set; }
        private string[] VarNickName { get; set; }
        private string[] ObjNickName { get; set; }
        private Dictionary<string, object> Settings { get; set; }
        private Func<double[], int, EvaluatedGHResult> EvalFunc { get; set; }
        private double[] XOpt { get; set; }
        private double[] FxOpt { get; set; }

        public OptunaAlgorithm(
            double[] lb, double[] ub, string[] varNickName, string[] objNickName,
            Dictionary<string, object> settings,
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

                var name = new StringBuilder();
                foreach (var objName in ObjNickName)
                {
                    name.Append(objName + ",");
                }
                name.Remove(name.Length - 1, 1);
                study.set_user_attr("objective_names", name.ToString());

                var xTest = new double[n];
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
                        for (int j = 0; j < n; j++)
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
                    trial.set_user_attr("geometry", result.ModelDraco);
                    study.tell(trial, result.ObjectiveValues.ToArray());
                }

                if (nObjective == 1)
                {
                    var values = (double[])study.best_params.values();
                    var keys = (string[])study.best_params.keys();
                    var opt = new double[VarNickName.Length];

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