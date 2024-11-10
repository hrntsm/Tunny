import optuna
from optuna import Study
import plotly.graph_objects as go


def plot_rank(
    study: Study, objective_name: str, objective_index: int, variable_names: list[str]
) -> go.Figure:
    return optuna.visualization.plot_rank(
        study,
        target_name=objective_name,
        target=lambda t: t.values[objective_index],
        params=variable_names,
    )
