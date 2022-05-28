using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

using GH_IO.Serialization;

using Grasshopper.Kernel.Types;

using Newtonsoft.Json;

using Rhino.Geometry;

namespace Tunny.GHType
{
    public class GH_CFish : GH_Goo<CFish>
    {
        public GH_CFish()
        {
        }

        public GH_CFish(CFish cFish)
            : base(cFish)
        {
        }

        public GH_CFish(GH_CFish other)
            : base(other.Value)
        {
        }

        public override bool IsValid => true;
        public override string TypeName => "CFish";
        public override string TypeDescription => "Tunny optimizer result.";
        public override bool CastFrom(object source) => false;
        public override IGH_GooProxy EmitProxy() => new GH_CFishProxy(this);
        public override IGH_Goo Duplicate() => new GH_CFish(this);
        public override bool Read(GH_IReader reader)
        {
            string base64 = string.Empty;
            if (reader.TryGetString("CFish_bin", ref base64))
            {
                Value = CFish.FromBase64(base64);
            }
            return base.Read(reader);
        }

        public override bool Write(GH_IWriter writer)
        {
            writer.SetString("CFish_bin", CFish.ToBase64(Value));
            return base.Write(writer);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("Model Number:\n  " + m_value.ModelNumber + "\n");
            sb.Append("Variables:\n");
            foreach (KeyValuePair<string, double> variable in m_value.Variables)
            {
                sb.Append("  \"" + variable.Key + "\": " + variable.Value + "\n");
            }

            sb.Append("Objectives:\n");
            foreach (KeyValuePair<string, double> objective in m_value.Objectives)
            {
                sb.Append("  \"" + objective.Key + "\": " + objective.Value + "\n");
            }

            sb.Append("Attributes:\n");
            bool hasGeometry = m_value.ModelMesh != null;
            sb.Append("  Include Geometry: " + hasGeometry);

            return sb.ToString();
        }

        public class GH_CFishProxy : GH_GooProxy<GH_CFish>
        {
            public GH_CFishProxy(GH_CFish value)
                : base(value)
            {
            }
        }
    }

    [Serializable]
    public class CFish
    {
        public int ModelNumber;
        public Mesh ModelMesh;
        public Dictionary<string, double> Variables;
        public Dictionary<string, double> Objectives;

        public string SerializeToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        public static CFish DeserializeFromJson(string json)
        {
            return JsonConvert.DeserializeObject<CFish>(json);
        }
        public static string ToBase64(CFish cFish)
        {
            using (var ms = new MemoryStream())
            {
                new BinaryFormatter().Serialize(ms, cFish);
                return Convert.ToBase64String(ms.ToArray());
            }
        }

        public static CFish FromBase64(string base64String)
        {
            byte[] bytes = Convert.FromBase64String(base64String);
            using (var ms = new MemoryStream(bytes))
            {
                return (CFish)new BinaryFormatter().Deserialize(ms);
            }

        }
    }
}
