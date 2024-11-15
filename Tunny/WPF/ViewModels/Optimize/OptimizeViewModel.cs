using System.Collections.ObjectModel;

using Prism.Mvvm;

using Tunny.Process;
using Tunny.WPF.Models;

namespace Tunny.WPF.ViewModels.Optimize
{
    internal class OptimizeViewModel : BindableBase
    {
        private ObservableCollection<ObjectiveSettingItem> _objectiveSettingItems;
        public ObservableCollection<ObjectiveSettingItem> ObjectiveSettingItems { get => _objectiveSettingItems; set => SetProperty(ref _objectiveSettingItems, value); }
        private ObservableCollection<VariableSettingItem> _variableSettingItems;
        public ObservableCollection<VariableSettingItem> VariableSettingItems { get => _variableSettingItems; set => SetProperty(ref _variableSettingItems, value); }

        public OptimizeViewModel()
        {
            Util.GrasshopperInOut ghIO = OptimizeProcess.Component.GhInOut;
            ObjectiveSettingItems = new ObservableCollection<ObjectiveSettingItem>();
            foreach (string item in ghIO.Objectives.GetNickNames())
            {
                ObjectiveSettingItems.Add(new ObjectiveSettingItem
                {
                    Name = item,
                    Minimize = true
                });
            }
            VariableSettingItems = new ObservableCollection<VariableSettingItem>();
            foreach (Core.Input.VariableBase item in ghIO.Variables)
            {
                if (item is Core.Input.NumberVariable numVariable)
                {
                    VariableSettingItems.Add(new VariableSettingItem
                    {
                        Name = numVariable.NickName,
                        Low = numVariable.LowerBond,
                        High = numVariable.UpperBond,
                        Step = numVariable.Epsilon,
                        IsLogScale = false
                    });
                }
            }
        }
    }
}
