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
