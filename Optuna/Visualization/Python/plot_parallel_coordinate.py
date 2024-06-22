import optuna
from optuna import Study
import plotly.graph_objects as go


def plot_parallel_coordinate(
    study: Study, objective_name: str, objective_index: int, variable_names: list[str]
) -> go.Figure:
    fig = optuna.visualization.plot_parallel_coordinate(
        study,
        target_name=objective_name,
        target=lambda t: t.values[objective_index],
        params=variable_names,
    )

    return fig
