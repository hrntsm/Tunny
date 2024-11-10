import optuna
from optuna import Study
import plotly.graph_objects as go


def plot_timeline(study: Study) -> go.Figure:
    return optuna.visualization.plot_timeline(
        study,
    )
