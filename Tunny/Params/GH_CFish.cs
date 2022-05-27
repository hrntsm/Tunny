using Grasshopper.Kernel.Types;

namespace Tunny.Params
{
    public class GH_CFish : GH_Goo<int>
    {
        public GH_CFish() => m_value = 0;
        public GH_CFish(int number) => m_value = number;
        public GH_CFish(GH_CFish other) => m_value = other.m_value;
        public override bool IsValid => true;

        public override string TypeName => "Integer";

        public override string TypeDescription => "A 32bit integer.";

        public override IGH_Goo Duplicate() => new GH_CFish(this);

        public override string ToString() => m_value.ToString();
    }
}
