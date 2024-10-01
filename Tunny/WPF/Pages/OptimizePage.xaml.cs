using System.Windows.Controls;

using Tunny.WPF.Pages.Settings.Sampler;

namespace Tunny.WPF.Pages
{
    public partial class OptimizePage : Page
    {
        public OptimizePage()
        {
            InitializeComponent();
            switch (OptimizeSamplerComboBox.SelectedItem)
            {
                case "BayesianOptimization(TPE)":
                    optimizeDynamicFrame.Content = new TPESettingsPage();
                    break;
            }
        }
    }
}
