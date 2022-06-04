using System.Collections.Generic;

namespace Tunny.Optimization
{
    public class EvaluatedGHResult
    {
        public List<double> ObjectiveValues { get; set; }
        public List<string> GeometryJson { get; set; }
    }
}
