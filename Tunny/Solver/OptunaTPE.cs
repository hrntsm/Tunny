using System;
using System.Collections.Generic;
using System.Linq;

using Tunny.Util;

using Python.Runtime;
using System.Windows.Forms;

namespace Tunny.Solver
{
    public class OptunaTPE : ISolver
    {
        public double[] XOpt { get; private set; }
        public double[] FxOpt { get; private set; }

        private readonly string _componentFolder;
        private readonly Dictionary<string, Dictionary<string, double>> _presets = new Dictionary<string, Dictionary<string, double>>();

        public OptunaTPE(string componentFolder)
        {
            _componentFolder = componentFolder;
            string envPath = componentFolder + @"\Python\python310.dll";
            Environment.SetEnvironmentVariable("PYTHONNET_PYDLL", envPath, EnvironmentVariableTarget.Process);
        }

        public bool RunSolver(
            List<Variable> variables, Func<IList<decimal>, int, List<double>> evaluate,
            string preset, Dictionary<string, object> settings, string installFolder, string documentPath)
        {
            int dVar = variables.Count;
            var lb = new double[dVar];
            var ub = new double[dVar];
            var integer = new bool[dVar];
            var nickName = new string[dVar];

            for (var i = 0; i < dVar; i++)
            {
                lb[i] = Convert.ToDouble(variables[i].LowerBond);
                ub[i] = Convert.ToDouble(variables[i].UpperBond);
                integer[i] = variables[i].Integer;
                nickName[i] = variables[i].NickName;
            }

            double[] Eval(double[] x, int progress)
            {
                List<decimal> decimals = x.Select(Convert.ToDecimal).ToList();
                return evaluate(decimals, progress).ToArray();
            }

            try
            {
                var tpe = new OptunaTPEAlgorithm(lb, ub, nickName, settings, Eval);
                tpe.Solve();
                XOpt = tpe.Get_XOptimum();
                FxOpt = tpe.Get_fxOptimum();

                MessageBox.Show("Solver completed successfully.");
                return true;
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
                return false;
            }
        }

        public string GetErrorMessage() => "";
        public double[] Get_XOptimum => XOpt;
        public IEnumerable<string> GetPresetNames() => _presets.Keys;

        public void ShowResult(string visualize, string studyName)
        {
            var strage = "sqlite:///" + _componentFolder + "/tunny.db";
            PythonEngine.Initialize();
            using (Py.GIL())
            {
                dynamic vis;
                dynamic optuna = Py.Import("optuna");
                dynamic study = optuna.create_study(storage: strage, study_name: studyName, load_if_exists: true);
                switch (visualize)
                {
                    case "contour":
                        vis = optuna.visualization.plot_contour(study);
                        break;
                    case "edf":
                        vis = optuna.visualization.plot_edf(study);
                        break;
                    case "intermediate values":
                        vis = optuna.visualization.plot_intermediate_values(study);
                        break;
                    case "optimization history":
                        vis = optuna.visualization.plot_optimization_history(study);
                        break;
                    case "parallel coordinate":
                        vis = optuna.visualization.plot_parallel_coordinate(study);
                        break;
                    case "param importances":
                        vis = optuna.visualization.plot_param_importances(study);
                        break;
                    case "pareto front":
                        vis = optuna.visualization.plot_pareto_front(study);
                        break;
                    case "slice":
                        vis = optuna.visualization.plot_slice(study);
                        break;
                    default:
                        vis = optuna.visualization.plot_optimization_history(study);
                        break;
                }
                vis.show();
            }
            PythonEngine.Shutdown();
        }
    }

    public class OptunaTPEAlgorithm
    {
        private double[] Lb { get; set; }
        private double[] Ub { get; set; }
        private string[] NickName { get; set; }
        private Dictionary<string, object> Settings { get; set; }
        private Func<double[], int, double[]> EvalFunc { get; set; }
        private double[] XOpt { get; set; }
        private double[] FxOpt { get; set; }

        public OptunaTPEAlgorithm(double[] lb, double[] ub, string[] nickName, Dictionary<string, object> settings, Func<double[], int, double[]> evalFunc)
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
            int nTrials = (int)Settings["nTrials"];
            bool loadIfExists = (bool)Settings["loadIfExists"];
            string studyName = (string)Settings["studyName"];
            var strage = "sqlite:///" + (string)Settings["storage"] + "/tunny.db";

            PythonEngine.Initialize();
            using (Py.GIL())
            {
                dynamic optuna = Py.Import("optuna");
                dynamic study = optuna.create_study(storage: strage, study_name: studyName, load_if_exists: loadIfExists);

                for (int i = 0; i < nTrials; i++)
                {
                    int progress = i * 100 / nTrials;
                    double[] xTest = new double[n];
                    dynamic trial = study.ask();
                    for (int j = 0; j < n; j++)
                    {
                        xTest[j] = trial.suggest_uniform(NickName[j], Lb[j], Ub[j]);
                    }
                    double fxTest = EvalFunc(xTest, progress)[0];
                    study.tell(trial, fxTest);

                    if (double.IsNaN(fxTest))
                    {
                        return;
                    }
                }

                XOpt = (double[])study.best_params.values();
                FxOpt = new[] { (double)study.best_value };
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