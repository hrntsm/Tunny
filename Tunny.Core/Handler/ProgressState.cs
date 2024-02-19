using System;
using System.Collections.Generic;

using Tunny.Core.Input;

namespace Tunny.Core.Handler
{
    public class ProgressState
    {
        public IList<Parameter> Parameter { get; set; }
        public int TrialNumber { get; set; }
        public int ObjectiveNum { get; set; }
        public double[][] BestValues { get; set; }
        public double HypervolumeRatio { get; set; }
        public TimeSpan EstimatedTimeRemaining { get; set; }
    }
}
