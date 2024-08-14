using System;
using System.Collections.Generic;
using System.IO;

using Newtonsoft.Json;

namespace Tunny.Util.RhinoComputeWrapper
{
    public static class GrasshopperCompute
    {
        static string ApiAddress()
        {
            return "grasshopper";
        }

        public static List<GrasshopperDataTree> EvaluateDefinition(string definition, IEnumerable<GrasshopperDataTree> trees)
        {
            var schema = new Schema();
            string algo = null;
            string pointer = null;
            if (definition.StartsWith("http", StringComparison.OrdinalIgnoreCase))
            {
                pointer = definition;
            }
            else
            {
                byte[] bytes = File.ReadAllBytes(definition);
                algo = Convert.ToBase64String(bytes);
            }

            schema.Algo = algo;
            schema.Pointer = pointer;
            schema.Values = new List<GrasshopperDataTree>(trees);
            Schema rc = ComputeServer.Post<Schema>(ApiAddress(), schema);

            return rc.Values;
        }

        private class Schema
        {
            public Schema() { }

            [JsonProperty(PropertyName = "algo")]
            public string Algo { get; set; }

            [JsonProperty(PropertyName = "pointer")]
            public string Pointer { get; set; }

            [JsonProperty(PropertyName = "values")]
            public List<GrasshopperDataTree> Values { get; set; } = new List<GrasshopperDataTree>();
        }
    }
}
