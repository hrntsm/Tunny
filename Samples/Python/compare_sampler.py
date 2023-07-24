# #############################################################################
# This is an example of how optuna compares each sampler it can handle.
# #############################################################################

import optuna

n_trials = 50


def objective(trial):
    x = trial.suggest_float("x", -5, 5, step=0.1)
    y = trial.suggest_int("y", -5, 5)
    return x**2 + y**2


studies = []

# compare samplers
cmaes = optuna.samplers.CmaEsSampler(with_margin=True)
study_cmaes = optuna.create_study(
    sampler=cmaes, direction="minimize", study_name="cmaes"
)
study_cmaes.optimize(objective, n_trials=n_trials)
studies.append(study_cmaes)

nsgaii = optuna.samplers.NSGAIISampler(population_size=10)
study_nsgaii = optuna.create_study(
    sampler=nsgaii, direction="minimize", study_name="nsgaii"
)
study_nsgaii.optimize(objective, n_trials=n_trials)
studies.append(study_nsgaii)

nsgaiii = optuna.samplers.NSGAIIISampler(population_size=10)
study_nsgaiii = optuna.create_study(
    sampler=nsgaiii, direction="minimize", study_name="nsgaiii"
)
study_nsgaiii.optimize(objective, n_trials=n_trials)
studies.append(study_nsgaiii)

tpe = optuna.samplers.TPESampler()
study_tpe = optuna.create_study(sampler=tpe, direction="minimize", study_name="tpe")
study_tpe.optimize(objective, n_trials=n_trials)
studies.append(study_tpe)

bo = optuna.integration.BoTorchSampler()
study_bo = optuna.create_study(sampler=bo, direction="minimize", study_name="bo")
study_bo.optimize(objective, n_trials=n_trials)
studies.append(study_bo)

random = optuna.samplers.RandomSampler()
study_random = optuna.create_study(
    sampler=random, direction="minimize", study_name="random"
)
study_random.optimize(objective, n_trials=n_trials)

qmc = optuna.samplers.QMCSampler()
study_qmc = optuna.create_study(sampler=qmc, direction="minimize", study_name="qmc")
study_qmc.optimize(objective, n_trials=n_trials)

brute = optuna.samplers.BruteForceSampler()
study_brute = optuna.create_study(
    sampler=brute, direction="minimize", study_name="brute"
)
study_brute.optimize(objective, n_trials=n_trials)

print("Result")
print("  true value       :  0.0 {'x': 0.0, 'y': 0}")
print("  CmaEsSampler     : ", study_cmaes.best_value, study_cmaes.best_params)
print("  NSGAIISampler    : ", study_nsgaii.best_value, study_nsgaii.best_params)
print("  NSGAIIISampler   : ", study_nsgaiii.best_value, study_nsgaiii.best_params)
print("  TPESampler       : ", study_tpe.best_value, study_tpe.best_params)
print("  BoTorchSampler   : ", study_bo.best_value, study_bo.best_params)
print("  RandomSampler    : ", study_random.best_value, study_random.best_params)
print("  QMCSampler       : ", study_qmc.best_value, study_qmc.best_params)
print("  BruteForceSampler: ", study_brute.best_value, study_brute.best_params)

fig = optuna.visualization.plot_optimization_history(studies, error_bar=False)
fig.show()
