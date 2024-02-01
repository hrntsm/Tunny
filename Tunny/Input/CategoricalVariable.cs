using System;

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
            Categories = categories;
            SelectedItem = selectedItem;
        }
    }
}
