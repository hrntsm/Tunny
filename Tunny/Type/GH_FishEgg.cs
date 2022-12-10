using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

using GH_IO.Serialization;

using Grasshopper.Kernel.Types;

namespace Tunny.Type
{
    public class GH_FishEgg : GH_Goo<Dictionary<string, FishEgg>>
    {
        public GH_FishEgg()
        {
        }

        public GH_FishEgg(Dictionary<string, FishEgg> internalData) : base(internalData)
        {
        }

        public GH_FishEgg(GH_Goo<Dictionary<string, FishEgg>> other) : base(other)
        {
        }

        public override bool IsValid => Value != null;
        public override string TypeName => "FishEgg";
        public override string TypeDescription => "DictionaryGoo for Tunny fish egg";
        public override IGH_Goo Duplicate() => new GH_FishEgg { Value = Value };

        public override bool Read(GH_IReader reader)
        {
            string base64 = string.Empty;
            if (reader.TryGetString("FishEgg_bin", ref base64))
            {
                Value = FromBase64(base64);
            }
            return base.Read(reader);
        }

        public override bool Write(GH_IWriter writer)
        {
            writer.SetString("FishEgg_bin", ToBase64(Value));
            return base.Write(writer);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (KeyValuePair<string, FishEgg> attr in Value)
            {
                string valueStrings = string.Empty;
                valueStrings = string.Join(", ", attr.Value.Values.Select(v => string.Format("{0, 6}", v)));
                sb.AppendLine(attr.Key + ":\n  " + valueStrings);
            }
            return sb.ToString();
        }

        public override bool CastFrom(object source)
        {
            if (source is Dictionary<string, FishEgg> dict)
            {
                Value = dict;
                return true;
            }
            else
            {
                return false;
            }
        }

        public override bool CastTo<T>(ref T target)
        {
            target = default;
            if (typeof(T).IsAssignableFrom(typeof(Dictionary<string, FishEgg>)))
            {
                target = (T)(object)Value;
                return true;
            }
            else
            {
                return false;
            }
        }

        private static Dictionary<string, FishEgg> FromBase64(string base64)
        {
            byte[] bytes = Convert.FromBase64String(base64);
            using (var ms = new MemoryStream(bytes))
            {
                return (Dictionary<string, FishEgg>)new BinaryFormatter().Deserialize(ms);
            }
        }

        private static string ToBase64(Dictionary<string, FishEgg> value)
        {
            using (var ms = new MemoryStream())
            {
                new BinaryFormatter().Serialize(ms, value);
                return Convert.ToBase64String(ms.ToArray());
            }
        }
    }
}
