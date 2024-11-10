import optuna_integration
import optuna
from optuna import Study
import plotly.graph_objects as go


def plot_importances(
    study: Study,
    objective_name: str,
    objective_index: int,
    params: list[str],
    evaluator_type: str,
) -> go.Figure:

    evaluator = None
    if evaluator_type == "fAnova":
        evaluator = optuna.importance.FanovaImportanceEvaluator()
    elif evaluator_type == "PedAnova":
        evaluator = optuna.importance.PedAnovaImportanceEvaluator()
    elif evaluator_type == "MeanDecreaseImpurity":
        evaluator = optuna.importance.MeanDecreaseImpurityImportanceEvaluator()
    elif evaluator_type == "SHAP":
        evaluator = optuna_integration.ShapleyImportanceEvaluator()

    return optuna.visualization.plot_param_importances(
        study,
        params=params,
        target_name=objective_name,
        target=lambda t: t.values[objective_index],
        evaluator=evaluator,
    )
