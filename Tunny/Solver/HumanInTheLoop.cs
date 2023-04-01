using System.IO;
using System.Text;

using Python.Runtime;

namespace Tunny.Solver
{
    public class HumanInTheLoop
    {
        private readonly PyModule _importedLibrary;
        private readonly dynamic _artifactBackend;

        public HumanInTheLoop()
        {
            PyModule importedLibrary = Py.CreateScope();
            importedLibrary.Exec(@"
from __future__ import annotations

import os
import textwrap
import threading
import time
from typing import NoReturn
from wsgiref.simple_server import make_server

import optuna
from PIL import Image
from optuna.trial import TrialState

from optuna_dashboard import ObjectiveChoiceWidget, save_note, ObjectiveSliderWidget, ObjectiveUserAttrRef
from optuna_dashboard import register_objective_form_widgets
from optuna_dashboard import set_objective_names
from optuna_dashboard import wsgi
from optuna_dashboard.artifact import upload_artifact
from optuna_dashboard.artifact.file_system import FileSystemBackend"
            );

            dynamic fileSystemBackend = importedLibrary.Get("FileSystemBackend");
            _artifactBackend = fileSystemBackend(base_path: "./artifacts");
            _importedLibrary = importedLibrary;
        }

        public void StartDashboardServerOnBackground(dynamic storage)
        {
            dynamic wsgi = _importedLibrary.Get("wsgi");
            dynamic app = wsgi(storage, artifact_backend: _artifactBackend);
            dynamic makeServer = _importedLibrary.Get("make_server");
            dynamic httpd = makeServer("127.0.0.1", 8080, app);

            dynamic threading = _importedLibrary.Get("threading");
            dynamic thread = threading.Thread(target: httpd.serve_forever);
            thread.start();
        }

        public void SetObjective(dynamic study, string[] objectiveNames)
        {
            dynamic setObjectiveNames = _importedLibrary.Get("set_objective_names");
            var pyNameList = new PyList();
            pyNameList.Append(new PyString("Check Rhino viewport: How do you like the look of this model?"));
            foreach (string objectiveName in objectiveNames)
            {
                pyNameList.Append(new PyString(objectiveName));
            }
            setObjectiveNames(study, pyNameList);
        }

        public void SetWidgets(dynamic study, string[] objectiveNames)
        {
            dynamic registerObjectiveFromWidgets = _importedLibrary.Get("register_objective_form_widgets");
            dynamic objectiveSliderWidget = _importedLibrary.Get("ObjectiveSliderWidget");
            dynamic objectiveUserAttrRef = _importedLibrary.Get("ObjectiveUserAttrRef");

            var widgets = new dynamic[objectiveNames.Length + 1];
            widgets[0] = objectiveSliderWidget(min: 1, max: 10, step: 1, description: "Smaller is better");
            for (int i = 1; i < objectiveNames.Length + 1; i++)
            {
                string objectiveName = objectiveNames[i - 1];
                var key = new PyString("result_" + objectiveName);
                widgets[i] = objectiveUserAttrRef(key: key);
            }

            registerObjectiveFromWidgets(study, widgets: widgets);
        }

        public static int GetRunningTrialNumber(dynamic study)
        {
            PyModule lenModule = Py.CreateScope();
            var pyCode = new StringBuilder();
            pyCode.AppendLine("import optuna");
            pyCode.AppendLine("from optuna.trial import TrialState");
            pyCode.AppendLine("");
            pyCode.AppendLine("def len4cs(study):");
            pyCode.AppendLine("    running_trials = study.get_trials(deepcopy=False, states=(TrialState.RUNNING, ))");
            pyCode.AppendLine("    return len(running_trials)");
            lenModule.Exec(pyCode.ToString());

            dynamic len = lenModule.Get("len4cs");

            return len(study);
        }

        public void SaveNote(dynamic study, dynamic trial)
        {
            string directoryName = "./artifacts";
            if (!Directory.Exists(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }

            string imagePath = @"C:\Users\hiroa\Desktop\TunnyTest\OptunaDotNet\image.jpg";

            dynamic uploadArtifact = _importedLibrary.Get("upload_artifact");
            dynamic artifactId = uploadArtifact(_artifactBackend, trial, imagePath);

            dynamic textWrap = _importedLibrary.Get("textwrap");
            dynamic note = textWrap.dedent($@"
# Rhino Viewport Image

![](/artifacts/{study._study_id}/{trial._trial_id}/{artifactId})
"
            );
            dynamic saveNote = _importedLibrary.Get("save_note");
            saveNote(trial, note);
        }
    }
}
