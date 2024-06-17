def export_fish_csv(storage, target_study_name, output_path):
    import optuna
    import csv
    import json

    study = optuna.load_study(storage=storage, study_name=target_study_name)
    s_id = (str)(study._study_id)
    trial = study.get_trials(deepcopy=False, states=[optuna.trial.TrialState.COMPLETE])
    metric_names = study.metric_names
    param_keys = list(trial[0].params.keys())
    artifact_path = 'http://127.0.0.1:8080/artifacts/' + s_id + '/'
    has_img = False

    label = []

    for key in param_keys:
        label.append('in:' + key)

    if metric_names is not None:
        for name in metric_names:
            label.append('out:' + name)
    for attr in trial[0].system_attrs:
        if attr.startswith('artifacts:'):
            artifact_value = trial[0].system_attrs[attr]
            j = json.loads(artifact_value)
            if j['mimetype'] == 'image/png':
                label.append('img')
                has_img = True

    with open(output_path + '/fish.csv', 'w', newline='') as f:
        writer = csv.writer(f)
        writer.writerow(label)

        for t in trial:
            t_id = (str)(t._trial_id)
            row = []
            for key in param_keys:
                row.append(t.params[key])

            for v in t.values:
                row.append(v)

            for attr in t.system_attrs:
                if attr.startswith('artifacts:'):
                    artifact_value = t.system_attrs[attr]
                    j = json.loads(artifact_value)
                    if j['mimetype'] == 'image/png':
                        row.append(artifact_path + t_id + '/' + j['artifact_id'])
                        break

            writer.writerow(row)
    data = {
        'studyInfo': {
            'name': study.study_name,
            'date': 'Tunny result for DesignExplorer'
        },
        'dimScales': {},
        'dimTicks': {},
        'dimMark': {}
    }

    with open(output_path + '/settings.json', 'w') as file:
        json.dump(data, file, indent=4)

    return has_img
