using System;
using System.IO;
namespace Tunny.Storage
{
    public class StorageHandler : IStorage
    {
        public dynamic Storage { get; set; }

        public dynamic CreateNewStorage(string storagePath = null)
        {
            ICreateStorage storage;

            switch (Path.GetExtension(storagePath))
            {
                case null:
                    storage = new InMemoryStorage();
                    break;
                case ".sqlite3":
                case ".db":
                    storage = new SqliteStorage();
                    break;
                case ".log":
                    storage = new JournalStorage();
                    break;
                default:
                    throw new ArgumentException("Storage type not supported");
            }
            Storage = storage.CreateNewStorage(storagePath);
            return Storage;
        }

        public void DuplicateStudyInStorage(string fromStudyName, string toStudyName, string storagePath)
        {
            IStorage storage;

            switch (Path.GetExtension(storagePath))
            {
                case ".sqlite3":
                case ".db":
                    storage = new SqliteStorage();
                    break;
                case ".log":
                    storage = new JournalStorage();
                    break;
                default:
                    throw new ArgumentException("Storage type not supported");
            }
            storage.DuplicateStudyInStorage(fromStudyName, toStudyName, storagePath);
        }

        public StudySummary[] GetStudySummaries(string storagePath)
        {
            IStorage storage;

            switch (Path.GetExtension(storagePath))
            {
                case ".sqlite3":
                case ".db":
                    storage = new SqliteStorage();
                    break;
                case ".log":
                    storage = new JournalStorage();
                    break;
                default:
                    throw new ArgumentException("Storage type not supported");
            }

            return storage.GetStudySummaries(storagePath);
        }
    }
}
