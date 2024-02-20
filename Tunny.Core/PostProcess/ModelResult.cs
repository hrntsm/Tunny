using System.Collections.Generic;

namespace Tunny.Core.PostProcess
{
    public class ModelResult
    {
        public int Number { get; set; }
        public double[] Objectives { get; set; }
        public Dictionary<string, object> Variables { get; set; }
        public Dictionary<string, List<string>> Attributes { get; set; }
    }
}
