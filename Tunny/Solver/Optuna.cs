using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using Python.Runtime;

using Tunny.UI;
using Tunny.Util;

namespace Tunny.Solver
{
    public class Optuna : ISolver
    {
        public double[] XOpt { get; private set; }
        public double[] FxOpt { get; private set; }

        private readonly string _componentFolder;
        private readonly Dictionary<string, Dictionary<string, double>> _presets = new Dictionary<string, Dictionary<string, double>>();

        public Optuna(string componentFolder)
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
                var tpe = new OptunaAlgorithm(lb, ub, nickName, settings, Eval);
                tpe.Solve();
                XOpt = tpe.Get_XOptimum();
                FxOpt = tpe.Get_fxOptimum();

                if (FxOpt.Length == 1)
                {
                    TunnyMessageBox.Show("Solver completed successfully.", "Tunny");
                }
                else
                {
                    TunnyMessageBox.Show(
                        "Solver completed successfully.\n\n" +
                        "**Multi objective optimization is experimental.**\n\n" +
                        "NumberSliders are not update to best value." +
                        "Please see pareto front graph & update these values manually.", "Tunny");
                }
                return true;
            }
            catch (Exception e)
            {
                TunnyMessageBox.Show(
                    "Tunny runtime error:\n" +
                    "Please send below message (& gh file if possible) to Tunny support.\n\n" +
                    "\" " + e.Message + " \"", "Tunny",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                dynamic study = optuna.load_study(storage: strage, study_name: studyName);
                switch (visualize)
                {
                    case "contour":
                        vis = optuna.visualization.plot_contour(study);
                        vis.show();
                        break;
                    case "EDF":
                        vis = optuna.visualization.plot_edf(study);
                        vis.show();
                        break;
                    case "intermediate values":
                        vis = optuna.visualization.plot_intermediate_values(study);
                        vis.show();
                        break;
                    case "optimization history":
                        vis = optuna.visualization.plot_optimization_history(study);
                        vis.show();
                        break;
                    case "parallel coordinate":
                        vis = optuna.visualization.plot_parallel_coordinate(study);
                        vis.show();
                        break;
                    case "param importances":
                        vis = optuna.visualization.plot_param_importances(study);
                        vis.show();
                        break;
                    case "pareto front":
                        vis = optuna.visualization.plot_pareto_front(study);
                        vis.show();
                        break;
                    case "slice":
                        vis = optuna.visualization.plot_slice(study);
                        vis.show();
                        break;
                    default:
                        TunnyMessageBox.Show("This visualization type is not supported in this study case.", "Tunny");
                        break;
                }
            }
            PythonEngine.Shutdown();
        }
    }

    public class OptunaAlgorithm
    {
        private double[] Lb { get; set; }
        private double[] Ub { get; set; }
        private string[] NickName { get; set; }
        private Dictionary<string, object> Settings { get; set; }
        private Func<double[], int, double[]> EvalFunc { get; set; }
        private double[] XOpt { get; set; }
        private double[] FxOpt { get; set; }

        public OptunaAlgorithm(double[] lb, double[] ub, string[] nickName, Dictionary<string, object> settings, Func<double[], int, double[]> evalFunc)
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
            string storage = "sqlite:///" + (string)Settings["storage"] + "/tunny.db";

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

                for (int i = 0; i < nTrials; i++)
                {
                    int progress = i * 100 / nTrials;
                    double[] xTest = new double[n];
                    dynamic trial = study.ask();
                    for (int j = 0; j < n; j++)
                    {
                        xTest[j] = trial.suggest_uniform(NickName[j], Lb[j], Ub[j]);
                    }
                    double[] fxTest = EvalFunc(xTest, progress);
                    study.tell(trial, fxTest);
                }

                if (nObjective == 1)
                {
                    XOpt = (double[])study.best_params.values();
                    FxOpt = new[] { (double)study.best_value };
                }
                else
                {
                    XOpt = Lb;
                    FxOpt = new double[nObjective];
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