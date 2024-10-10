from optuna.trial import TrialState


def check_duplicate(trial):

    states_to_consider = (TrialState.COMPLETE,)
    trials_to_consider = trial.study.get_trials(
        deepcopy=False, states=states_to_consider
    )
    for t in reversed(trials_to_consider):
        if trial.params == t.params:
            trial.set_user_attr(
                "NOTE",
                f"trial {t.number} and trial {trial.number} were duplicate parameters.",
            )
            if "Constraint" in t.user_attrs:
                trial.set_user_attr("Constraint", t.user_attrs["Constraint"])
            return t.values
    return None
