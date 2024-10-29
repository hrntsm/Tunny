using Prism.Mvvm;

using Tunny.Process;

namespace Tunny.WPF.ViewModels
{
    internal class MainWindowViewModel : BindableBase
    {

        public bool IsSingleObjective { get => !_isMultiObjective; }

        private bool _isMultiObjective;
        public bool IsMultiObjective { get => _isMultiObjective; set => SetProperty(ref _isMultiObjective, value); }

        public MainWindowViewModel()
        {
            IsMultiObjective = OptimizeProcess.Component.GhInOut.IsMultiObjective;
        }
    }
}
