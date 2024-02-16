using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;

using Optuna.Study;
using Optuna.Trial;

namespace Optuna.Storage.RDB
{
    public class SqliteStorage : BaseStorage
    {
        private int _nextStudyId;
        private readonly SQLiteConnectionStringBuilder _sqliteConnection;

        public SqliteStorage(string filePath, bool createIfNotExist = false)
        {
            CheckFileExist(filePath, createIfNotExist);

            _sqliteConnection = new SQLiteConnectionStringBuilder
            {
                DataSource = filePath,
                Version = 3,
            };
        }

        private static void CheckFileExist(string filePath, bool createIfNotExist)
        {
            if (!File.Exists(filePath))
            {
                if (!createIfNotExist)
                {
                    throw new ArgumentException("The specified database does not exist.");
                }
                else
                {
                    SQLiteConnection.CreateFile(filePath);
                }
            }
        }

        public override void CheckTrialIsUpdatable(int trialId, TrialState trialState)
        {
            throw new System.NotImplementedException();
        }

        public override int CreateNewStudy(StudyDirection[] studyDirections, string studyName)
        {
            using (var connection = new SQLiteConnection(_sqliteConnection.ToString()))
            {
                connection.Open();
                string createTableQuery = "CREATE TABLE IF NOT EXISTS studies (study_id INTEGER, study_name VARCHAR(512));";
                using (var command = new SQLiteCommand(createTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
                using (var command = new SQLiteCommand(connection))
                {
                    command.CommandText = "INSERT INTO studies (study_name) VALUES (@studyName);";
                    command.Parameters.AddWithValue("@studyName", studyName);
                    command.ExecuteNonQuery();
                }
                return _nextStudyId++;
            }
        }

        public override int CreateNewTrial(int studyId, Trial.Trial templateTrial = null)
        {
            throw new System.NotImplementedException();
        }

        public override void DeleteStudy(int studyId)
        {
            throw new System.NotImplementedException();
        }

        public override Study.Study[] GetAllStudies()
        {
            throw new System.NotImplementedException();
        }

        public override Trial.Trial[] GetAllTrials(int studyId, bool deepcopy = true)
        {
            throw new System.NotImplementedException();
        }

        public override Trial.Trial GetBestTrial(int studyId)
        {
            throw new System.NotImplementedException();
        }

        public override int GetNTrials(int studyId)
        {
            throw new System.NotImplementedException();
        }

        public override StudyDirection[] GetStudyDirections(int studyId)
        {
            throw new System.NotImplementedException();
        }

        public override int GetStudyIdFromName(string studyName)
        {
            throw new System.NotImplementedException();
        }

        public override string GetStudyNameFromId(int studyId)
        {
            throw new System.NotImplementedException();
        }

        public override Dictionary<string, object> GetStudySystemAttrs(int studyId)
        {
            throw new System.NotImplementedException();
        }

        public override Dictionary<string, object> GetStudyUserAttrs(int studyId)
        {
            throw new System.NotImplementedException();
        }

        public override Trial.Trial GetTrial(int trialId)
        {
            throw new System.NotImplementedException();
        }

        public override int GetTrialIdFromStudyIdTrialNumber(int studyId, int trialNumber)
        {
            throw new System.NotImplementedException();
        }

        public override int GetTrialNumberFromId(int trialId)
        {
            throw new System.NotImplementedException();
        }

        public override double GetTrialParam(int trialId, string paramName)
        {
            throw new System.NotImplementedException();
        }

        public override Dictionary<string, object> GetTrialParams(int trialId)
        {
            throw new System.NotImplementedException();
        }

        public override Dictionary<string, object> GetTrialSystemAttrs(int trialId)
        {
            throw new System.NotImplementedException();
        }

        public override Dictionary<string, object> GetTrialUserAttrs(int trialId)
        {
            throw new System.NotImplementedException();
        }

        public override void RemoveSession()
        {
            throw new System.NotImplementedException();
        }

        public override void SetStudySystemAttr(int studyId, string key, object value)
        {
            throw new System.NotImplementedException();
        }

        public override void SetStudyUserAttr(int studyId, string key, object value)
        {
            throw new System.NotImplementedException();
        }

        public override void SetTrailParam(int trialId, string paramName, double paramValueInternal, object distribution)
        {
            throw new System.NotImplementedException();
        }

        public override void SetTrialIntermediateValue(int trialId, int step, double intermediateValue)
        {
            throw new System.NotImplementedException();
        }

        public override bool SetTrialStateValue(int trialId, TrialState state, double[] values = null)
        {
            throw new System.NotImplementedException();
        }

        public override void SetTrialSystemAttr(int trialId, string key, object value)
        {
            throw new System.NotImplementedException();
        }

        public override void SetTrialUserAttr(int trialId, string key, object value)
        {
            throw new System.NotImplementedException();
        }
    }
}
