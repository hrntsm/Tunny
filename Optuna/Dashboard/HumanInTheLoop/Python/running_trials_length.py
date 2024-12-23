from optuna.study import Study
from optuna.trial import TrialState, FrozenTrial


def running_trials_length(study: Study) -> int:
    running_trials: list[FrozenTrial] = study.get_trials(
        deepcopy=False, states=(TrialState.RUNNING,)
    )

    return len(running_trials)
