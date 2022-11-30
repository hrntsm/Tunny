using System.Collections.Generic;

using Tunny.Util;

namespace Tunny.Type
{
    public class FishEgg
    {
        public bool IsInteger { get; set; }
        public string NickName { get; set; }
        public List<double> Values { get; set; }

        public FishEgg(Variable variable)
        {
            IsInteger = variable.IsInteger;
            NickName = variable.NickName;
            Values = new List<double> { variable.Value };
        }
    }
}
