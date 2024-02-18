using System;
using System.Linq;

using Optuna.Study;

using Python.Runtime;

using Tunny.Util;

namespace Tunny.Storage
{
    public class SqliteStorage : PythonInit, IStorage
    {
        public dynamic Storage { get; set; }

        public dynamic CreateNewStorage(bool useInnerPythonEngine, Settings.Storage storageSetting)
        {
            TLog.MethodStart();
            string sqlitePath = storageSetting.GetOptunaStoragePathByExtension();
            if (useInnerPythonEngine)
            {
                PythonEngine.Initialize();
                using (Py.GIL())
                {
                    CreateStorageProcess(sqlitePath);
                }
                PythonEngine.Shutdown();
            }
            else
            {
                CreateStorageProcess(sqlitePath);
            }

            return Storage;
        }

        private void CreateStorageProcess(string sqlitePath)
        {
            TLog.MethodStart();
            dynamic optuna = Py.Import("optuna");
            Storage = optuna.storages.RDBStorage(sqlitePath);
        }

        public void DuplicateStudyInStorage(string fromStudyName, string toStudyName, Settings.Storage storageSetting)
        {
            TLog.MethodStart();
            string storage = storageSetting.GetOptunaStoragePath();
            PythonEngine.Initialize();
            using (Py.GIL())
            {
                dynamic optuna = Py.Import("optuna");
                optuna.copy_study(from_study_name: fromStudyName, to_study_name: toStudyName, from_storage: storage, to_storage: storage);
            }
            PythonEngine.Shutdown();
        }

        public StudySummary[] GetStudySummaries(string storagePath)
        {
            TLog.MethodStart();
            var storage = new Optuna.Storage.RDB.SqliteStorage(storagePath);
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
    }
}
