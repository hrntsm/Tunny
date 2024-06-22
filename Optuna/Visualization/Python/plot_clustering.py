import numpy as np
import optuna
from optuna import Study
from optuna.trial import FrozenTrial, TrialState
from sklearn.cluster import KMeans
import plotly.graph_objects as go
from optuna.visualization._utils import _make_hovertext


def plot_clustering(
    study: Study,
    n_clusters: int,
    objectives_index: list[int],
    variables_index: list[int],
) -> go.Figure:
    feasible_trials, infeasible_trials = filter_constraint(study)
    kmeans = compute_kmeans(
        n_clusters, objectives_index, variables_index, feasible_trials
    )
    marker = generate_markers(kmeans)
    fig = go.Figure()
    plot_cluster_nd(study, feasible_trials, infeasible_trials, marker, fig)

    metric_names = study.metric_names
    if metric_names is not None:
        update_layout_cluster_nd(fig, metric_names)

    return go.Figure(fig)


def update_layout_cluster_nd(fig: go.Figure, metric_names: list[str]) -> None:
    if len(metric_names) == 3:
        update_layout_cluster_3d(fig, metric_names)
    else:
        update_layout_cluster_2d(fig, metric_names)


def update_layout_cluster_2d(fig: go.Figure, metric_names: list[str]):
    fig.update_layout(
        title="Clustering of Trials",
        xaxis=dict(title=metric_names[0]),
        yaxis=dict(title=metric_names[1]),
    )


def update_layout_cluster_3d(fig: go.Figure, metric_names: list[str]) -> None:
    fig.update_layout(
        title="Clustering of Trials",
        scene=dict(
            xaxis_title=metric_names[0],
            yaxis_title=metric_names[1],
            zaxis_title=metric_names[2],
        ),
    )


def plot_cluster_nd(
    study: Study,
    feasible_trials: list[FrozenTrial],
    infeasible_trials: list[FrozenTrial],
    marker: tuple[dict, dict],
    fig: go.Figure,
) -> None:
    if len(study.directions) == 2:
        plot_cluster_2d(feasible_trials, infeasible_trials, marker, fig)
    else:
        plot_cluster_3d(feasible_trials, infeasible_trials, marker, fig)


def plot_cluster_3d(feasible_trials, infeasible_trials, marker, fig):
    fig.add_trace(
        go.Scatter3d(
            x=[trial.values[0] for trial in feasible_trials],
            y=[trial.values[1] for trial in feasible_trials],
            z=[trial.values[2] for trial in feasible_trials],
            mode="markers",
            marker=marker[0],
            showlegend=False,
            text=[_make_hovertext(trial) for trial in feasible_trials],
            hovertemplate="%{text}<extra>Trial</extra>",
        )
    )
    fig.add_trace(
        go.Scatter3d(
            x=[trial.values[0] for trial in infeasible_trials],
            y=[trial.values[1] for trial in infeasible_trials],
            z=[trial.values[2] for trial in infeasible_trials],
            mode="markers",
            marker=marker[1],
            showlegend=False,
            text=[_make_hovertext(trial) for trial in feasible_trials],
            hovertemplate="%{text}<extra>Infeasible Trial</extra>",
        )
    )


def plot_cluster_2d(
    feasible_trials: list[FrozenTrial],
    infeasible_trials: list[FrozenTrial],
    marker: tuple[dict, dict],
    fig: go.Figure,
) -> None:
    fig.add_trace(
        go.Scatter(
            x=[trial.values[0] for trial in feasible_trials],
            y=[trial.values[1] for trial in feasible_trials],
            mode="markers",
            marker=marker[0],
            showlegend=False,
            text=[_make_hovertext(trial) for trial in feasible_trials],
            hovertemplate="%{text}<extra>Trial</extra>",
        )
    )
    fig.add_trace(
        go.Scatter(
            x=[trial.values[0] for trial in infeasible_trials],
            y=[trial.values[1] for trial in infeasible_trials],
            mode="markers",
            marker=marker[1],
            showlegend=False,
            text=[_make_hovertext(trial) for trial in feasible_trials],
            hovertemplate="%{text}<extra>Infeasible Trial</extra>",
        )
    )


def generate_markers(kmeans: KMeans) -> tuple[dict, dict]:
    feasible_marker = dict(
        color=kmeans.labels_,
        showscale=True,
        colorscale="RdYlBu_r",
        colorbar=dict(title="Cluster"),
        size=12,
    )
    infeasible_marker = dict(
        color="#cccccc",
        showscale=False,
        size=12,
    )

    return (feasible_marker, infeasible_marker)


def compute_kmeans(
    n_clusters: int,
    objectives_index: list[int],
    variables_index: list[int],
    feasible_trials: list[FrozenTrial],
) -> KMeans:
    target = []
    for trial in feasible_trials:
        values = []
        for i in objectives_index:
            values.append(trial.values[i])
        for i in variables_index:
            values.append(list(trial.params.values())[i])
        target.append(values)
    np_array = np.array(target)
    kmeans = KMeans(n_clusters=n_clusters).fit(np_array)
    return kmeans


def filter_constraint(study: Study) -> tuple[list[FrozenTrial], list[FrozenTrial]]:
    trials = study.get_trials(deepcopy=False, states=[TrialState.COMPLETE])
    feasible_trials: list[FrozenTrial] = []
    infeasible_trials: list[FrozenTrial] = []
    for trial in trials:
        constraints = trial.system_attrs.get("constraints")
        if constraints is None or all([x <= 0.0 for x in constraints]):
            feasible_trials.append(trial)
        else:
            infeasible_trials.append(trial)
    return feasible_trials, infeasible_trials


if __name__ == "__main__":
    # study = optuna.create_study(directions=["minimize", "minimize"])
    study = optuna.create_study(directions=["minimize", "maximize", "minimize"])
    study.optimize(
        lambda t: [
            t.suggest_float("x", 0, 1),
            t.suggest_float("y", 0, 1),
            t.suggest_float("z", 0, 1),
        ],
        n_trials=100,
    )
    fig = plot_clustering(study, 8, [1], [0])
    fig.show()
