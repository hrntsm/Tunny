using System;
using System.Windows.Forms;

using Python.Runtime;

using Tunny.Settings;
using Tunny.UI;

namespace Tunny.Solver
{
    public class Visualize
    {
        private readonly TunnySettings _settings;
        private readonly bool _hasConstraint;

        public Visualize(TunnySettings settings, bool hasConstraint)
        {
            _settings = settings;
            _hasConstraint = hasConstraint;
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
    }
}
