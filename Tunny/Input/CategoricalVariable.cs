using System;

using Tunny.Util;

namespace Tunny.Input
{
    [Serializable]
    public class CategoricalVariable : VariableBase
    {
        public string[] Categories { get; }
        public string SelectedItem { get; }

        public CategoricalVariable(string[] categories, string selectedItem, string nickName, Guid id)
         : base(nickName, id)
        {
            TLog.MethodStart();
            Categories = categories;
            SelectedItem = selectedItem;
        }
    }
}
