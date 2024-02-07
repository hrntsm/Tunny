using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Globalization;
using System.IO;

using Optuna.Study;
using Optuna.Trial;

using Python.Runtime;

using Tunny.Util;

namespace Tunny.Storage
{
    public class SqliteStorage : PythonInit, IStorage
    {
        public dynamic Storage { get; set; }

        private static bool CheckTableExist(SQLiteConnection connection)
        {
            TLog.MethodStart();
            int hasStudiesTable;
            using (var command = new SQLiteCommand(connection))
            {
                command.CommandText = "SELECT COUNT(*) FROM sqlite_master WHERE type='table' AND name='studies';";
                hasStudiesTable = Convert.ToInt32(command.ExecuteScalar(), CultureInfo.InvariantCulture);
            }
            return hasStudiesTable > 0;
        }

        private static void GetStudyAttributes(List<StudySummary> studySummaries, SQLiteConnection connection)
        {
            TLog.MethodStart();
            using (var command = new SQLiteCommand(connection))
            {
                command.CommandText = "SELECT * FROM study_user_attributes";
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        long studyId = (long)reader["study_id"];
                        string key = (string)reader["key"];

                        if (key == "variable_names")
                        {
                            string valueJson = (string)reader["value_json"];
                            string[] values = valueJson.Replace("\"", "").Replace("[", "").Replace("]", "").Replace(" ", "").Split(',');
                            studySummaries.Find(x => x.StudyId == studyId).UserAttrs.Add(key, values);
                        }
                    }
                }
            }
            using (var command = new SQLiteCommand(connection))
            {
                command.CommandText = "SELECT * FROM study_system_attributes";
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        long studyId = (long)reader["study_id"];
                        string key = (string)reader["key"];

                        if (key == "study:metric_names")
                        {
                            string valueJson = (string)reader["value_json"];
                            string[] values = valueJson.Replace("\"", "").Replace("[", "").Replace("]", "").Replace(" ", "").Split(',');
                            studySummaries.Find(x => x.StudyId == studyId).SystemAttrs.Add(key, values);
                        }
                    }
                }
            }
        }

        private static void GetStudy(List<StudySummary> studySummaries, SQLiteConnection connection)
        {
            TLog.MethodStart();
            using (var command = new SQLiteCommand(connection))
            {
                command.CommandText = "SELECT * FROM studies";
                using (SQLiteDataReader studyReader = command.ExecuteReader())
                {
                    while (studyReader.Read())
                    {
                        long studyId = (long)studyReader["study_id"];
                        string studyName = (string)studyReader["study_name"];

                        var summary = new StudySummary(studyName,
                                                       StudyDirection.NotSet,
                                                       new Trial(),
                                                       new Dictionary<string, object>(),
                                                       new Dictionary<string, object>(),
                                                       100,
                                                       DateTime.Now,
                                                       (int)studyId);

                        studySummaries.Add(summary);
                    }
                }
            }
        }

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
            var studySummaries = new List<StudySummary>();
            if (!File.Exists(storagePath))
            {
                return studySummaries.ToArray();
            }

            var sqliteConnection = new SQLiteConnectionStringBuilder
            {
                DataSource = storagePath,
                Version = 3
            };

            using (var connection = new SQLiteConnection(sqliteConnection.ToString()))
            {
                connection.Open();

                if (!CheckTableExist(connection))
                {
                    return studySummaries.ToArray();
                }
                GetStudy(studySummaries, connection);
                GetStudyAttributes(studySummaries, connection);
            }

            return studySummaries.ToArray();
        }
    }
}
