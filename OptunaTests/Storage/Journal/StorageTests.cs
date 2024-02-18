using System.IO;

using Xunit;

namespace Optuna.Storage.Journal.Tests
{
    public class JournalStorageTests
    {
        [Fact()]
        public void MakeFileIfNotExistTest()
        {
            string path = @"TestFile/created.log";
            _ = new JournalStorage(path, true);
            Assert.True(File.Exists(path));
            File.Delete(path);
        }
    }
}
