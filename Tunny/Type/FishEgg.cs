using System;
using System.Collections.Generic;

using Tunny.Core.Input;
using Tunny.Core.Util;

namespace Tunny.Type
{
    [Serializable]
    public class FishEgg
    {
        public bool IsInteger { get; }
        public bool IsCategorical { get; }
        public bool IsNumber => !IsCategorical;
        public string NickName { get; }
        public List<double> Values { get; }
        public string Category { get; }

        public FishEgg(VariableBase variable)
        {
            TLog.MethodStart();
            NickName = variable.NickName;
            switch (variable)
            {
                case NumberVariable numberVariable:
                    IsCategorical = false;
                    IsInteger = numberVariable.IsInteger;
                    Values = new List<double> { numberVariable.Value };
                    break;
                case CategoricalVariable categoricalVariable:
                    IsCategorical = true;
                    Category = categoricalVariable.SelectedItem;
                    break;
            }
        }
    }
}
