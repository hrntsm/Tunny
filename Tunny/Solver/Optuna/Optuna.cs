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

        public void ShowSelectedTypePlot(string visualize, string studyName)
        {
            string storage = "sqlite:///" + _settings.Storage;
            PythonEngine.Initialize();
            using (Py.GIL())
            {
                dynamic study;
                dynamic optuna = Py.Import("optuna");
                try
                {
                    study = optuna.load_study(storage: storage, study_name: studyName);
                }
                catch (Exception e)
                {
                    TunnyMessageBox.Show(e.Message, "Tunny", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string[] nickNames = ((string)study.user_attrs["objective_names"]).Split(',');
                try
                {
                    ShowPlot(optuna, visualize, study, nickNames);
                }
                catch (Exception)
                {
                    TunnyMessageBox.Show("This visualization type is not supported in this study case.", "Tunny", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            PythonEngine.Shutdown();
        }

        private void ShowPlot(dynamic optuna, string visualize, dynamic study, string[] nickNames)
        {
            switch (visualize)
            {
                case "contour":
                    optuna.visualization.plot_contour(study, target_name: nickNames[0]).show();
                    break;
                case "EDF":
                    optuna.visualization.plot_edf(study, target_name: nickNames[0]).show();
                    break;
                case "intermediate values":
                    optuna.visualization.plot_intermediate_values(study).show();
                    break;
                case "optimization history":
                    optuna.visualization.plot_optimization_history(study, target_name: nickNames[0]).show();
                    break;
                case "parallel coordinate":
                    optuna.visualization.plot_parallel_coordinate(study, target_name: nickNames[0]).show();
                    break;
                case "param importances":
                    optuna.visualization.plot_param_importances(study, target_name: nickNames[0]).show();
                    break;
                case "pareto front":
                    dynamic fig = optuna.visualization.plot_pareto_front(study, target_names: nickNames, constraints_func: _hasConstraint ? Sampler.ConstraintFunc() : null);
                    TruncateParetoFrontPlotHover(fig, study).show();
                    break;
                case "slice":
                    optuna.visualization.plot_slice(study, target_name: nickNames[0]).show();
                    break;
                case "hypervolume":
                    PlotHypervolume(optuna, study).show();
                    break;
                default:
                    TunnyMessageBox.Show("This visualization type is not supported in this study case.", "Tunny");
                    return;
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
                "            new_texts.append(json.dumps(json_label, indent=2).replace('\\n', '<br>'))\n" +
                "        fig.data[scatter_id]['text'] = new_texts\n" +
                "    return fig\n"
            );
            dynamic truncate = ps.Get("truncate");
            return truncate(fig, study);
        }

        private static dynamic PlotHypervolume(dynamic optuna, dynamic study)
        {

            var trials = (dynamic[])study.trials;
            int objectivesCount = ((double[])trials[0].values).Length;
            var trialValues = new List<double[]>();
            foreach (dynamic trial in trials)
            {
                trialValues.Add((double[])trial.values);
            }
            double[] maxObjectiveValues = new double[objectivesCount];
            for (int i = 0; i < objectivesCount; i++)
            {
                maxObjectiveValues[i] = trialValues.Select(v => v[i]).Max();
            }

            PyList hvs = ComputeHypervolume(optuna, trials, maxObjectiveValues, out PyList trialNumbers);
            return CreateHypervolumeFigure(trials, hvs, trialNumbers);
        }

        private static PyList ComputeHypervolume(dynamic optuna, dynamic[] trials, double[] maxObjectiveValues, out PyList trialNumbers)
        {
            dynamic np = Py.Import("numpy");

            var hvs = new PyList();
            var rpObj = new PyList();
            trialNumbers = new PyList();

            foreach (double max in maxObjectiveValues)
            {
                rpObj.Append(new PyFloat(max));
            }
            dynamic referencePoint = np.array(rpObj);

            dynamic wfg = optuna._hypervolume.WFG();
            for (int i = 1; i < trials.Length + 1; i++)
            {
                var vector = new PyList();
                for (int j = 0; j < i; j++)
                {
                    vector.Append(trials[j].values);
                }
                hvs.Append(wfg.compute(np.array(vector), referencePoint));
                trialNumbers.Append(new PyInt(i));
            }
            return hvs;
        }

        private static dynamic CreateHypervolumeFigure(dynamic[] trials, PyList hvs, PyList trialNumbers)
        {
            dynamic go = Py.Import("plotly.graph_objects");

            var plotItems = new PyDict();
            plotItems.SetItem("x", trialNumbers);
            plotItems.SetItem("y", hvs);

            var plotRange = new PyDict();
            var rangeObj = new PyObject[] { new PyFloat(0), new PyFloat(trials.Length + 1) };
            plotRange.SetItem("range", new PyList(rangeObj));

            dynamic fig = go.Figure();
            fig.add_trace(go.Scatter(plotItems, name: "Hypervolume"));
            fig.update_layout(xaxis: plotRange);
            fig.update_xaxes(title_text: "#Trials");
            fig.update_yaxes(title_text: "Hypervolume");

            return fig;
        }

        public void ShowClusteringPlot(string studyName, int numCluster)
        {
            string storage = "sqlite:///" + _settings.Storage;
            PythonEngine.Initialize();
            using (Py.GIL())
            {
                dynamic study;
                dynamic optuna = Py.Import("optuna");
                try
                {
                    study = optuna.load_study(storage: storage, study_name: studyName);
                }
                catch (Exception e)
                {
                    TunnyMessageBox.Show(e.Message, "Tunny", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string[] nickNames = ((string)study.user_attrs["objective_names"]).Split(',');
                if (nickNames.Length == 1)
                {
                    TunnyMessageBox.Show("Clustering Error\n\nClustering is for multi-objective optimization only.", "Tunny", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    try
                    {
                        ShowCluster(optuna, study, nickNames, numCluster);
                    }
                    catch (Exception)
                    {
                        TunnyMessageBox.Show("Clustering Error", "Tunny", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            PythonEngine.Shutdown();

        }

        private void ShowCluster(dynamic optuna, dynamic study, string[] nickNames, int numCluster)
        {
            dynamic fig = optuna.visualization.plot_pareto_front(study, target_names: nickNames, constraints_func: _hasConstraint ? Sampler.ConstraintFunc() : null);
            fig = TruncateParetoFrontPlotHover(fig, study);
            ClusteringParetoFrontPlot(fig, study, numCluster).show();
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
                dynamic trial = study.trials[res];
                ParseTrial(modelResult, trial);
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
                if (OutputLoop.IsForcedStopOutput)
                {
                    break;
                }
                ParseTrial(modelResult, trial);
                worker.ReportProgress(i * 100 / bestTrials.Length);
            }
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
