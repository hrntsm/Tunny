using System;
using System.Collections.Generic;
using System.IO;

using Optuna.Study;

using Xunit;

namespace Optuna.Storage.RDB.Tests
{
    public class SqliteStorageTests : IDisposable
    {
        private readonly List<string> _tempDBPaths = new();

        [Fact()]
        public void NoFileTest()
        {
            string path = @"TestFile/no_exist_file.db";
            Assert.Throws<ArgumentException>(() => new SqliteStorage(path));
        }

        [Fact()]
        public void CreateFileTest()
        {
            string path = @"TestFile/created.db";
            _tempDBPaths.Add(path);
            _ = new SqliteStorage(path, true);
            Assert.True(File.Exists(path));
        }

        [Fact()]
        public void AAA()
        {
            string path = @"TestFile/new_study.db";
            _tempDBPaths.Add(path);
            var storage = new SqliteStorage(path, true);
            storage.CreateNewStudy(new StudyDirection[] { StudyDirection.Maximize }, "test");
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
