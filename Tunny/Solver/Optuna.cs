using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using Python.Runtime;

using Tunny.Optimization;
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
            List<Variable> variables,
            Func<IList<decimal>, int, EvaluatedGHResult> evaluate,
            string preset,
            Dictionary<string, object> settings, string installFolder, string documentPath)
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

            EvaluatedGHResult Eval(double[] x, int progress)
            {
                List<decimal> decimals = x.Select(Convert.ToDecimal).ToList();
                return evaluate(decimals, progress);
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

        public void ShowResultVisualize(string visualize, string studyName)
        {
            var strage = "sqlite:///" + _componentFolder + "/Tunny_Opt_Result.db";
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

        public string[] GetResultDraco(int[] num, string studyName)
        {
            var strage = "sqlite:///" + _componentFolder + "/Tunny_Opt_Result.db";
            var resultDraco = new string[num.Length];
            PythonEngine.Initialize();
            using (Py.GIL())
            {
                dynamic optuna = Py.Import("optuna");
                dynamic study = optuna.load_study(storage: strage, study_name: studyName);
                for (var i = 0; i < num.Length; i++)
                {
                    resultDraco[i] = (string)study.trials[num[i]].user_attrs["geometry"];
                }
            }
            PythonEngine.Shutdown();

            return resultDraco;
        }
    }
}