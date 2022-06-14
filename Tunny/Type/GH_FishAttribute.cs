using System.Collections;
using System.Collections.Generic;
using System.Text;

using Grasshopper.Kernel.Types;

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

        public override string TypeName => "GH_FishAttributes";

        public override string TypeDescription => "DictionaryGoo for grasshopper";

        public override IGH_Goo Duplicate()
        {
            return new GH_FishAttribute() { Value = Value };
        }

        public override string ToString()
        {
            var text = new StringBuilder();
            foreach (string key in Value.Keys)
            {
                text.AppendLine($"{key}:");
                if (Value[key] is IEnumerable iEnum)
                {
                    foreach (object item in iEnum)
                        text.AppendLine($"  {item}");
                }
                else
                {
                    text.AppendLine($"  {Value[key]}");
                }
            }
            return text.ToString();
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
    }
}
