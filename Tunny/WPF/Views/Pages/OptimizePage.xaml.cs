using System;
using System.Windows.Controls;

using Tunny.WPF.Pages.Settings.Sampler;

namespace Tunny.WPF.Pages
{
    public partial class OptimizePage : Page
    {
        public OptimizePage()
        {
            InitializeComponent();
            ChangeFrameContent();
        }

        private void ChangeFrameContent()
        {
            switch (OptimizeSamplerComboBox.SelectedItem)
            {
                case "BayesianOptimization(TPE)":
                    optimizeDynamicFrame.Content = new TPESettingsPage();
                    break;
                case "BayesianOptimization(GP:Optuna)":
                    optimizeDynamicFrame.Content = new GPOptunaSettingsPage();
                    break;
                case "BayesianOptimization(GP:Botorch)":
                    optimizeDynamicFrame.Content = new GPBoTorchSettingsPage();
                    break;
                case "GeneticAlgorithm(NSGA-II)":
                    optimizeDynamicFrame.Content = new NSGAIISettingsPage();
                    break;
                case "GeneticAlgorithm(NSGA-III)":
                    optimizeDynamicFrame.Content = new NSGAIIISettingsPage();
                    break;
                case "EvolutionStrategy(CMA-ES)":
                    optimizeDynamicFrame.Content = new CmaEsSettingsPage();
                    break;
                case "Quasi-MonteCarlo":
                    optimizeDynamicFrame.Content = new QMCSettingsPage();
                    break;
                case "Random":
                    optimizeDynamicFrame.Content = new RandomSettingsPage();
                    break;
                case "BruteForce":
                    optimizeDynamicFrame.Content = new BruteForceSettingsPage();
                    break;
                default:
                    throw new ArgumentException("Invalid sampler selected.");
            }
        }
    }
}
