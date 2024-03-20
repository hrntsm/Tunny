using System;

using Python.Runtime;

namespace Optuna.Visualization
{
    public class Visualization
    {
        public bool HasFigure
        {
            get
            { return _fig != null; }
        }

        private readonly dynamic _study;
        private dynamic _fig;

        public Visualization(dynamic study)
        {
            _study = study;
        }

        public void Slice(string objectiveName, int objectiveIndex, string variableName)
        {
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
            dynamic visualize = ps.Get("visualize");
            _fig = visualize(_study, objectiveName, objectiveIndex, variableName);
        }

        public void ParetoFront(string[] objectiveNames, int[] objectiveIndices, bool hasConstraint, bool includeDominatedTrials)
        {
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
                         (objectiveIndices.Length == 2
                            ? "lambda t: [t.values[objective_index[0]], t.values[objective_index[1]]],\n"
                            : "lambda t: [t.values[objective_index[0]], t.values[objective_index[1]], t.values[objective_index[2]]],\n") +
                "        constraints_func=" + (hasConstraint ? "constraints" : "None") + ",\n" +
                "        include_dominated_trials=" + (includeDominatedTrials ? "True" : "False") + "\n" +
                "    )\n" +
                "    return fig\n"
            );
            dynamic visualize = ps.Get("visualize");
            _fig = visualize(_study, objectiveNames, objectiveIndices);
        }

        public void ParamImportances(string objectiveName, int objectiveIndex)
        {
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
            dynamic visualize = ps.Get("visualize");
            _fig = visualize(_study, objectiveName, objectiveIndex);
        }

        public void ParallelCoordinate(string objectiveName, int objectiveIndex, string[] variableNames)
        {
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
            dynamic visualize = ps.Get("visualize");
            _fig = visualize(_study, objectiveName, objectiveIndex, variableNames);
        }

        public void OptimizationHistory(string objectiveName, int objectiveIndex)
        {
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
            dynamic visualize = ps.Get("visualize");
            _fig = visualize(_study, objectiveName, objectiveIndex);
        }

        public void EDF(string objectiveName, int objectiveIndex)
        {
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
            dynamic visualize = ps.Get("visualize");
            _fig = visualize(_study, objectiveName, objectiveIndex);
        }

        public void Contour(string objectiveName, int objectiveIndex, string[] variableNames)
        {
            PyModule ps = Py.CreateScope();
            ps.Exec(
                "def visualize(study, objective_name, objective_index, variable_names):\n" +
                "    import optuna\n" +
                "    fig = optuna.visualization.plot_contour(\n" +
                "        study,\n" +
                "        params=variable_names,\n" +
                "        target_name=objective_name,\n" +
                "        target=lambda t:t.values[objective_index]\n" +
                "    )\n" +
                "    return fig\n"
            );
            dynamic visualize = ps.Get("visualize");
            _fig = visualize(_study, objectiveName, objectiveIndex, variableNames);
        }

        public void Hypervolume()
        {
            PyModule ps = Py.CreateScope();
            ps.Exec(
                "def visualize(study):\n" +
                "    import optuna\n" +
                "    trials = study.get_trials(deepcopy=False, states=[optuna.trial.TrialState.COMPLETE])\n" +
                "    values = [t.values for t in trials]\n" +
                "    max_values = []\n" +
                "    list_length = len(values[0])\n" +
                "    for i in range(list_length):\n" +
                "        max_value = max(row[i] for row in values)\n" +
                "        max_values.append(max_value)\n" +
                "    fig = optuna.visualization.plot_hypervolume_history(study, max_values)\n" +
                "    return fig\n"
            );
            dynamic visualize = ps.Get("visualize");
            _fig = visualize(_study);
        }

        public void Clustering(int nClusters, int[] objectiveIndex, int[] variableIndex)
        {
            PyModule ps = Py.CreateScope();
            ps.Exec(
            "def visualize(study, n_clusters, objectives_index, variables_index):\n" +
            "    import numpy as np\n" +
            "    import optuna\n" +
            "    from sklearn.cluster import KMeans\n" +
            "    import plotly.graph_objects as go\n" +
            "    from optuna.visualization._utils import _make_hovertext\n" +

            "    trials = study.get_trials(deepcopy=False, states=[optuna.trial.TrialState.COMPLETE])\n" +
            "    feasible_trials = []\n" +
            "    infeasible_trials = []\n" +
            "    for trial in trials:\n" +
            "        constraints = trial.system_attrs.get('constraints')\n" +
            "        if constraints is None or all([x <= 0.0 for x in constraints]):\n" +
            "            feasible_trials.append(trial)\n" +
            "        else:\n" +
            "            infeasible_trials.append(trial)\n" +

            "    target = []\n" +
            "    for trial in feasible_trials:\n" +
            "        values = []\n" +
            "        for i in objectives_index:\n" +
            "            values.append(trial.values[i])\n" +
            "        for i in variables_index:\n" +
            "            values.append(list(trial.params.values())[i])\n" +
            "        target.append(values)\n" +
            "    np_array = np.array(target)\n" +
            "    kmeans = KMeans(n_clusters=n_clusters).fit(np_array)\n" +

            "    feasible_marker = dict(\n" +
            "        color=kmeans.labels_,\n" +
            "        showscale=True,\n" +
            "        colorscale='RdYlBu_r',\n" +
            "        colorbar=dict(title='Cluster'),\n" +
            "        size=12,\n" +
            "    )\n" +
            "    infeasible_marker = dict(\n" +
            "        color='#cccccc',\n" +
            "        showscale=False,\n" +
            "        size=12,\n" +
            "    )\n" +
            "    fig = go.Figure()\n" +
            "    if len(study.directions) == 2:\n" +
            "        fig.add_trace(\n" +
            "            go.Scatter(\n" +
            "                x=[trial.values[0] for trial in feasible_trials],\n" +
            "                y=[trial.values[1] for trial in feasible_trials],\n" +
            "                mode='markers',\n" +
            "                marker=feasible_marker,\n" +
            "                showlegend=False,\n" +
            "                text=[_make_hovertext(trial) for trial in feasible_trials],\n" +
            "                hovertemplate='%{text}<extra>Trial</extra>',\n" +
            "            )\n" +
            "        )\n" +
            "        fig.add_trace(\n" +
            "            go.Scatter(\n" +
            "                x=[trial.values[0] for trial in infeasible_trials],\n" +
            "                y=[trial.values[1] for trial in infeasible_trials],\n" +
            "                mode='markers',\n" +
            "                marker=infeasible_marker,\n" +
            "                showlegend=False,\n" +
            "                text=[_make_hovertext(trial) for trial in feasible_trials],\n" +
            "                hovertemplate='%{text}<extra>Infeasible Trial</extra>',\n" +
            "            )\n" +
            "        )\n" +
            "    else:\n" +
            "        fig.add_trace(\n" +
            "            go.Scatter3d(\n" +
            "                x=[trial.values[0] for trial in feasible_trials],\n" +
            "                y=[trial.values[1] for trial in feasible_trials],\n" +
            "                z=[trial.values[2] for trial in feasible_trials],\n" +
            "                mode='markers',\n" +
            "                marker=feasible_marker,\n" +
            "                showlegend=False,\n" +
            "                text=[_make_hovertext(trial) for trial in feasible_trials],\n" +
            "                hovertemplate='%{text}<extra>Trial</extra>',\n" +
            "            )\n" +
            "        )\n" +
            "        fig.add_trace(\n" +
            "            go.Scatter3d(\n" +
            "                x=[trial.values[0] for trial in infeasible_trials],\n" +
            "                y=[trial.values[1] for trial in infeasible_trials],\n" +
            "                z=[trial.values[2] for trial in infeasible_trials],\n" +
            "                mode='markers',\n" +
            "                marker=infeasible_marker,\n" +
            "                showlegend=False,\n" +
            "                text=[_make_hovertext(trial) for trial in feasible_trials],\n" +
            "                hovertemplate='%{text}<extra>Infeasible Trial</extra>',\n" +
            "            )\n" +
            "        )\n" +
            "    metric_names = study.metric_names\n" +
            "    if metric_names is not None:\n" +
            "        if len(metric_names) == 3:\n" +
            "            fig.update_layout(\n" +
            "                title=f'Clustering of Trials',\n" +
            "                scene=dict(\n" +
            "                    xaxis_title=metric_names[0],\n" +
            "                    yaxis_title=metric_names[1],\n" +
            "                    zaxis_title=metric_names[2],\n" +
            "                ),\n" +
            "            )\n" +
            "        else:\n" +
            "            fig.update_layout(\n" +
            "                title=f'Clustering of Trials',\n" +
            "                xaxis=dict(title=metric_names[0]),\n" +
            "                yaxis=dict(title=metric_names[1]),\n" +
            "            )\n" +
            "    return go.Figure(fig)\n"
            );
            dynamic visualize = ps.Get("visualize");
            _fig = visualize(_study, nClusters, objectiveIndex, variableIndex);
        }

        public void TruncateParetoFrontPlotHover()
        {
            CheckPlotCreated();
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
            _fig = truncate(_fig, _study);
        }

        private void CheckPlotCreated()
        {
            if (_fig == null)
            {
                throw new InvalidOperationException("No plot has been created yet.");
            }
        }

        public void Show()
        {
            CheckPlotCreated();
            _fig.show();
        }

        public void SaveHtml(string path)
        {
            CheckPlotCreated();
            _fig.write_html(path);
        }
    }
}
