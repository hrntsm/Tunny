using System.Windows;
using System.Windows.Controls;

using Tunny.Core.Settings;
using Tunny.Process;
using Tunny.WPF.Common;
using Tunny.WPF.ViewModels;

namespace Tunny.WPF.Views.Pages.Visualize
{
    public partial class VisualizePage : Page
    {
        private readonly VisualizeViewModel _viewModel;
        private readonly TSettings _settings;

        public VisualizePage()
        {
            InitializeComponent();
            _settings = OptimizeProcess.Settings;
            _viewModel = new VisualizeViewModel();
            DataContext = _viewModel;
            Loaded += SetStudyNameComboBoxItems;
        }

        private void SetStudyNameComboBoxItems(object sender, RoutedEventArgs e)
        {
            _viewModel.SetStudyNameItems();
        }

        internal void SetTargetVisualizeType(VisualizeType visualizeType)
        {
            _viewModel.SetTargetVisualizeType(visualizeType);
        }
    }
}
