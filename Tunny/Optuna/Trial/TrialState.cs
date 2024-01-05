namespace Tunny.Optuna.Trial
{
    public enum TrialState
    {
        RUNNING,
        WAITING,
        COMPLETE,
        PRUNED,
        FAIL
    }
}
