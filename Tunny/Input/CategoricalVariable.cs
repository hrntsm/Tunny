using System;

namespace Tunny.Input
{
    [Serializable]
    public class CategoricalVariable : VariableBase
    {
        public string[] Categories { get; }

        public CategoricalVariable(string[] categories, string nickName, Guid id)
         : base(nickName, id)
        {
            Categories = categories;
        }
    }
}
