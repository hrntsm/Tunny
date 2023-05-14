using System;

using Tunny.Storage;

namespace Tunny.Settings
{
    public class Storage
    {
        public string Path { get; set; } = "/fish.log";
        public StorageType Type { get; set; } = StorageType.Journal;

        public string GetOptunaStoragePath()
        {
            switch (Type)
            {
                case StorageType.InMemory:
                    return string.Empty;
                case StorageType.Sqlite:
                    return "sqlite:///" + Path;
                case StorageType.Postgres:
                case StorageType.MySql:
                    throw new NotImplementedException();
                case StorageType.Journal:
                    return Path;
                default:
                    throw new NotImplementedException();
            }
        }

        public string GetOptunaStoragePathByExtension()
        {
            switch (System.IO.Path.GetExtension(Path))
            {
                case null:
                    return string.Empty;
                case ".sqlite3":
                case ".db":
                    return "sqlite:///" + Path;
                case ".log":
                    return Path;
                default:
                    throw new NotImplementedException();
            }
        }

        public string GetOptunaStorageCommandLinePathByExtension()
        {
            switch (System.IO.Path.GetExtension(Path))
            {
                case null:
                    return string.Empty;
                case ".sqlite3":
                case ".db":
                    return @"sqlite:///" + $"\"{Path}\"";
                case ".log":
                    return $"\"{Path}\"";
                default:
                    throw new NotImplementedException();
            }
        }

        public dynamic CreateNewOptunaStorage(bool useInnerPythonEngine)
        {
            dynamic storage;
            switch (Type)
            {
                case StorageType.InMemory:
                    storage = new InMemoryStorage().CreateNewStorage(useInnerPythonEngine, this);
                    break;
                case StorageType.Sqlite:
                    storage = new SqliteStorage().CreateNewStorage(useInnerPythonEngine, this);
                    break;
                case StorageType.Journal:
                    storage = new JournalStorage().CreateNewStorage(useInnerPythonEngine, this);
                    break;
                default:
                    throw new ArgumentException("Storage type is not defined.");
            }

            return storage;
        }
    }

    public enum StorageType
    {
        InMemory,
        Sqlite,
        Postgres,
        MySql,
        Journal,
    }
}
