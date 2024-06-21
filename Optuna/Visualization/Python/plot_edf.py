import optuna
from optuna import Study
import plotly.graph_objects as go


def plot_edf(study: Study, objective_name: str, objective_index: int) -> go.Figure:
    fig = optuna.visualization.plot_edf(
        study, target_name=objective_name, target=lambda t: t.values[objective_index]
    )

    return fig
