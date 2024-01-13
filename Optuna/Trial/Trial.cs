using System;
using System.Collections.Generic;

namespace Optuna.Trial
{
    public class Trial
    {
        public int TrialId { get; set; }
        public int Number { get; set; }
        public TrialState State { get; set; }
        public DateTime DatetimeStart { get; set; }
        public DateTime DatetimeComplete { get; set; }
        public double[] Values { get; set; }
        public Dictionary<string, object> Params { get; set; }
        public Dictionary<string, object> UserAttrs { get; set; }
        public Dictionary<string, object> SystemAttrs { get; set; }

        public Trial()
        {
            Params = new Dictionary<string, object>();
            UserAttrs = new Dictionary<string, object>();
            SystemAttrs = new Dictionary<string, object>();
        }
    }
}
