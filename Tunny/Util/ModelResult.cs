using System.Collections.Generic;

namespace Tunny.Util
{
    public class ModelResult
    {
        public int Number { get; set; }
        public string[] GeometryJson { get; set; }
        public Dictionary<string, double> Variables { get; set; }
        public double[] Objectives { get; set; }
    }
}
