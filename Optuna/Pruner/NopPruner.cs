namespace Optuna.Pruner
{
    /// <summary>
    /// https://optuna.readthedocs.io/en/stable/reference/generated/optuna.pruners.NopPruner.html#optuna.pruners.NopPruner
    /// </summary>
    public class NopPruner : IPruner
    {
        public dynamic ToPython(dynamic optuna)
        {
            return optuna.pruners.NopPruner();
        }
    }
}
