using System.Collections.Generic;

using Tunny.Type;
using Tunny.Util;

namespace Tunny.Solver
{
    sealed internal class OptimizationHandlingInfo
    {
        public int NTrials { get; set; }
        public double Timeout { get; set; }
        public dynamic Study { get; set; }
        public dynamic Storage { get; set; }
        public dynamic ArtifactBackend { get; set; }
        public Dictionary<string, FishEgg> EnqueueItems { get; set; }
        public string[] ObjectiveNames { get; set; }
        public HumanInTheLoop.HumanSliderInput HumanSliderInput { get; set; }
        public HumanInTheLoop.Preferential Preferential { get; set; }

        public OptimizationHandlingInfo(int nTrials, double timeout, dynamic study, dynamic storage,
                                        dynamic artifactBackend, Dictionary<string, FishEgg> enqueueItems,
                                        string[] objectiveNames)
        {
            TLog.MethodStart();
            NTrials = nTrials;
            Timeout = timeout;
            Study = study;
            Storage = storage;
            ArtifactBackend = artifactBackend;
            EnqueueItems = enqueueItems;
            ObjectiveNames = objectiveNames;
        }
    }
}
