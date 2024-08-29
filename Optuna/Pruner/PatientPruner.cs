namespace Optuna.Pruner
{
    /// <summary>
    /// https://optuna.readthedocs.io/en/stable/reference/generated/optuna.pruners.PatientPruner.html
    /// </summary>
    public class PatientPruner : IPruner
    {
        public IPruner BasePruner { get; set; }
        public int Patience { get; set; }
        public double MinDelta { get; set; }

        public PatientPruner()
        {
        }

        public PatientPruner(IPruner basePruner, int patience, double minDelta)
        {
            BasePruner = basePruner;
            Patience = patience;
            MinDelta = minDelta;
        }

        public dynamic ToPython(dynamic optuna)
        {
            return optuna.pruners.PatientPruner(
                base_pruner: BasePruner.ToPython(optuna),
                patience: Patience,
                min_delta: MinDelta
            );
        }
    }
}
