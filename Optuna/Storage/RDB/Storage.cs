using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;

using Optuna.Study;
using Optuna.Trial;

namespace Optuna.Storage.RDB
{
    public class SqliteStorage : IStorage
    {
        private readonly Dictionary<int, Study.Study> _studies = new Dictionary<int, Study.Study>();
        private readonly SQLiteConnectionStringBuilder _sqliteConnection;
        private int _nextStudyId;
        private int _trialId;

        public SqliteStorage(string filePath, bool createIfNotExist = false)
        {
            _sqliteConnection = new SQLiteConnectionStringBuilder
            {
                DataSource = filePath,
                Version = 3,
            };
            if (!File.Exists(filePath))
            {
                if (!createIfNotExist)
                {
                    throw new ArgumentException("The specified database does not exist.");
                }
                else
                {
                    SQLiteConnection.CreateFile(filePath);
                    CreateBaseTables();
                }
            }

            GetAllStudies();
        }

        public void CheckTrialIsUpdatable(int trialId, TrialState trialState)
        {
            throw new NotImplementedException();
        }

        public int CreateNewStudy(StudyDirection[] studyDirections, string studyName)
        {
            long maxLength;
            using (var connection = new SQLiteConnection(_sqliteConnection.ToString()))
            {
                connection.Open();
                using (var command = new SQLiteCommand("SELECT COUNT(*) FROM studies", connection))
                {
                    maxLength = (long)command.ExecuteScalar();
                }
                using (var command = new SQLiteCommand(connection))
                {
                    command.CommandText = "SELECT COUNT(*) FROM studies WHERE study_name = @SearchName;";
                    command.Parameters.AddWithValue("@SearchName", studyName);
                    bool hasTargetStudyName = (long)command.ExecuteScalar() > 0;
                    if (hasTargetStudyName)
                    {
                        throw new InvalidOperationException($"The study name '{studyName}' already exists.");
                    }
                }
                using (var command = new SQLiteCommand(connection))
                {
                    command.CommandText = "INSERT INTO studies (study_name) VALUES (@studyName);";
                    command.Parameters.AddWithValue("@studyName", studyName);
                    command.ExecuteNonQuery();
                }
                using (var command = new SQLiteCommand(connection))
                {
                    for (int i = 0; i < studyDirections.Length; i++)
                    {
                        StudyDirection direction = studyDirections[i];
                        command.CommandText = "INSERT INTO study_directions (direction, study_id, objective) VALUES (@direction, @studyId, @objective);";
                        command.Parameters.AddWithValue("@direction", direction.ToString().ToUpperInvariant());
                        command.Parameters.AddWithValue("@studyId", maxLength + 1);
                        command.Parameters.AddWithValue("@objective", i);
                        command.ExecuteNonQuery();
                    }
                }
                connection.Close();
            }
            _nextStudyId = (int)maxLength + 1;
            _studies[_nextStudyId] = new Study.Study(this, _nextStudyId, studyName, studyDirections);
            return _nextStudyId++;
        }

        private void CreateBaseTables()
        {
            var commands = new StringBuilder();
            commands.Append("CREATE TABLE IF NOT EXISTS alembic_version(");
            commands.Append("   version_num VARCHAR(32) PRIMARY KEY NOT NULL");
            commands.Append(");");
            commands.Append("CREATE TABLE IF NOT EXISTS studies(");
            commands.Append("   study_id INTEGER PRIMARY KEY NOT NULL,");
            commands.Append("   study_name VARCHAR(512) NOT NULL");
            commands.Append(");");
            commands.Append("CREATE TABLE IF NOT EXISTS study_directions(");
            commands.Append("   study_direction_id INTEGER PRIMARY KEY NOT NULL,");
            commands.Append("   direction VARCHAR(8) NOT NULL,");
            commands.Append("   study_id INTEGER NOT NULL,");
            commands.Append("   objective INTEGER NOT NULL");
            commands.Append(");");
            commands.Append("CREATE TABLE IF NOT EXISTS study_system_attributes(");
            commands.Append("   study_system_attribute_id INTEGER PRIMARY KEY NOT NULL,");
            commands.Append("   study_id INTEGER,");
            commands.Append("   key VARCHAR(512),");
            commands.Append("   value_json TEXT");
            commands.Append(");");
            commands.Append("CREATE TABLE IF NOT EXISTS study_user_attributes(");
            commands.Append("   study_user_attribute_id INTEGER PRIMARY KEY NOT NULL,");
            commands.Append("   study_id INTEGER,");
            commands.Append("   key VARCHAR(512),");
            commands.Append("   value_json TEXT");
            commands.Append(");");
            commands.Append("CREATE TABLE IF NOT EXISTS trial_heartbeats(");
            commands.Append("   trial_heartbeat_id INTEGER PRIMARY KEY NOT NULL,");
            commands.Append("   trial_id INTEGER NOT NULL,");
            commands.Append("   heartbeat DATETIME NOT NULL");
            commands.Append(");");
            commands.Append("CREATE TABLE IF NOT EXISTS trial_intermediate_values(");
            commands.Append("   trial_intermediate_value_id INTEGER PRIMARY KEY NOT NULL,");
            commands.Append("   trial_id INTEGER NOT NULL,");
            commands.Append("   step INTEGER NOT NULL,");
            commands.Append("   intermediate_value FLOAT,");
            commands.Append("   intermediate_value_type VARCHAR(7) NOT NULL");
            commands.Append(");");
            commands.Append("CREATE TABLE IF NOT EXISTS trial_params(");
            commands.Append("   param_id INTEGER PRIMARY KEY NOT NULL,");
            commands.Append("   trial_id INTEGER,");
            commands.Append("   param_name VARCHAR(512),");
            commands.Append("   param_value FLOAT,");
            commands.Append("   distribution_json TEXT");
            commands.Append(");");
            commands.Append("CREATE TABLE IF NOT EXISTS trial_system_attributes(");
            commands.Append("   trial_system_attribute_id INTEGER PRIMARY KEY NOT NULL,");
            commands.Append("   trial_id INTEGER,");
            commands.Append("   key VARCHAR(512),");
            commands.Append("   value_json TEXT");
            commands.Append(");");
            commands.Append("CREATE TABLE IF NOT EXISTS trial_user_attributes(");
            commands.Append("   trial_user_attribute_id INTEGER PRIMARY KEY NOT NULL,");
            commands.Append("   trial_id INTEGER,");
            commands.Append("   key VARCHAR(512),");
            commands.Append("   value_json TEXT");
            commands.Append(");");
            commands.Append("CREATE TABLE IF NOT EXISTS trial_values(");
            commands.Append("   trial_value_id INTEGER PRIMARY KEY NOT NULL,");
            commands.Append("   trial_id INTEGER NOT NULL,");
            commands.Append("   objective INTEGER NOT NULL,");
            commands.Append("   value FLOAT");
            commands.Append("   value_type VARCHAR(7) NOT NULL");
            commands.Append(");");
            commands.Append("CREATE TABLE IF NOT EXISTS trials(");
            commands.Append("   trial_id INTEGER PRIMARY KEY NOT NULL,");
            commands.Append("   number INTEGER,");
            commands.Append("   study_id INTEGER,");
            commands.Append("   state VARCHAR(8) NOT NULL,");
            commands.Append("   datetime_start DATETIME,");
            commands.Append("   datetime_complete DATETIME");
            commands.Append(");");
            commands.Append("CREATE TABLE IF NOT EXISTS version_info(");
            commands.Append("   version_info_id INTEGER PRIMARY KEY NOT NULL,");
            commands.Append("   schema_version INTEGER,");
            commands.Append("   library_version VARCHAR(256)");
            commands.Append(");");

            using (var connection = new SQLiteConnection(_sqliteConnection.ToString()))
            {
                connection.Open();
                using (var command = new SQLiteCommand(commands.ToString(), connection))
                {
                    command.ExecuteNonQuery();
                }
                SetAlembicVersion(connection);
                SetVersionInfo(connection);
                connection.Close();
            }
        }

        private static void SetVersionInfo(SQLiteConnection connection)
        {
            long columnLength;
            using (var command = new SQLiteCommand("SELECT COUNT(*) FROM version_info", connection))
            {
                columnLength = (long)command.ExecuteScalar();
            }
            if (columnLength > 0)
            {
                return;
            }
            using (var command = new SQLiteCommand(connection))
            {
                command.CommandText = "INSERT INTO version_info (schema_version, library_version) VALUES (@schema, @library);";
                command.Parameters.AddWithValue("@schema", 12);
                command.Parameters.AddWithValue("@library", "3.4.0");
                command.ExecuteNonQuery();
            }
        }

        private static void SetAlembicVersion(SQLiteConnection connection)
        {
            long columnLength;
            using (var command = new SQLiteCommand("SELECT COUNT(*) FROM alembic_version", connection))
            {
                columnLength = (long)command.ExecuteScalar();
            }
            if (columnLength > 0)
            {
                return;
            }
            using (var command = new SQLiteCommand(connection))
            {
                command.CommandText = "INSERT INTO alembic_version (version_num) VALUES (@versionNum);";
                command.Parameters.AddWithValue("@versionNum", "v3.2.0.a");
                command.ExecuteNonQuery();
            }
        }

        public int CreateNewTrial(int studyId, Trial.Trial templateTrial = null)
        {
            if (templateTrial != null)
            {
                throw new NotImplementedException();
            }

            using (var connection = new SQLiteConnection(_sqliteConnection.ToString()))
            {
                connection.Open();
                using (var command = new SQLiteCommand(connection))
                {
                    command.CommandText = "INSERT INTO trials (number, study_id, datetime_start) VALUES (@number, @studyId, @datetimeStart);";
                    command.Parameters.AddWithValue("@number", 0);
                    command.ExecuteNonQuery();
                }
            }

            return 2;
        }

        public void DeleteStudy(int studyId)
        {
            throw new NotImplementedException();
        }

        public Study.Study[] GetAllStudies()
        {
            using (var connection = new SQLiteConnection(_sqliteConnection.ToString()))
            {
                connection.Open();
                using (var command = new SQLiteCommand("SELECT study_id, study_name FROM studies", connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int studyId = reader.GetInt32(0);
                            string studyName = reader.GetString(1);
                            _studies[studyId] = new Study.Study(this, studyId, studyName, GetStudyDirections(studyId));
                        }
                    }
                }
                connection.Close();
            }

            return _studies.Values.ToArray();
        }

        public void RemoveSession()
        {
            throw new NotImplementedException();
        }

        public void SetStudySystemAttr(int studyId, string key, object value)
        {
            throw new NotImplementedException();
        }

        public void SetStudyUserAttr(int studyId, string key, object value)
        {
            throw new NotImplementedException();
        }

        public void SetTrailParam(int trialId, string paramName, double paramValueInternal, object distribution)
        {
            throw new NotImplementedException();
        }

        public void SetTrialIntermediateValue(int trialId, int step, double intermediateValue)
        {
            throw new NotImplementedException();
        }

        public bool SetTrialStateValue(int trialId, TrialState state, double[] values = null)
        {
            throw new NotImplementedException();
        }

        public void SetTrialSystemAttr(int trialId, string key, object value)
        {
            throw new NotImplementedException();
        }

        public void SetTrialUserAttr(int trialId, string key, object value)
        {
            throw new NotImplementedException();
        }

        public int GetStudyIdFromName(string studyName)
        {
            throw new NotImplementedException();
        }

        public string GetStudyNameFromId(int studyId)
        {
            throw new NotImplementedException();
        }

        public StudyDirection[] GetStudyDirections(int studyId)
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, object> GetStudyUserAttrs(int studyId)
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, object> GetStudySystemAttrs(int studyId)
        {
            throw new NotImplementedException();
        }

        public int GetTrialIdFromStudyIdTrialNumber(int studyId, int trialNumber)
        {
            throw new NotImplementedException();
        }

        public int GetTrialNumberFromId(int trialId)
        {
            throw new NotImplementedException();
        }

        public double GetTrialParam(int trialId, string paramName)
        {
            throw new NotImplementedException();
        }

        public Trial.Trial GetTrial(int trialId)
        {
            throw new NotImplementedException();
        }

        public Trial.Trial[] GetAllTrials(int studyId, bool deepcopy = true)
        {
            throw new NotImplementedException();
        }

        public int GetNTrials(int studyId)
        {
            throw new NotImplementedException();
        }

        public Trial.Trial GetBestTrial(int studyId)
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, object> GetTrialParams(int trialId)
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, object> GetTrialUserAttrs(int trialId)
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, object> GetTrialSystemAttrs(int trialId)
        {
            throw new NotImplementedException();
        }
    }
}
