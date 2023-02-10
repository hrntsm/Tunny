
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Globalization;
using System.IO;

using Python.Runtime;

using Tunny.Handler;
using Tunny.Settings;

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

        public StudySummary[] GetAllStudySummariesCS()
        {
            var studySummaries = new List<StudySummary>();
            if (!File.Exists(_settings.Storage.Path))
            {
                return studySummaries.ToArray();
            }

            var sqliteConnection = new SQLiteConnectionStringBuilder
            {
                DataSource = _settings.Storage.Path,
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
                GetStudyUserAttributes(studySummaries, connection);
                // GetTrials(studySummaries, connection);
            }

            return studySummaries.ToArray();
        }

        private static bool CheckTableExist(SQLiteConnection connection)
        {
            int hasStudiesTable = 0;
            using (var command = new SQLiteCommand(connection))
            {
                command.CommandText = "SELECT COUNT(*) FROM sqlite_master WHERE type='table' AND name='studies';";
                hasStudiesTable = Convert.ToInt32(command.ExecuteScalar(), CultureInfo.InvariantCulture);
            }
            return hasStudiesTable > 0;
        }

        private static void GetTrials(List<StudySummary> studySummaries, SQLiteConnection connection)
        {
            using (var command = new SQLiteCommand(connection))
            {
                command.CommandText = "SELECT * FROM trials";
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        long studyId = (long)reader["study_id"];
                        _ = Enum.TryParse((string)reader["state"], out TrialState trialState);

                        SetTrialInfoFromStudySummaries(studySummaries, reader, studyId, trialState);
                    }
                }
            }

            foreach (StudySummary summary in studySummaries)
            {
                summary.NTrials = summary.Trials.Count;
            }
        }

        private static void SetTrialInfoFromStudySummaries(List<StudySummary> studySummaries, SQLiteDataReader reader, long studyId, TrialState trialState)
        {
            studySummaries.Find(x => x.StudyId == studyId).Trials.Add(new Trial
            {
                TrialId = (int)(long)reader["trial_id"],
                Number = (int)(long)reader["number"],
                State = trialState,
                DatetimeStart = (DateTime)reader["datetime_start"],
                DatetimeComplete = (DateTime)reader["datetime_complete"],
            });
        }

        private static void GetStudyUserAttributes(List<StudySummary> studySummaries, SQLiteConnection connection)
        {
            using (var command = new SQLiteCommand(connection))
            {
                command.CommandText = "SELECT * FROM study_user_attributes";
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        long studyId = (long)reader["study_id"];
                        string key = (string)reader["key"];

                        if (key == "objective_names" || key == "variable_names")
                        {
                            string valueJson = (string)reader["value_json"];
                            string[] values = valueJson.Replace("\"", "").Replace("[", "").Replace("]", "").Replace(" ", "").Split(',');
                            studySummaries.Find(x => x.StudyId == studyId).UserAttributes.Add(key, values);
                        }
                    }
                }
            }
        }

        private static void GetStudy(List<StudySummary> studySummaries, SQLiteConnection connection)
        {
            using (var command = new SQLiteCommand(connection))
            {
                command.CommandText = "SELECT * FROM studies";
                using (SQLiteDataReader studyReader = command.ExecuteReader())
                {
                    while (studyReader.Read())
                    {
                        long studyId = (long)studyReader["study_id"];
                        string studyName = (string)studyReader["study_name"];

                        studySummaries.Add(new StudySummary
                        {
                            StudyId = (int)studyId,
                            StudyName = studyName,
                            SystemAttributes = new Dictionary<string, string[]>(),
                            UserAttributes = new Dictionary<string, string[]>()
                        });
                    }
                }
            }
        }

        public StudySummary[] GetAllStudySummariesPY()
        {
            var studySummaries = new List<StudySummary>();
            string storage = "sqlite:///" + _settings.Storage.Path;
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
            string storage = "sqlite:///" + _settings.Storage.Path;
            PythonEngine.Initialize();
            using (Py.GIL())
            {
                dynamic optuna = Py.Import("optuna");
                optuna.storages.RDBStorage(storage);
            }
            PythonEngine.Shutdown();
        }

        public void Copy(string fromStudyName, string toStudyName)
        {
            string storage = "sqlite:///" + _settings.Storage.Path;
            PythonEngine.Initialize();
            using (Py.GIL())
            {
                dynamic optuna = Py.Import("optuna");
                optuna.copy_study(from_study_name: fromStudyName, to_study_name: toStudyName, from_storage: storage, to_storage: storage);
            }
            PythonEngine.Shutdown();
        }

    }

    public class StudySummary
    {
        public int StudyId { get; set; }
        public string StudyName { get; set; }
        public Dictionary<string, string[]> UserAttributes { get; set; }
        public Dictionary<string, string[]> SystemAttributes { get; set; }
        public int NTrials { get; set; }
        public List<Trial> Trials { get; set; }
    }

    public class Trial
    {
        public int TrialId { get; set; }
        public int Number { get; set; }
        public TrialState State { get; set; }
        public DateTime DatetimeStart { get; set; }
        public DateTime DatetimeComplete { get; set; }
    }

    public enum TrialState
    {
        RUNNING,
        WAITING,
        COMPLETE,
        PRUNED,
        FAIL
    }
}
