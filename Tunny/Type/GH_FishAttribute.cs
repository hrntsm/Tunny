using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

using GH_IO.Serialization;

using Grasshopper.Kernel.Types;

using Rhino.Geometry;

namespace Tunny.Type
{
    public class GH_FishAttribute : GH_Goo<Dictionary<string, object>>
    {
        public GH_FishAttribute()
        {
        }

        public GH_FishAttribute(Dictionary<string, object> internal_data) : base(internal_data)
        {
        }

        public GH_FishAttribute(GH_Goo<Dictionary<string, object>> other) : base(other)
        {
        }

        public override bool IsValid => Value != null;
        public override string TypeName => "FishAttribute";
        public override string TypeDescription => "DictionaryGoo for grasshopper";
        public override IGH_Goo Duplicate()
        {
            return new GH_FishAttribute() { Value = Value };
        }

        public override bool Read(GH_IReader reader)
        {
            string base64 = string.Empty;
            if (reader.TryGetString("FishAttribute_bin", ref base64))
            {
                Value = FromBase64(base64);
            }
            return base.Read(reader);
        }

        public override bool Write(GH_IWriter writer)
        {
            writer.SetString("FishAttribute_bin", ToBase64(Value));
            return base.Write(writer);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (KeyValuePair<string, object> attr in Value)
            {
                string valueStrings = string.Empty;
                switch (attr.Value)
                {
                    case List<object> obj:
                        valueStrings = string.Join(", ", obj);
                        break;
                    case List<string> str:
                        valueStrings = string.Join(", ", str);
                        break;
                    case List<GeometryBase> geo:
                        valueStrings = string.Join(", ", GeometryBaseToGoo(geo));
                        break;
                }
                sb.AppendLine("  " + attr.Key + ": " + valueStrings);
            }
            return sb.ToString();
        }

        private List<string> GeometryBaseToGoo(List<GeometryBase> geometryBase)
        {
            var list = new List<string>();
            foreach (GeometryBase geo in geometryBase)
            {
                switch (geo)
                {
                    case Mesh mesh:
                        list.Add(new GH_Mesh(mesh).ToString());
                        break;
                    case Curve curve:
                        list.Add(new GH_Curve(curve).ToString());
                        break;
                    case Brep brep:
                        list.Add(new GH_Brep(brep).ToString());
                        break;
                    case Surface surface:
                        list.Add(new GH_Surface(surface).ToString());
                        break;
                    case SubD subD:
                        list.Add(new GH_SubD(subD).ToString());
                        break;
                    default:
                        throw new Exception("Tunny doesn't handle this type of geometry");
                }
            }
            return list;
        }

        public override bool CastFrom(object source)
        {
            if (source is Dictionary<string, object> dict)
            {
                Value = dict;
                return true;
            }
            else
                return false;
        }

        public override bool CastTo<Q>(ref Q target)
        {
            target = default;
            if (typeof(Q).IsAssignableFrom(typeof(Dictionary<string, object>)))
            {
                target = (Q)(object)Value;
                return true;
            }
            else
                return false;
        }

        private Dictionary<string, object> FromBase64(string base64)
        {
            byte[] bytes = Convert.FromBase64String(base64);
            using (var ms = new MemoryStream(bytes))
            {
                return (Dictionary<string, object>)new BinaryFormatter().Deserialize(ms);
            }
        }

        private string ToBase64(Dictionary<string, object> value)
        {
            using (var ms = new MemoryStream())
            {
                new BinaryFormatter().Serialize(ms, value);
                return Convert.ToBase64String(ms.ToArray());
            }
        }
    }
}
