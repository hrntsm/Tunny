using System;

namespace Optuna.Trial
{
    public class Trial
    {
        public int TrialId { get; set; }
        public int Number { get; set; }
        public TrialState State { get; set; }
        public DateTime DatetimeStart { get; set; }
        public DateTime DatetimeComplete { get; set; }
    }
}
