using System.Collections.Generic;

using Tunny.Type;

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
        public HumanInTheLoop.Slider HitlSlider { get; set; }
        public HumanInTheLoop.Preferential HitlPreferential { get; set; }

        public OptimizationHandlingInfo(int nTrials, double timeout, dynamic study, dynamic storage,
                                        dynamic artifactBackend, Dictionary<string, FishEgg> enqueueItems,
                                        string[] objectiveNames)
        {
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
