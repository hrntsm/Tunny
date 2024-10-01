import numpy as np

_CONSTRAINTS_KEY = "Constraint"


def constraints(trial):
    con = trial.user_attrs[_CONSTRAINTS_KEY]
    if np.any(np.isnan(con)) or not isinstance(con, (tuple, list)):
        con = [-1]
    return con
