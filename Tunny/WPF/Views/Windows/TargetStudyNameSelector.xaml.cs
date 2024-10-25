using System.Windows;

using Tunny.WPF.ViewModels;

namespace Tunny.WPF.Views.Windows
{
    public partial class TargetStudyNameSelector : Window
    {
        public TargetStudyNameSelector(Core.Settings.TSettings settings)
        {
            InitializeComponent();
            DataContext = new TargetStudyNameSelectorViewModel(settings);
        }
    }
}
