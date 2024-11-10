import optuna
from optuna import Study
import plotly.graph_objects as go


def plot_optimization_history(
    studies: list[Study], objective_name: str, objective_index: int, error_bar: bool
) -> go.Figure:
    fig = optuna.visualization.plot_optimization_history(
        studies,
        target_name=objective_name,
        target=lambda t: t.values[objective_index],
        error_bar=error_bar,
    )

    return fig
