# ##############################################################################
# This script creates a gif animation from the optimization history
# of the Optuna study. The script loads the study and gets the system attributes
# of the trials. The script then creates a gif animation from the artifacts of the
# trials. The script also shows the optimization history of the study.
# ##############################################################################


import json
import os
from PIL import Image
import optuna

# set the path to the storage and artifact directory
storage_path = "STORAGE_PATH"
artifact_dir_path = "ARTIFACT_DIR_PATH"
gif_path = "GIF_PATH"

# load the study and get the system attributes
lock_obj = optuna.storages.JournalFileOpenLock(storage_path)
storage = optuna.storages.JournalStorage(
    optuna.storages.JournalFileStorage(storage_path, lock_obj=lock_obj),
)
study = optuna.study.load_study(study_name="gif_test", storage=storage)
sys_attr_list = [trial.system_attrs for trial in study.trials]

# if you need to see the optimization history
# optuna.visualization.plot_optimization_history(study).show()

# create gif animation
images = []
for attr in sys_attr_list:
    for key in attr.keys():
        if "artifact" in key:
            artifact_json = json.loads(attr[key])
            images.append(
                Image.open(
                    os.path.join(artifact_dir_path, artifact_json["artifact_id"])
                )
            )
            break

images[0].save(
    gif_path,
    save_all=True,
    append_images=images[
        1:
    ],  # If you want to create a gif with 2 skips, you can set index like images[1::2]
    optimize=False,
    duration=40,
)
