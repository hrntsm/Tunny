import json
import optuna
import csv


def export_fish_csv(storage, target_study_name, output_path):
    study = optuna.load_study(storage=storage, study_name=target_study_name)
    s_id = (str)(study._study_id)
    trial = study.get_trials(deepcopy=False, states=[optuna.trial.TrialState.COMPLETE])
    metric_names = study.metric_names
    param_keys = list(trial[0].params.keys())
    artifact_path = "http://127.0.0.1:8080/artifacts/" + s_id + "/"

    label = []
    has_img = create_csv_label(param_keys, label, metric_names, trial)
    write_fish_csv(label, trial, param_keys, artifact_path, output_path)
    write_design_explorer_settings(study, output_path)

    return has_img


def create_csv_label(param_keys, label, metric_names, trial):
    set_in_label(param_keys, label)
    set_out_label(label, metric_names)
    has_img = set_img_label(label, trial)
    return has_img


def set_in_label(param_keys, label):
    for key in param_keys:
        label.append("in:" + key)


def set_out_label(label, metric_names):
    if metric_names is not None:
        for name in metric_names:
            label.append("out:" + name)


def set_img_label(label, trial):
    has_img = False
    for attr in trial[0].system_attrs:
        if attr.startswith("artifacts:"):
            artifact_value = trial[0].system_attrs[attr]
            j = json.loads(artifact_value)
            if j["mimetype"] == "image/png":
                label.append("img")
                has_img = True
    return has_img


def write_fish_csv(label, trial, param_keys, artifact_path, output_path):
    with open(output_path + "/fish.csv", "w", newline="") as f:
        writer = csv.writer(f)
        writer.writerow(label)

        for t in trial:
            t_id = (str)(t._trial_id)
            row = []
            add_row_in_params(param_keys, t, row)
            add_row_out_values(t, row)
            add_row_img(artifact_path, t, t_id, row)
            writer.writerow(row)


def add_row_img(artifact_path, t, t_id, row):
    for attr in t.system_attrs:
        if attr.startswith("artifacts:"):
            artifact_value = t.system_attrs[attr]
            j = json.loads(artifact_value)
            if j["mimetype"] == "image/png":
                row.append(artifact_path + t_id + "/" + j["artifact_id"])
                break


def add_row_out_values(t, row):
    for v in t.values:
        row.append(v)


def add_row_in_params(param_keys, t, row):
    for key in param_keys:
        row.append(t.params[key])


def write_design_explorer_settings(study, output_path):
    data = {
        "studyInfo": {
            "name": study.study_name,
            "date": "Tunny result for DesignExplorer",
        },
        "dimScales": {},
        "dimTicks": {},
        "dimMark": {},
    }

    with open(output_path + "/settings.json", "w") as file:
        json.dump(data, file, indent=4)
