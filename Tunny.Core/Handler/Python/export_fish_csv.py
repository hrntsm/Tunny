import json
import optuna
from optuna.storages import BaseStorage
from optuna.trial import FrozenTrial
from optuna.study import Study
import csv


def export_fish_csv(
    storage: BaseStorage, target_study_name: str, output_path: str
) -> bool:
    study: Study = optuna.load_study(storage=storage, study_name=target_study_name)
    s_id = (str)(study._study_id)
    trial: list[FrozenTrial] = study.get_trials(
        deepcopy=False, states=[optuna.trial.TrialState.COMPLETE]
    )
    artifact_path = "http://127.0.0.1:8080/artifacts/" + s_id + "/"
    metric_names: list[str] = set_metric_names(study)

    label: list[str] = []
    has_img: bool = create_csv_label(label, trial, metric_names)
    write_fish_csv(label, trial, artifact_path, output_path)
    write_design_explorer_settings(study.study_name, output_path)

    return has_img


def set_metric_names(study: Study) -> list[str]:
    metric_names: list[str] | None = study.metric_names
    if metric_names is None:
        metric_names = []
        for i in range(len(study.directions)):
            metric_names.append("objective_" + (str)(i))
    return metric_names


def create_csv_label(
    label: list[str], trials: list[FrozenTrial], metric_names: list[str]
) -> bool:
    set_other_label(label)
    param_keys = list(trials[0].params.keys())
    set_in_param_label(label, param_keys)
    set_in_attr_label(label, trials)
    set_out_label(label, metric_names)
    has_img = set_img_label(label, trials)

    return has_img


def set_other_label(label: list[str]) -> None:
    label.append("trial_id")


def set_in_attr_label(label: list[str], trials: list[FrozenTrial]) -> None:
    for key, value in trials[0].user_attrs.items():
        if value is None:
            continue

        if isinstance(value, int) or isinstance(value, float):
            label.append("in:attr_" + key)
        elif isinstance(value, list) and (
            all(isinstance(x, int) for x in value)
            or all(isinstance(x, float) for x in value)
        ):
            for i in range(len(value)):
                label.append("in:attr_" + key + "_" + (str)(i))


def set_in_param_label(label: list[str], param_keys: list[str]) -> None:
    for key in param_keys:
        label.append("in:param_" + key)


def set_out_label(label: list[str], metric_names: list[str]) -> None:
    for name in metric_names:
        label.append("out:" + name)


def set_img_label(label: list[str], trials: list[FrozenTrial]) -> bool:
    has_img = False
    artifact_attrs: list[str] = [
        attr for attr in trials[0].system_attrs if attr.startswith("artifacts:")
    ]
    for attr in artifact_attrs:
        artifact_value = trials[0].system_attrs[attr]
        j = json.loads(artifact_value)
        if j["mimetype"] == "image/png":
            label.append("img")
            has_img = True
            break

    return has_img


def write_fish_csv(
    label: list[str], trial: list[FrozenTrial], artifact_path: str, output_path: str
) -> None:
    param_keys = list(trial[0].params.keys())
    with open(output_path + "/fish.csv", "w", newline="") as f:
        writer = csv.writer(f)
        writer.writerow(label)

        for t in trial:
            row = []
            add_row_other(t, row)
            add_row_in_params(t, row, param_keys)
            add_row_in_attr(t, row)
            add_row_out_values(t, row)
            add_row_img(t, row, artifact_path)
            writer.writerow(row)


def add_row_other(trial: FrozenTrial, row: list[str]) -> None:
    row.append((str)(trial.number))


def add_row_img(trial: FrozenTrial, row: list[str], artifact_path: str) -> None:
    trial_id = (str)(trial._trial_id)
    artifact_attrs = [
        attr for attr in trial.system_attrs if attr.startswith("artifacts:")
    ]
    for attr in artifact_attrs:
        artifact_value = trial.system_attrs[attr]
        j = json.loads(artifact_value)
        if j["mimetype"] == "image/png":
            row.append(artifact_path + trial_id + "/" + j["artifact_id"])
            break


def add_row_out_values(trial: FrozenTrial, row: list[str]) -> None:
    for v in trial.values:
        row.append(v)


def add_row_in_params(
    trial: FrozenTrial, row: list[str], param_keys: list[str]
) -> None:
    for key in param_keys:
        row.append(trial.params[key])


def add_row_in_attr(trial: FrozenTrial, row: list[str]) -> None:
    for value in trial.user_attrs.values():
        if value is None:
            continue
        if isinstance(value, int) or isinstance(value, float):
            s: str = (str)(value)
            row.append(s)
        elif isinstance(value, list) and (
            all(isinstance(x, int) for x in value)
            or all(isinstance(x, float) for x in value)
        ):
            for v in value:
                row.append(v)


def write_design_explorer_settings(study_name: str, output_path: str) -> None:
    data = {
        "studyInfo": {
            "name": study_name,
            "date": "Tunny result for DesignExplorer",
        },
        "dimScales": {},
        "dimTicks": {},
        "dimMark": {},
    }

    with open(output_path + "/settings.json", "w") as file:
        json.dump(data, file, indent=4)
