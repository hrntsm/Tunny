import json
import optuna
from optuna import Study
import plotly.graph_objects as go


def plot_pareto_front(
    study: Study,
    objective_name: str,
    objective_index: list[int],
    hasConstraint: bool,
    includeDominatedTrials: bool,
) -> go.Figure:
    fig = optuna.visualization.plot_pareto_front(
        study,
        target_names=objective_name,
        targets=lambda t: (
            [t.values[objective_index[0]], t.values[objective_index[1]]]
            if len(objective_index) == 2
            else lambda t: [
                t.values[objective_index[0]],
                t.values[objective_index[1]],
                t.values[objective_index[2]],
            ]
        ),
        constraints_func=constraint_func if hasConstraint else None,
        include_dominated_trials=True if includeDominatedTrials else False,
    )

    return fig


def constraint_func(trial):
    return trial.user_attrs["Constraint"]


def truncate(fig, study: Study) -> go.Figure:
    user_attr = study.trials[0].user_attrs
    has_geometry = "Geometry" in user_attr
    if has_geometry == False:
        return fig

    for scatter_id, _ in enumerate(fig.data):
        new_texts = []
        for _, original_label in enumerate(fig.data[scatter_id]["text"]):
            json_label = json.loads(original_label.replace("<br>", "\\n"))
            json_label["user_attrs"].pop("Geometry")
            param_len = len(json_label["params"])
            while len(json_label["params"]) > 10:
                keys = list(json_label["params"].keys())
                json_label["params"].pop(keys.pop())
            if param_len > 10:
                json_label["params"]["__Omit_values__"] = "True"
            new_texts.append(json.dumps(json_label, indent=2).replace("\\n", "<br>"))
        fig.data[scatter_id]["text"] = new_texts

    return fig
