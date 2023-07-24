# #############################################################################
# This is an example of how to use the visualization function.
# #############################################################################


import optuna

n_trials = 50


def objective(trial):
    # Rosenbrock function
    x = trial.suggest_float("x", -5, 5, step=0.1)
    y = trial.suggest_int("y", -5, 5)
    trial.set_user_attr("too_long_str", "too_long_str, " * 100)

    return [(1 - x) ** 2 + 100 * (y - x**2) ** 2, x]


tpe = optuna.samplers.TPESampler()
study = optuna.create_study(sampler=tpe, directions=["minimize", "minimize"])
study.optimize(objective, n_trials=n_trials)

name = "Rosenbrock function"

optuna.visualization.plot_slice(
    study,
    params=["x", "y"],
    target=lambda t: t.values[0],
    target_name=name,
).show()

optuna.visualization.plot_pareto_front(
    study,
    target_names=[name, "x"],
).show()

optuna.visualization.plot_param_importances(
    study,
    target=lambda t: t.values[0],
    target_name=name,
).show()

optuna.visualization.plot_contour(
    study,
    params=["x", "y"],
    target=lambda t: t.values[0],
    target_name=name,
).show()

optuna.visualization.plot_optimization_history(
    study, target=lambda t: t.values[0], target_name=name
).show()

optuna.visualization.plot_parallel_coordinate(
    study, params=["x", "y"], target=lambda t: t.values[0], target_name=name
).show()

optuna.visualization.plot_edf(
    study,
    target=lambda t: t.values[0],
    target_name=name,
).show()
