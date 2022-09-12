using System.Collections.Generic;

namespace Tunny.Util
{
    public class EvaluatedGHResult
    {
        public List<double> ObjectiveValues { get; set; }
        public List<string> GeometryJson { get; set; }
        public Dictionary<string, List<string>> Attribute { get; set; }
    }
}
