namespace Tunny.Settings
{
    public class Storage
    {
        public string Path { get; set; } = "/Fish.db";
        public StorageType Type { get; set; } = StorageType.Sqlite;
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
