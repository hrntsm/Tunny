using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using Newtonsoft.Json;

using Rhino.Geometry;

namespace Tunny.GHType
{
    [Serializable]
    public class Fish
    {
        public int ModelNumber;
        public List<GeometryBase> Geometries;
        public Dictionary<string, double> Variables;
        public Dictionary<string, double> Objectives;

        public string SerializeToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        public static Fish DeserializeFromJson(string json)
        {
            return JsonConvert.DeserializeObject<Fish>(json);
        }
        public static string ToBase64(Fish fish)
        {
            using (var ms = new MemoryStream())
            {
                new BinaryFormatter().Serialize(ms, fish);
                return Convert.ToBase64String(ms.ToArray());
            }
        }

        public static Fish FromBase64(string base64String)
        {
            byte[] bytes = Convert.FromBase64String(base64String);
            using (var ms = new MemoryStream(bytes))
            {
                return (Fish)new BinaryFormatter().Deserialize(ms);
            }

        }
    }
}
