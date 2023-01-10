using System;
using System.Collections.Generic;

namespace Tunny.Handler
{
    public class ProgressState
    {
        public IList<decimal> Values { get; set; }
        public int TrialNumber { get; set; }
        public int ObjectiveNum { get; set; }
        public double[][] BestValues { get; set; }
        public double HypervolumeRatio { get; set; }
        public TimeSpan EstimatedTimeRemaining { get; set; }
    }
}
