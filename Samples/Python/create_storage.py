# #############################################################################
# This is an example of creating STORAGE.
# #############################################################################

import optuna


def objective(trial):
    x = trial.suggest_float("x", -100, 100)
    return x**2


# SQLite3 storage
storage = optuna.storages.RDBStorage(
    url="sqlite:///test.db",
)
study_db = optuna.create_study(storage=storage)
study_db.optimize(objective, n_trials=10)


# Journal storage
file_path = "test.log"
lock_obj = optuna.storages.JournalFileOpenLock(file_path)

storage = optuna.storages.JournalStorage(
    optuna.storages.JournalFileStorage(file_path, lock_obj=lock_obj),
)
study_journal = optuna.create_study(storage=storage)
study_journal.optimize(objective, n_trials=10)
