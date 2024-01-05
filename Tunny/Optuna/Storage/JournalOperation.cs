namespace Tunny.Optuna.Storage
{
    public enum JournalOperation
    {
        CreateStudy = 0,
        DeleteStudy = 1,
        SetStudyUserAttr = 2,
        SetStudySystemAttr = 3,
        CreateTrial = 4,
        SetTrialParam = 5,
        SetTrialStateValues = 6,
        SetTrialIntermediateValue = 7,
        SetTrialUserAttr = 8,
        SetTrialSystemAttr = 9,
    }
}
