using System.Collections.Generic;

using Tunny.Input;

namespace Tunny.PostProcess
{
    public class TrialGrasshopperItems
    {
        public Objective Objectives { get; set; }
        public string[] GeometryJson { get; set; }
        public Dictionary<string, List<string>> Attribute { get; set; }
        public Artifact Artifacts { get; set; }
    }
}
