
using System;
using System.Collections.Generic;

using Python.Runtime;

using Tunny.Settings;
using Tunny.Util;

namespace Tunny.Solver
{
    public class Study
    {
        private readonly string _componentFolder;
        private readonly TunnySettings _settings;

        public Study(string componentFolder, TunnySettings settings)
        {
            _componentFolder = componentFolder;
            _settings = settings;
            string envPath = PythonInstaller.GetEmbeddedPythonPath() + @"\python310.dll";
            Environment.SetEnvironmentVariable("PYTHONNET_PYDLL", envPath, EnvironmentVariableTarget.Process);
        }

        public StudySummary[] GetAllStudySummaries()
        {
            var studySummaries = new List<StudySummary>();
            string storage = "sqlite:///" + _settings.StoragePath;
            PythonEngine.Initialize();
            using (Py.GIL())
            {
                dynamic optuna = Py.Import("optuna");
                PyList summaries = optuna.study.get_all_study_summaries(storage);
                foreach (dynamic summary in summaries)
                {
                    var studySummary = new StudySummary
                    {
                        StudyName = (string)summary.study_name,
                        NTrials = (int)summary.n_trials,
                    };
                    studySummaries.Add(studySummary);
                }
            }
            PythonEngine.Shutdown();
            return studySummaries.ToArray();
        }

        public void CreateNewStorage()
        {
            string storage = "sqlite:///" + _settings.StoragePath;
            PythonEngine.Initialize();
            using (Py.GIL())
            {
                dynamic optuna = Py.Import("optuna");
                optuna.storages.RDBStorage(storage);
            }
            PythonEngine.Shutdown();
        }
    }

    public class StudySummary
    {
        public string StudyName { get; set; }
        public Dictionary<string, string[]> UserAttributes { get; set; }
        public Dictionary<string, string[]> SystemAttributes { get; set; }
        public int NTrials { get; set; }
    }
}
