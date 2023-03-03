namespace Tunny.Storage
{
    public interface IStorage : ICreateStorage
    {
        void DuplicateStudyInStorage(string fromStudyName, string toStudyName, string storagePath);
        StudySummary[] GetStudySummaries(string storagePath);
    }

    public interface ICreateStorage
    {
        dynamic Storage { get; set; }
        dynamic CreateNewStorage(bool useInnerPythonEngine, string storagePath);
    }
}
