import optuna
import plotly.graph_objects as go


def plot_contour(
    study: optuna.Study, objective_name: str, objective_index: int, variable_names: str
) -> go.Figure:
    fig = optuna.visualization.plot_contour(
        study,
        params=variable_names,
        target_name=objective_name,
        target=lambda t: t.values[objective_index],
    )

    return fig
