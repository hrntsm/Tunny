# #############################################################################
# This example shows how to coloring and plot the optimized results.
# #############################################################################

import optuna
import plotly.graph_objects as go


def objective(trial):
    x = trial.suggest_float("x", -10, 10)
    y = trial.suggest_float("y", -10, 10)
    return [x + y, x - y]


study = optuna.create_study(directions=["minimize", "minimize"])
study.optimize(objective, n_trials=100)

criteria_x = 0
criteria_value = 5

good_trials = []
no_good_trials = []

for trial in filter(
    lambda t: t.state == optuna.trial.TrialState.COMPLETE, study.trials
):
    if trial.values[0] < criteria_value and trial.params["x"] < criteria_x:
        good_trials.append(trial)
    else:
        no_good_trials.append(trial)

traces = []

traces.append(
    go.Scatter(
        x=[t.values[0] for t in good_trials],
        y=[t.values[1] for t in good_trials],
        mode="markers",
        name="good",
        marker={"color": "blue"},
    )
)

traces.append(
    go.Scatter(
        x=[t.values[0] for t in no_good_trials],
        y=[t.values[1] for t in no_good_trials],
        mode="markers",
        name="no good",
        marker={"color": "#cccccc"},
    )
)

fig = go.Figure(traces)
fig.update_layout(
    plot_bgcolor="white",
    xaxis=dict(
        title="x+y",
        showline=True,
        linewidth=1,
        linecolor="black",
        zeroline=True,
        zerolinecolor="black",
        zerolinewidth=1,
        showgrid=True,
        gridcolor="lightgray",
        range=[-10, 10],
    ),
    yaxis=dict(
        title="x-y",
        showline=True,
        linewidth=1,
        linecolor="black",
        zeroline=True,
        zerolinecolor="black",
        zerolinewidth=1,
        showgrid=True,
        gridcolor="lightgray",
        range=[-10, 10],
    ),
    width=640,
    height=480,
)

fig.show()
