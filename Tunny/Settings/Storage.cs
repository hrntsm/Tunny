namespace Tunny.Settings
{
    public class Storage
    {
        public string Path { get; set; } = "/fish.log";
        public StorageType Type { get; set; } = StorageType.Journal;
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
