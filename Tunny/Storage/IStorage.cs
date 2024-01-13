using Optuna.Study;

namespace Tunny.Storage
{
    public interface IStorage : ICreateStorage
    {
        void DuplicateStudyInStorage(string fromStudyName, string toStudyName, Settings.Storage storageSetting);
        StudySummary[] GetStudySummaries(string storagePath);
    }

    public interface ICreateStorage
    {
        dynamic Storage { get; set; }
        dynamic CreateNewStorage(bool useInnerPythonEngine, Settings.Storage storageSetting);
    }
}
