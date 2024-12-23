from optuna import Trial
from optuna.trial import TrialState, FrozenTrial


def check_duplicate(trial: Trial):
    states_to_consider = (TrialState.COMPLETE,)
    trials_to_consider: list[FrozenTrial] = trial.study.get_trials(
        deepcopy=False, states=states_to_consider
    )
    for t in reversed(trials_to_consider):
        if trial.params == t.params:
            set_attr_to_duplicate_trial(trial, t)
            return t.values
    return None


def set_attr_to_duplicate_trial(base_trial: Trial, compared_trial: FrozenTrial) -> None:
    base_trial.set_user_attr(
        "NOTE",
        f"trial {compared_trial.number} and trial {base_trial.number} were duplicate parameters.",
    )
    for key, value in compared_trial.user_attrs.items():
        base_trial.set_user_attr(key, value)
