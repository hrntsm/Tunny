using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

using Grasshopper.Kernel;

using Python.Runtime;

using Tunny.Optimization;
using Tunny.Settings;
using Tunny.UI;
using Tunny.Util;

namespace Tunny.Solver.Optuna
{
    public class Optuna
    {
        public double[] XOpt { get; private set; }
        private readonly string _componentFolder;
        private readonly bool _hasConstraint;
        private readonly TunnySettings _settings;

        public Optuna(string componentFolder, TunnySettings settings, bool hasConstraint)
        {
            _componentFolder = componentFolder;
            _settings = settings;
            _hasConstraint = hasConstraint;
            string envPath = PythonInstaller.GetEmbeddedPythonPath() + @"\python310.dll";
            Environment.SetEnvironmentVariable("PYTHONNET_PYDLL", envPath, EnvironmentVariableTarget.Process);
        }

        public bool RunSolver(
            List<Variable> variables,
            IEnumerable<IGH_Param> objectives,
            Func<IList<decimal>, int, EvaluatedGHResult> evaluate)
        {
            string[] objNickName = objectives.Select(x => x.NickName).ToArray();

            EvaluatedGHResult Eval(double[] x, int progress)
            {
                var decimals = x.Select(Convert.ToDecimal).ToList();
                return evaluate(decimals, progress);
            }

            try
            {
                var optimize = new Algorithm(variables, _hasConstraint, objNickName, _settings, Eval);
                optimize.Solve();
                XOpt = optimize.GetXOptimum();

                ShowEndMessages(optimize);
                return true;
            }
            catch (Exception e)
            {
                ShowErrorMessages(e);
                return false;
            }
        }

        private static void ShowEndMessages(Algorithm optimize)
        {
            switch (optimize.EndState)
            {
                case EndState.Timeout:
                    TunnyMessageBox.Show("Solver completed successfully.\n\nThe specified time has elapsed.", "Tunny");
                    break;
                case EndState.AllTrialCompleted:
                    TunnyMessageBox.Show("Solver completed successfully.\n\nThe specified number of trials has been completed.", "Tunny");
                    break;
                case EndState.StoppedByUser:
                    TunnyMessageBox.Show("Solver completed successfully.\n\nThe user stopped the solver.", "Tunny");
                    break;
                case EndState.DirectionNumNotMatch:
                    TunnyMessageBox.Show("Solver error.\n\nThe number of Objective in the existing Study does not match the one that you tried to run; Match the number of objective, or change the \"Study Name\".", "Tunny", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                case EndState.UseExitStudyWithoutLoading:
                    TunnyMessageBox.Show("Solver error.\n\n\"Load if study file exists\" was false even though the same \"Study Name\" exists. Please change the name or set it to true.", "Tunny", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                default:
                    TunnyMessageBox.Show("Solver error.", "Tunny");
                    break;
            }
        }

        private static void ShowErrorMessages(Exception e)
        {
            TunnyMessageBox.Show(
                "Tunny runtime error:\n" +
                "Please send below message (& gh file if possible) to Tunny support.\n" +
                "If this error occurs, the Tunny solver will not work after this unless Rhino is restarted.\n\n" +
                "\" " + e.Message + " \"", "Tunny",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private static dynamic LoadStudy(dynamic optuna, string storage, string studyName)
        {
            try
            {
                return optuna.load_study(storage: storage, study_name: studyName);
            }
            catch (Exception e)
            {
                TunnyMessageBox.Show(e.Message, "Tunny", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

        }

        public void Plot(string visualize, string studyName, PlotType pType)
        {
            string storage = "sqlite:///" + _settings.Storage;
            PythonEngine.Initialize();
            using (Py.GIL())
            {
                dynamic optuna = Py.Import("optuna");
                dynamic study = LoadStudy(optuna, storage, studyName);
                if (study == null)
                {
                    return;
                }

                string[] nickNames = ((string)study.user_attrs["objective_names"]).Split(',');
                try
                {
                    dynamic fig = CreateFigure(optuna, visualize, study, nickNames);
                    FigureActions(pType, fig, visualize);
                }
                catch (Exception)
                {
                    TunnyMessageBox.Show("This visualization type is not supported in this study case.", "Tunny", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            PythonEngine.Shutdown();
        }

        private static void SaveFigure(dynamic fig, string name)
        {
            var sfd = new SaveFileDialog
            {
                FileName = name + ".html",
                Filter = "HTML file(*.html)|*.html",
                Title = "Save"
            };
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                fig.write_html(sfd.FileName);
            }
        }

        private dynamic CreateFigure(dynamic optuna, string visualize, dynamic study, string[] nickNames)
        {
            switch (visualize)
            {
                case "contour":
                    return optuna.visualization.plot_contour(study, target_name: nickNames[0]);
                case "EDF":
                    return optuna.visualization.plot_edf(study, target_name: nickNames[0]);
                case "intermediate values":
                    return optuna.visualization.plot_intermediate_values(study);
                case "optimization history":
                    return optuna.visualization.plot_optimization_history(study, target_name: nickNames[0]);
                case "parallel coordinate":
                    return optuna.visualization.plot_parallel_coordinate(study, target_name: nickNames[0]);
                case "param importances":
                    return optuna.visualization.plot_param_importances(study, target_name: nickNames[0]);
                case "pareto front":
                    dynamic fig = optuna.visualization.plot_pareto_front(study, target_names: nickNames, constraints_func: _hasConstraint ? Sampler.ConstraintFunc() : null);
                    return TruncateParetoFrontPlotHover(fig, study);
                case "slice":
                    return optuna.visualization.plot_slice(study, target_name: nickNames[0]);
                case "hypervolume":
                    return Hypervolume.CreateFigure(optuna, study);
                default:
                    TunnyMessageBox.Show("This visualization type is not supported in this study case.", "Tunny");
                    return null;
            }
        }

        private static dynamic TruncateParetoFrontPlotHover(dynamic fig, dynamic study)
        {
            PyModule ps = Py.CreateScope();
            ps.Exec(
                "def truncate(fig, study):\n" +
                "    import json\n" +
                "    user_attr = study.trials[0].user_attrs\n" +
                "    has_geometry = 'Geometry' in user_attr\n" +
                "    if has_geometry == False:\n" +
                "        return fig\n" +
                "    for scatter_id in range(len(fig.data)):\n" +
                "        new_texts = []\n" +
                "        for i, original_label in enumerate(fig.data[scatter_id]['text']):\n" +
                "            json_label = json.loads(original_label.replace('<br>', '\\n'))\n" +
                "            json_label['user_attrs'].pop('Geometry')\n" +
                "            param_len = len(json_label['params'])\n" +
                "            while len(json_label['params']) > 10:\n" +
                "                keys = list(json_label['params'].keys())\n" +
                "                json_label['params'].pop(keys.pop())\n" +
                "            if param_len > 10:\n" +
                "                json_label['params']['__Omit_values__'] = 'True'\n" +
                "            new_texts.append(json.dumps(json_label, indent=2).replace('\\n', '<br>'))\n" +
                "        fig.data[scatter_id]['text'] = new_texts\n" +
                "    return fig\n"
            );
            dynamic truncate = ps.Get("truncate");
            return truncate(fig, study);
        }

        public void ClusteringPlot(string studyName, int numCluster, PlotType pType)
        {
            string storage = "sqlite:///" + _settings.Storage;
            PythonEngine.Initialize();
            using (Py.GIL())
            {
                dynamic optuna = Py.Import("optuna");
                dynamic study = LoadStudy(optuna, storage, studyName);
                if (study == null)
                {
                    return;
                }

                string[] nickNames = ((string)study.user_attrs["objective_names"]).Split(',');
                if (nickNames.Length == 1)
                {
                    TunnyMessageBox.Show("Clustering Error\n\nClustering is for multi-objective optimization only.", "Tunny", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    dynamic fig = CreateClusterFigure(optuna, study, nickNames, numCluster);
                    FigureActions(pType, fig, "cluster");
                }
            }
            PythonEngine.Shutdown();
        }

        private void FigureActions(PlotType pType, dynamic fig, string name)
        {
            if (fig != null && pType == PlotType.Show)
            {
                fig.show();
            }
            else if (fig != null && pType == PlotType.Save)
            {
                SaveFigure(fig, name);
            }
        }

        private dynamic CreateClusterFigure(dynamic optuna, dynamic study, string[] nickNames, int numCluster)
        {
            try
            {
                dynamic fig = optuna.visualization.plot_pareto_front(study, target_names: nickNames, constraints_func: _hasConstraint ? Sampler.ConstraintFunc() : null);
                fig = TruncateParetoFrontPlotHover(fig, study);
                return ClusteringParetoFrontPlot(fig, study, numCluster);
            }
            catch (Exception)
            {
                TunnyMessageBox.Show("Clustering Error", "Tunny", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return null;
        }

        private static dynamic ClusteringParetoFrontPlot(dynamic fig, dynamic study, int numCluster)
        {
            PyModule ps = Py.CreateScope();
            //FIXME: Rewrite to c-sharp code.
            ps.Exec(
                "def clustering(fig, study, num):\n" +
                "    from sklearn.cluster import KMeans\n" +
                "    import optuna\n" +

                "    if 'Constraint' in study.trials[0].user_attrs:\n" +
                "        feasible_trials = []\n" +
                "        infeasible_trials = []\n" +
                "        for trial in study.get_trials(deepcopy=False, states=(optuna.trial.TrialState.COMPLETE,)):\n" +
                "            if all(map(lambda x: x <= 0.0, trial.user_attrs['Constraint'])):\n" +
                "                feasible_trials.append(trial)\n" +
                "            else:\n" +
                "                infeasible_trials.append(trial)\n" +
                "        best_trials = optuna.visualization._pareto_front._get_pareto_front_trials_by_trials(\n" +
                "            feasible_trials, study.directions)\n" +
                "        non_best_trials = optuna.visualization._pareto_front._get_non_pareto_front_trials(\n" +
                "            feasible_trials, best_trials)\n" +
                "    else:\n" +
                "        best_trials = study.best_trials\n" +

                "        non_best_trials = optuna.visualization._pareto_front._get_non_pareto_front_trials(\n" +
                "            study.get_trials(deepcopy=False, states=(optuna.trial.TrialState.COMPLETE,)), best_trials\n" +
                "        )\n" +
                "        infeasible_trials = []\n" +

                "    best_values = [trial.values for trial in best_trials]\n" +
                "    kmeans = KMeans(n_clusters=num).fit(best_values)\n" +
                "    labels = kmeans.labels_\n" +
                "    best_length = len(best_values)\n" +

                "    for scatter_id in range(len(fig.data)):\n" +
                "        color = fig.data[scatter_id]['marker']['color']\n" +
                "        if (len(color) == best_length):\n" +
                "            fig.data[scatter_id]['marker']['color'] = labels\n" +
                "            fig.data[scatter_id]['marker']['colorscale'] = 'rainbow'\n" +
                "            fig.data[scatter_id]['marker']['colorbar']['title'] = '# Cluster'\n" +
                "        elif fig.data[scatter_id]['marker']['color'] == '#cccccc':\n" +
                "            pass\n" +
                "        else:\n" +
                "            new_color = ['#cccccc' for c in color]\n" +
                "            fig.data[scatter_id]['marker']['color'] = new_color\n" +
                "            fig.data[scatter_id]['marker'].pop('colorscale')\n" +
                "            fig.data[scatter_id]['marker'].pop('colorbar')\n" +
                "    return fig\n"
            );
            dynamic clustering = ps.Get("clustering");
            return clustering(fig, study, numCluster);
        }

        public ModelResult[] GetModelResult(int[] resultNum, string studyName, BackgroundWorker worker)
        {
            string storage = "sqlite:///" + _settings.Storage;
            var modelResult = new List<ModelResult>();
            PythonEngine.Initialize();
            using (Py.GIL())
            {
                dynamic optuna = Py.Import("optuna");
                dynamic study;

                try
                {
                    study = optuna.load_study(storage: storage, study_name: studyName);
                }
                catch (Exception e)
                {
                    TunnyMessageBox.Show(e.Message, "Tunny", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return modelResult.ToArray();
                }

                SetTrialsToModelResult(resultNum, modelResult, study, worker);
            }
            PythonEngine.Shutdown();

            return modelResult.ToArray();
        }

        private void SetTrialsToModelResult(int[] resultNum, List<ModelResult> modelResult, dynamic study, BackgroundWorker worker)
        {
            if (resultNum[0] == -1)
            {
                ParatoSolutions(modelResult, study, worker);
            }
            else if (resultNum[0] == -10)
            {
                AllTrials(modelResult, study, worker);
            }
            else
            {
                UseModelNumber(resultNum, modelResult, study, worker);
            }
        }

        private void UseModelNumber(int[] resultNum, List<ModelResult> modelResult, dynamic study, BackgroundWorker worker)
        {
            for (int i = 0; i < resultNum.Length; i++)
            {
                int res = resultNum[i];
                if (OutputLoop.IsForcedStopOutput)
                {
                    break;
                }

                try
                {
                    dynamic trial = study.trials[res];
                    ParseTrial(modelResult, trial);
                }
                catch (Exception e)
                {
                    TunnyMessageBox.Show("Error\n\n" + e.Message, "Tunny", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                worker.ReportProgress(i * 100 / resultNum.Length);
            }
        }

        private void AllTrials(List<ModelResult> modelResult, dynamic study, BackgroundWorker worker)
        {
            var trials = (dynamic[])study.trials;
            for (int i = 0; i < trials.Length; i++)
            {
                dynamic trial = trials[i];
                if (OutputLoop.IsForcedStopOutput)
                {
                    break;
                }
                ParseTrial(modelResult, trial);
                worker.ReportProgress(i * 100 / trials.Length);
            }
        }

        private void ParatoSolutions(List<ModelResult> modelResult, dynamic study, BackgroundWorker worker)
        {
            var bestTrials = (dynamic[])study.best_trials;
            for (int i = 0; i < bestTrials.Length; i++)
            {
                dynamic trial = bestTrials[i];
                bool isFeasible = CheckFeasible(trial);
                if (OutputLoop.IsForcedStopOutput)
                {
                    break;
                }
                if (isFeasible)
                {
                    ParseTrial(modelResult, trial);
                }
                worker.ReportProgress(i * 100 / bestTrials.Length);
            }
        }

        private static bool CheckFeasible(dynamic trial)
        {
            string[] keys = (string[])trial.user_attrs.keys();
            if (keys.Contains("Constraint"))
            {
                double[] constraint = (double[])trial.user_attrs["Constraint"];
                if (constraint.Max() > 0)
                {
                    return false;
                }
            }

            return true;
        }

        private static void ParseTrial(ICollection<ModelResult> modelResult, dynamic trial)
        {
            var trialResult = new ModelResult
            {
                Number = (int)trial.number,
                Variables = ParseVariables(trial),
                Objectives = (double[])trial.values,
                Attributes = ParseAttributes(trial),
            };
            if (trialResult.Objectives != null)
            {
                modelResult.Add(trialResult);
            }
        }

        private static Dictionary<string, double> ParseVariables(dynamic trial)
        {
            var variables = new Dictionary<string, double>();
            double[] values = (double[])trial.@params.values();
            string[] keys = (string[])trial.@params.keys();
            for (int i = 0; i < keys.Length; i++)
            {
                variables.Add(keys[i], values[i]);
            }

            return variables;
        }

        private static Dictionary<string, List<string>> ParseAttributes(dynamic trial)
        {
            var attributes = new Dictionary<string, List<string>>();
            string[] keys = (string[])trial.user_attrs.keys();
            for (int i = 0; i < keys.Length; i++)
            {
                var values = new List<string>();
                if (keys[i] == "Constraint")
                {
                    double[] constraint = (double[])trial.user_attrs[keys[i]];
                    values = constraint.Select(v => v.ToString()).ToList();
                }
                else
                {
                    string[] valueArray = (string[])trial.user_attrs[keys[i]];
                    values = valueArray.ToList();
                }
                attributes.Add(keys[i], values);
            }
            return attributes;
        }
    }
}
