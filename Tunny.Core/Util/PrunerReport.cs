using System.IO;

using Newtonsoft.Json;

namespace Tunny.Core.Util
{
    public class PrunerReport
    {
        public double Value { get; set; }
        public string Attribute { get; set; }

        public PrunerReport()
        {
        }

        public static PrunerReport Deserialize(string path)
        {
            string value = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<PrunerReport>(value);
        }
    }
}
