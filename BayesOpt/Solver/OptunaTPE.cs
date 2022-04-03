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
            List<Variable> variables, Func<IList<decimal>, List<double>> evaluate,
            string preset, string expertsettings, string installFolder, string documentPath)
        {
            var settings = _presets[preset];
            var dVar = variables.Count;
            var lb = new double[dVar];
            var ub = new double[dVar];
            var integer = new bool[dVar];

            for (var i = 0; i < dVar; i++)
            {
                lb[i] = Convert.ToDouble(variables[i].LowerBond);
                ub[i] = Convert.ToDouble(variables[i].UpperBond);
                integer[i] = variables[i].Integer;
            }

            double[] Eval(double[] x)
            {
                List<decimal> decimals = x.Select(Convert.ToDecimal).ToList();
                return evaluate(decimals).ToArray();
            }

            try
            {
                var iterMax = (int)settings["itermax"];
                var tpe = new OptunaTPEAlgorithm(lb, ub, iterMax, Eval);
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
        public double[] Lb { get; private set; }
        public double[] Ub { get; private set; }
        public int IterMax { get; private set; }
        public Func<double[], double[]> EvalFunc { get; private set; }
        public double[] XOpt { get; private set; }
        public double[] FxOpt { get; private set; }

        public OptunaTPEAlgorithm(double[] lb, double[] ub, int iterMax, Func<double[], double[]> evalFunc)
        {
            Lb = lb;
            Ub = ub;
            IterMax = iterMax;
            EvalFunc = evalFunc;
        }

        public void Solve()
        {
            int n = Lb.Length;

            using (Py.GIL())
            {
                dynamic optuna = Py.Import("optuna");
                dynamic study = optuna.create_study();
                int nTrials = IterMax;

                for (int i = 0; i < nTrials; i++)
                {
                    double[] xTest = new double[n];
                    dynamic trial = study.ask();
                    for (int j = 0; j < n; j++)
                    {
                        xTest[j] = trial.suggest_uniform(j, Lb[j], Ub[j]);
                    }
                    double fxTest = EvalFunc(xTest)[0];
                    study.tell(trial, fxTest);

                    if (double.IsNaN(fxTest))
                    {
                        return;
                    }
                }
                dynamic vis = optuna.visualization.plot_optimization_history(study);
                vis.show();
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