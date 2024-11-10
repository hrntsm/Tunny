using System;
using System.IO;

namespace Optuna.Storage
{
    public static class StorageHelper
    {
        public static IOptunaStorage GetStorage(string path)
        {
            string ext = Path.GetExtension(path);
            IOptunaStorage storage;
            switch (ext)
            {
                case ".db":
                case ".sqlite":
                    storage = new RDB.SqliteStorage(path, true);
                    break;
                case ".log":
                    storage = new Journal.JournalStorage(path, true);
                    break;
                default:
                    throw new ArgumentException("Storage type not supported");
            }

            return storage;
        }
    }
}
