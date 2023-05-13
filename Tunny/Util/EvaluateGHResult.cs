using System.Collections.Generic;
using System.Drawing;

namespace Tunny.Util
{
    public class EvaluatedGHResult
    {
        public double[] ObjectiveValues { get; set; }
        public string[] GeometryJson { get; set; }
        public Dictionary<string, List<string>> Attribute { get; set; }
        public Bitmap[] ObjectiveImages { get; set; }
    }
}
