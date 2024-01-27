namespace Optuna.Trial
{
    public enum TrialState
    {
        RUNNING = 0,
        COMPLETE = 1,
        PRUNED = 2,
        FAIL = 3,
        WAITING = 4,
    }
}
