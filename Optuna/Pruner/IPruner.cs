namespace Optuna.Pruner
{
    public interface IPruner
    {
        dynamic ToPython(dynamic optuna);
    }
}
