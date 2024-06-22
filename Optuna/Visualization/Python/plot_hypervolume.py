import optuna
import plotly.graph_objects as go


def plot_hypervolume(study: optuna.Study) -> go.Figure:
    trials = study.get_trials(deepcopy=False, states=[optuna.trial.TrialState.COMPLETE])
    values = [t.values for t in trials]
    max_values: list[float] = []
    list_length = len(values[0])

    for i in range(list_length):
        max_value = max(row[i] for row in values)
        max_values.append(max_value)
    fig = optuna.visualization.plot_hypervolume_history(study, max_values)

    return fig
