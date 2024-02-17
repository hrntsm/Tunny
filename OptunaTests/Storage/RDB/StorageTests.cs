using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;

using Optuna.Study;

using Xunit;

namespace Optuna.Storage.RDB.Tests
{
    public class CreateStorage
    {
        public CreateStorage()
        {
            string path = @"TestFile/created.db";
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            _ = new SqliteStorage(path, true);
        }
    }

    public class SqliteStorageTests : IClassFixture<CreateStorage>, IDisposable
    {
        private readonly string _createFilePath = @"TestFile/created.db";
        private readonly string _existFilePath = @"TestFile/sqlite.db";
        private readonly List<string> _tempDBPaths = new();

        [Fact()]
        public void NoFileTest()
        {
            string path = @"TestFile/no_exist_file.db";
            Assert.Throws<ArgumentException>(() => new SqliteStorage(path));
        }

        [Fact()]
        public void CreateNewStorageTest()
        {
            var storage = new SqliteStorage(_createFilePath, true);
            var studyDirections = new StudyDirection[] { StudyDirection.Maximize, StudyDirection.Minimize };
            string studyName = "create_new_study_test";
            int id = storage.CreateNewStudy(studyDirections, studyName);
            Assert.Equal(1, id);

            var sqlConnection = new SQLiteConnectionStringBuilder
            {
                DataSource = _createFilePath,
                Version = 3,
            };

            CreateNewStudyTest(studyDirections, studyName, sqlConnection);
        }

        private static void CreateNewStudyTest(StudyDirection[] studyDirections, string studyName, SQLiteConnectionStringBuilder sqlConnection)
        {
            using (var connection = new SQLiteConnection(sqlConnection.ToString()))
            {
                connection.Open();
                using (var command = new SQLiteCommand(connection))
                {
                    command.CommandText = "SELECT study_name FROM studies";
                    SQLiteDataReader reader = command.ExecuteReader();
                    Assert.True(reader.Read());
                    Assert.Equal(studyName, reader.GetString(0));
                }

                using (var command = new SQLiteCommand(connection))
                {
                    command.CommandText = "SELECT direction FROM study_directions";
                    SQLiteDataReader reader = command.ExecuteReader();
                    Assert.True(reader.Read());
                    Assert.Equal(studyDirections[0].ToString().ToUpperInvariant(), reader.GetString(0));
                    Assert.True(reader.Read());
                    Assert.Equal(studyDirections[1].ToString().ToUpperInvariant(), reader.GetString(0));
                }
                connection.Close();
            }
        }

        [Fact()]
        public void CreateNewStudyWithSameNameTest()
        {
            string path = @"TestFile/new_study_same_name.db";
            _tempDBPaths.Add(path);
            var storage = new SqliteStorage(path, true);
            var studyDirections = new StudyDirection[] { StudyDirection.Maximize, StudyDirection.Minimize };
            string studyName = "create_new_study_test";
            storage.CreateNewStudy(studyDirections, studyName);
            Assert.Throws<InvalidOperationException>(() => storage.CreateNewStudy(studyDirections, studyName));
        }

        [Fact()]
        public void GetAllStudiesTest()
        {
            var storage = new SqliteStorage(_existFilePath);
        }

        public void Dispose()
        {
            foreach (string path in _tempDBPaths)
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
        }
    }
}
