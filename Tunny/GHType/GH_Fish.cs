using System.Collections.Generic;
using System.Text;

using GH_IO.Serialization;

using Grasshopper.Kernel.Types;

namespace Tunny.GHType
{
    public class GH_Fish : GH_Goo<Fish>
    {
        public GH_Fish()
        {
        }

        public GH_Fish(Fish cFish)
            : base(cFish)
        {
        }

        public GH_Fish(GH_Fish other)
            : base(other.Value)
        {
        }

        public override bool IsValid => true;
        public override string TypeName => "Fish";
        public override string TypeDescription => "Tunny optimizer result.";
        public override bool CastFrom(object source) => false;
        public override IGH_GooProxy EmitProxy() => new GH_FishProxy(this);
        public override IGH_Goo Duplicate() => new GH_Fish(this);
        public override bool Read(GH_IReader reader)
        {
            string base64 = string.Empty;
            if (reader.TryGetString("Fish_bin", ref base64))
            {
                Value = Fish.FromBase64(base64);
            }
            return base.Read(reader);
        }

        public override bool Write(GH_IWriter writer)
        {
            writer.SetString("Fish_bin", Fish.ToBase64(Value));
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

        public class GH_FishProxy : GH_GooProxy<GH_Fish>
        {
            public GH_FishProxy(GH_Fish value)
                : base(value)
            {
            }
        }
    }
}
