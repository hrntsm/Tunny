from typing import Any
import optuna
from optuna.trial import FrozenTrial, TrialState
import plotly.graph_objects as go


def plot_hypervolume(study: optuna.Study, reference_point: list[float]) -> go.Figure:
    trials: list[FrozenTrial] = study.get_trials(
        deepcopy=False, states=[TrialState.COMPLETE]
    )
    values: list[Any] = [t.values for t in trials]
    list_length: int = len(values[0])

    if reference_point is None:
        reference_point = []
        for i in range(list_length):
            max_value = max(row[i] for row in values)
            reference_point.append(max_value)
    fig: go.Figure = optuna.visualization.plot_hypervolume_history(
        study, reference_point
    )

    return fig
