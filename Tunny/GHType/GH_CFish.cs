using System.Collections.Generic;
using System.Text;

using Grasshopper.Kernel.Types;

using Rhino.Geometry;

namespace Tunny.GHType
{
    public class GH_CFish : GH_Goo<CFish>
    {
        public GH_CFish() => m_value = new CFish();

        public GH_CFish(CFish cFish) => m_value = cFish;
        public GH_CFish(GH_CFish other) => m_value = other.m_value;
        public override bool IsValid => true;

        public override string TypeName => "CFish";

        public override string TypeDescription => "Tunny optimizer result.";

        public override IGH_Goo Duplicate() => new GH_CFish(this);

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
    }

    public class CFish
    {
        internal int ModelNumber;
        internal Mesh ModelMesh;
        internal Dictionary<string, double> Variables;
        internal Dictionary<string, double> Objectives;
    }
}
