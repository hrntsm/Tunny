using Optuna.Study;

namespace Tunny.Core.Storage
{
    public interface ITStorage : ICreateTStorage
    {
        void DuplicateStudyInStorage(string fromStudyName, string toStudyName, Settings.Storage storageSetting);
        StudySummary[] GetStudySummaries(string storagePath);
    }

    public interface ICreateTStorage
    {
        dynamic Storage { get; set; }
        dynamic CreateNewTStorage(bool useInnerPythonEngine, Settings.Storage storageSetting);
    }
}
