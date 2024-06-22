import optuna
from optuna import Study
import plotly.graph_objects as go


def plot_slice(
    study: Study, objective_name: str, objective_index: int, variable_name: str
) -> go.Figure:
    fig = optuna.visualization.plot_slice(
        study,
        target_name=objective_name,
        target=lambda t: t.values[objective_index],
        params=variable_name,
    )

    return fig
