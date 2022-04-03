using System;
using System.Collections.Generic;
using System.Linq;

using BayesOpt.Util;

using Python.Runtime;

namespace BayesOpt.Solver
{
    public class OptunaTPE : ISolver
    {
        public double[] XOpt { get; private set; }
        public double[] FxOpt { get; private set; }

        private readonly Dictionary<string, Dictionary<string, double>> _presets = new Dictionary<string, Dictionary<string, double>>();

        public OptunaTPE()
        {
            var tpe = new Dictionary<string, double>
            {
                {"itermax", 100}
            };

            _presets.Add("OptunaTPE", tpe);
        }

        public bool RunSolver(
            List<Variable> variables, Func<IList<decimal>, int, List<double>> evaluate,
            string preset, Dictionary<string, object> settings, string installFolder, string documentPath)
        {
            int dVar = variables.Count;
            var lb = new double[dVar];
            var ub = new double[dVar];
            var integer = new bool[dVar];

            for (var i = 0; i < dVar; i++)
            {
                lb[i] = Convert.ToDouble(variables[i].LowerBond);
                ub[i] = Convert.ToDouble(variables[i].UpperBond);
                integer[i] = variables[i].Integer;
            }

            double[] Eval(double[] x, int progress)
            {
                List<decimal> decimals = x.Select(Convert.ToDecimal).ToList();
                return evaluate(decimals, progress).ToArray();
            }

            try
            {
                var tpe = new OptunaTPEAlgorithm(lb, ub, settings, Eval);
                tpe.Solve();
                XOpt = tpe.Get_XOptimum();
                FxOpt = tpe.Get_fxOptimum();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public string GetErrorMessage() => "";
        public double[] Get_XOptimum => XOpt;
        public IEnumerable<string> GetPresetNames() => _presets.Keys;
    }

    public class OptunaTPEAlgorithm
    {
        private double[] Lb { get; set; }
        private double[] Ub { get; set; }
        private Dictionary<string, object> Settings { get; set; }
        private Func<double[], int, double[]> EvalFunc { get; set; }
        private double[] XOpt { get; set; }
        private double[] FxOpt { get; set; }

        public OptunaTPEAlgorithm(double[] lb, double[] ub, Dictionary<string, object> settings, Func<double[], int, double[]> evalFunc)
        {
            Lb = lb;
            Ub = ub;
            Settings = settings;
            EvalFunc = evalFunc;
        }

        public void Solve()
        {
            int n = Lb.Length;
            int nTrials = (int)Settings["nTrials"];
            bool loadIfExists = (bool)Settings["loadIfExists"];
            string studyName = (string)Settings["studyName"];

            using (Py.GIL())
            {
                dynamic optuna = Py.Import("optuna");
                dynamic study = optuna.create_study(storage: "sqlite:///grasshopper_opt.db", study_name: studyName, load_if_exists: loadIfExists);

                for (int i = 0; i < nTrials; i++)
                {
                    int progress = i * 100 / nTrials;
                    double[] xTest = new double[n];
                    dynamic trial = study.ask();
                    for (int j = 0; j < n; j++)
                    {
                        xTest[j] = trial.suggest_uniform(j, Lb[j], Ub[j]);
                    }
                    double fxTest = EvalFunc(xTest, progress)[0];
                    study.tell(trial, fxTest);

                    if (double.IsNaN(fxTest))
                    {
                        return;
                    }
                }
                dynamic vis = optuna.visualization.plot_optimization_history(study);
                vis.show();

                XOpt = (double[])study.best_params.values();
                FxOpt = new[] { (double)study.best_value };
            }
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