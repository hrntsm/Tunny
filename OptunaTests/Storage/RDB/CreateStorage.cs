using System.IO;

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
}
