using System.Windows;
using System.Windows.Controls;

using Tunny.Core.Settings;
using Tunny.WPF.Common;
using Tunny.WPF.ViewModels;

namespace Tunny.WPF.Views.Pages
{
    public partial class VisualizePage : Page
    {
        private readonly VisualizeViewModel _viewModel;
        private readonly TSettings _settings;

        public VisualizePage()
        {
        }

        public VisualizePage(TSettings settings)
        {
            InitializeComponent();
            _settings = settings;
            _viewModel = new VisualizeViewModel(settings);
            DataContext = _viewModel;
            Loaded += SetStudyNameComboBoxItems;
        }

        private void SetStudyNameComboBoxItems(object sender, RoutedEventArgs e)
        {
            _viewModel.SetStudyNameItems(_settings);
        }

        internal void SetTargetVisualizeType(VisualizeType visualizeType)
        {
            _viewModel.SetTargetVisualizeType(visualizeType);
        }
    }
}
