using System.Collections.Generic;

using Tunny.Type;

namespace Tunny.Solver
{
    internal class RunOptimizeSettings
    {
        public int NTrials { get; set; }
        public double Timeout { get; set; }
        public dynamic Study { get; set; }
        public dynamic Storage { get; set; }
        public Dictionary<string, FishEgg> EnqueueItems { get; set; }
        public string[] ObjectiveNames { get; set; }
        public HumanInTheLoop HumanInTheLoop { get; set; }

        public RunOptimizeSettings(int nTrials, double timeout, dynamic study, dynamic storage, Dictionary<string, FishEgg> enqueueItems, string[] objectiveNames)
        {
            NTrials = nTrials;
            Timeout = timeout;
            Study = study;
            Storage = storage;
            EnqueueItems = enqueueItems;
            ObjectiveNames = objectiveNames;
        }
    }
}
