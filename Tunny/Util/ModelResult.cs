using System.Collections.Generic;

namespace Tunny.Util
{
    public class ModelResult
    {
        public int Number { get; set; }
        public string Draco { get; set; }
        public Dictionary<string, double> Variables { get; set; }
        public double[] Objectives { get; set; }
    }
}
