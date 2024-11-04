import optuna
from optuna import Study
import plotly.graph_objects as go


def plot_importances(
    study: Study, objective_name: str, objective_index: int, params: list[str]
) -> go.Figure:
    return optuna.visualization.plot_param_importances(
        study,
        params=params,
        target_name=objective_name,
        target=lambda t: t.values[objective_index],
    )
