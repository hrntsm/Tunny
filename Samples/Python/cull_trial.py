import optuna

storage_path = "fish.log"
target_study_name = "study_target"
culled_study_name = "study_cull"
cull_trial_number = [10, 15, 17]

# If you use .log file, use blow.
lock_obj = optuna.storages.JournalFileOpenLock(storage_path)
storage = optuna.storages.JournalStorage(
    optuna.storages.JournalFileStorage(storage_path, lock_obj=lock_obj),
)

# If you use RDB, use blow.
# storage = optuna.storages.RDBStorage("sqlite:///" + storage_path)

# Load storage
study = optuna.load_study(storage=storage, study_name=target_study_name)
usr_attr = study.user_attrs
trials = study.get_trials()
directions = study.directions

# Create new study to save cull result
cull_study = optuna.create_study(
    storage=storage, study_name=culled_study_name, directions=directions
)

# If you want to read this result file from Tunny, you need to set the following.
# Tunny needs to read some attributes
for key, value in usr_attr.items():
    cull_study.set_user_attr(key, value)

# Cull trials
for num in cull_trial_number:
    trials = list(filter(lambda trial: trial.number != num, trials))
cull_study.add_trials(trials=trials)

# visualize result(if you want)
optuna.visualization.plot_pareto_front(cull_study).show()
