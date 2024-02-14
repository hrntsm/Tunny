using System;
using System.Windows.Forms;

using Python.Runtime;

using Tunny.Enum;
using Tunny.PostProcess;
using Tunny.Settings;
using Tunny.UI;
using Tunny.Util;

namespace Tunny.Solver
{
    public class Visualize : PythonInit
    {
        private readonly TunnySettings _settings;
        private readonly bool _hasConstraint;

        public Visualize(TunnySettings settings, bool hasConstraint)
        {
            TLog.MethodStart();
            _settings = settings;
            _hasConstraint = hasConstraint;
        }

        private static dynamic LoadStudy(dynamic optuna, dynamic storage, string studyName)
        {
            TLog.MethodStart();
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

        public void Plot(PlotSettings pSettings)
        {
            TLog.MethodStart();
            PythonEngine.Initialize();
            using (Py.GIL())
            {
                dynamic optuna = Py.Import("optuna");
                dynamic storage = _settings.Storage.CreateNewOptunaStorage(false);
                dynamic study = LoadStudy(optuna, storage, pSettings.TargetStudyName);
                if (study == null)
                {
                    return;
                }

                try
                {
                    dynamic fig = CreateFigure(study, pSettings);
                    FigureActions(fig, pSettings);
                }
                catch (Exception)
                {
                    string message = "This visualization type is not supported in this study case.";
                    TunnyMessageBox.Show(message, "Tunny", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            PythonEngine.Shutdown();
        }

        private dynamic CreateFigure(dynamic study, PlotSettings pSettings)
        {
            TLog.MethodStart();
            switch (pSettings.PlotTypeName)
            {
                case "contour":
                    return VisualizeContour(study, pSettings);
                case "EDF":
                    return VisualizeEDF(study, pSettings);
                case "optimization history":
                    return VisualizeOptimizationHistory(study, pSettings);
                case "parallel coordinate":
                    return VisualizeParallelCoordinate(study, pSettings);
                case "param importances":
                    return VisualizeParamImportances(study, pSettings);
                case "pareto front":
                    dynamic fig = VisualizeParetoFront(study, pSettings);
                    return TruncateParetoFrontPlotHover(fig, study);
                case "slice":
                    return VisualizeSlice(study, pSettings);
                case "hypervolume":
                    return Hypervolume.CreateFigure(study, pSettings);
                default:
                    TunnyMessageBox.Show("This visualization type is not supported in this study case.", "Tunny");
                    return null;
            }
        }

        private static dynamic VisualizeSlice(dynamic study, PlotSettings pSettings)
        {
            TLog.MethodStart();
            PyModule ps = Py.CreateScope();
            ps.Exec(
                "def visualize(study, objective_name, objective_index, variable_name):\n" +
                "    import optuna\n" +
                "    fig = optuna.visualization.plot_slice(\n" +
                "        study," +
                "        target_name=objective_name,\n" +
                "        target=lambda t:t.values[objective_index],\n" +
                "        params=variable_name\n" +
                "    )\n" +
                "    return fig\n"
            );
            dynamic fig = ps.Get("visualize");
            return fig(study, pSettings.TargetObjectiveName[0], pSettings.TargetObjectiveIndex[0], pSettings.TargetVariableName);
        }

        private dynamic VisualizeParetoFront(dynamic study, PlotSettings pSettings)
        {
            TLog.MethodStart();
            PyModule ps = Py.CreateScope();
            ps.Exec(
                "def constraints(trial):\n" +
                "  return trial.user_attrs[\"Constraint\"]\n"
            );
            ps.Exec(
                "def visualize(study, objective_name, objective_index):\n" +
                "    import optuna\n" +
                "    fig = optuna.visualization.plot_pareto_front(\n" +
                "        study,\n" +
                "        target_names=objective_name,\n" +
                "        targets=" +
                         (pSettings.TargetObjectiveIndex.Length == 2
                            ? "lambda t: [t.values[objective_index[0]], t.values[objective_index[1]]],\n"
                            : "lambda t: [t.values[objective_index[0]], t.values[objective_index[1]], t.values[objective_index[2]]],\n") +
                "        constraints_func=" + (_hasConstraint ? "constraints" : "None") + ",\n" +
                "        include_dominated_trials=" + (pSettings.IncludeDominatedTrials ? "True" : "False") + "\n" +
                "    )\n" +
                "    return fig\n"
            );
            dynamic fig = ps.Get("visualize");
            return fig(study, pSettings.TargetObjectiveName, pSettings.TargetObjectiveIndex);
        }

        private static dynamic VisualizeParamImportances(dynamic study, PlotSettings pSettings)
        {
            TLog.MethodStart();
            PyModule ps = Py.CreateScope();
            ps.Exec(
                "def visualize(study, objective_name, objective_index):\n" +
                "    import optuna\n" +
                "    fig = optuna.visualization.plot_param_importances(\n" +
                "        study,\n" +
                "        target_name=objective_name,\n" +
                "        target=lambda t:t.values[objective_index]\n" +
                "    )\n" +
                "    return fig\n"
            );
            dynamic fig = ps.Get("visualize");
            return fig(study, pSettings.TargetObjectiveName[0], pSettings.TargetObjectiveIndex[0]);
        }

        private static dynamic VisualizeParallelCoordinate(dynamic study, PlotSettings pSettings)
        {
            TLog.MethodStart();
            PyModule ps = Py.CreateScope();
            ps.Exec(
                "def visualize(study, objective_name, objective_index, variable_name):\n" +
                "    import optuna\n" +
                "    fig = optuna.visualization.plot_parallel_coordinate(\n" +
                "        study,\n" +
                "        target_name=objective_name,\n" +
                "        target=lambda t:t.values[objective_index],\n" +
                "        params=variable_name\n" +
                "    )\n" +
                "    return fig\n"
            );
            dynamic fig = ps.Get("visualize");
            return fig(study, pSettings.TargetObjectiveName[0], pSettings.TargetObjectiveIndex[0], pSettings.TargetVariableName);
        }

        private static dynamic VisualizeOptimizationHistory(dynamic study, PlotSettings pSettings)
        {
            TLog.MethodStart();
            PyModule ps = Py.CreateScope();
            ps.Exec(
                "def visualize(study, objective_name, objective_index):\n" +
                "    import optuna\n" +
                "    fig = optuna.visualization.plot_optimization_history(\n" +
                "        study,\n" +
                "        target_name=objective_name,\n" +
                "        target=lambda t:t.values[objective_index]\n" +
                "    )\n" +
                "    return fig\n"
            );
            dynamic fig = ps.Get("visualize");
            return fig(study, pSettings.TargetObjectiveName[0], pSettings.TargetObjectiveIndex[0]);
        }

        private static dynamic VisualizeEDF(dynamic study, PlotSettings pSettings)
        {
            TLog.MethodStart();
            PyModule ps = Py.CreateScope();
            ps.Exec(
                "def visualize(study, objective_name, objective_index):\n" +
                "    import optuna\n" +
                "    fig = optuna.visualization.plot_edf(\n" +
                "        study,\n" +
                "        target_name=objective_name,\n" +
                "        target=lambda t:t.values[objective_index]\n" +
                "    )\n" +
                "    return fig\n"
            );
            dynamic fig = ps.Get("visualize");
            return fig(study, pSettings.TargetObjectiveName[0], pSettings.TargetObjectiveIndex[0]);
        }

        private static dynamic VisualizeContour(dynamic study, PlotSettings pSettings)
        {
            TLog.MethodStart();
            PyModule ps = Py.CreateScope();
            ps.Exec(
                "def visualize(study, variable_names, objective_name, objective_index):\n" +
                "    import optuna\n" +
                "    fig = optuna.visualization.plot_contour(\n" +
                "        study,\n" +
                "        params=variable_names,\n" +
                "        target_name=objective_name,\n" +
                "        target=lambda t:t.values[objective_index]\n" +
                "    )\n" +
                "    return fig\n"
            );
            dynamic fig = ps.Get("visualize");
            return fig(study, pSettings.TargetVariableName, pSettings.TargetObjectiveName[0], pSettings.TargetObjectiveIndex[0]);
        }

        private static dynamic TruncateParetoFrontPlotHover(dynamic fig, dynamic study)
        {
            TLog.MethodStart();
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

        private static void FigureActions(dynamic fig, PlotSettings pSettings)
        {
            TLog.MethodStart();
            if (fig != null && pSettings.PlotActionType == PlotActionType.Show)
            {
                fig.show();
            }
            else if (fig != null && pSettings.PlotActionType == PlotActionType.Save)
            {
                SaveFigure(fig, pSettings.PlotTypeName);
            }
        }

        private static void SaveFigure(dynamic fig, string name)
        {
            TLog.MethodStart();
            var sfd = new SaveFileDialog
            {
                FileName = name + ".html",
                Filter = @"HTML file(*.html)|*.html",
                Title = @"Save"
            };
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                fig.write_html(sfd.FileName);
            }
        }

        public void ClusteringPlot(PlotSettings pSettings)
        {
            TLog.MethodStart();
            PythonEngine.Initialize();
            using (Py.GIL())
            {
                dynamic optuna = Py.Import("optuna");
                dynamic storage = _settings.Storage.CreateNewOptunaStorage(false);
                dynamic study = LoadStudy(optuna, storage, pSettings.TargetStudyName);
                if (study == null)
                {
                    return;
                }

                string[] nickNames = ((string)study.system_attrs["study:metric_names"]).Split(',');
                if (nickNames.Length == 1)
                {
                    TunnyMessageBox.Show("Clustering Error\n\nClustering is for multi-objective optimization only.", "Tunny", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    dynamic fig = CreateClusterFigure(study, pSettings);
                    FigureActions(fig, pSettings);
                }
            }
            PythonEngine.Shutdown();
        }

        private dynamic CreateClusterFigure(dynamic study, PlotSettings pSettings)
        {
            TLog.MethodStart();
            try
            {
                dynamic fig = VisualizeParetoFront(study, pSettings);
                fig = TruncateParetoFrontPlotHover(fig, study);
                return ClusteringParetoFrontPlot(fig, study, pSettings.ClusterCount);
            }
            catch (Exception)
            {
                string message = "Clustering plot Error";
                TunnyMessageBox.Show(message, "Tunny", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return null;
        }

        private static dynamic ClusteringParetoFrontPlot(dynamic fig, dynamic study, int numCluster)
        {
            TLog.MethodStart();
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
    }
}
