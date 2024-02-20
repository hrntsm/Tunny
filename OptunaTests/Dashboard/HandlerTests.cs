using System;
using System.IO;

using Optuna.Dashboard;

using Xunit;

namespace OptunaTests.Dashboard
{
    public class HandlerTests
    {
        [Fact]
        public void FileNotFoundTest()
        {
            Assert.Throws<FileNotFoundException>(() => new Handler("TestFile/notFound.dll", "TestFile/sqlite.db"));
            Assert.Throws<FileNotFoundException>(() => new Handler("./optuna.dll", "TestFile/notFound.db"));
        }

        [Fact]
        public void WrongStorageFileTest()
        {
            Assert.Throws<ArgumentException>(() => new Handler("./optuna.dll", "./optuna.dll"));
        }

        [Fact]
        public void CreateArtifactDirTest()
        {
            _ = new Handler("./optuna.dll", "TestFile/sqlite.db");
            Assert.True(Directory.Exists("TestFile/artifacts"));
            Directory.Delete("TestFile/artifacts");

            string artifactDir = "./artifactDir";
            _ = new Handler("./optuna.dll", "TestFile/journal.log", artifactDir);
            Assert.True(Directory.Exists(artifactDir));
            Directory.Delete(artifactDir);
        }
    }
}

