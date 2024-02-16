using System;
using System.Linq;

using Optuna.Study;

using Python.Runtime;

using Tunny.Util;

namespace Tunny.Storage
{
    public class JournalStorage : PythonInit, IStorage
    {
        public dynamic Storage { get; set; }

        public StudySummary[] GetStudySummaries(string storagePath)
        {
            TLog.MethodStart();
            var storage = new Optuna.Storage.Journal.JournalStorage(storagePath, true);
            StudySummary[] studySummaries = Study.GetAllStudySummaries(storage);

            var oldFormatVersion = new Version("0.9.1");
            foreach (StudySummary studySummary in studySummaries)
            {
                string versionString = (studySummary.UserAttrs["tunny_version"] as string[])[0];
                var version = new Version(versionString);
                if (version <= oldFormatVersion)
                {
                    UpdateVariableNamesAttr(studySummary);
                }
            }

            return studySummaries;
        }

        private static void UpdateVariableNamesAttr(StudySummary studySummary)
        {
            TLog.MethodStart();
            studySummary.UserAttrs["variable_names"] = (studySummary.UserAttrs["variable_names"] as string[])[0].Split(',').ToArray();
        }

        public dynamic CreateNewStorage(bool useInnerPythonEngine, Settings.Storage storageSetting)
        {
            TLog.MethodStart();
            string storagePath = storageSetting.GetOptunaStoragePathByExtension();
            if (useInnerPythonEngine)
            {
                PythonEngine.Initialize();
                using (Py.GIL())
                {
                    CreateStorageProcess(storagePath);
                }
                PythonEngine.Shutdown();
            }
            else
            {
                CreateStorageProcess(storagePath);
            }

            return Storage;
        }

        private void CreateStorageProcess(string storagePath)
        {
            TLog.MethodStart();
            dynamic optuna = Py.Import("optuna");
            dynamic lockObj = optuna.storages.JournalFileOpenLock(storagePath);
            Storage = optuna.storages.JournalStorage(optuna.storages.JournalFileStorage(storagePath, lock_obj: lockObj));
        }

        public void DuplicateStudyInStorage(string fromStudyName, string toStudyName, Settings.Storage storageSetting)
        {
            TLog.MethodStart();
            PythonEngine.Initialize();
            using (Py.GIL())
            {
                dynamic optuna = Py.Import("optuna");
                dynamic storage = CreateNewStorage(false, storageSetting);
                optuna.copy_study(from_study_name: fromStudyName, to_study_name: toStudyName, from_storage: storage, to_storage: storage);
            }
            PythonEngine.Shutdown();
        }
    }
}
