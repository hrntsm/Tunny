using System;
using System.Windows.Controls;

using Tunny.WPF.Views.Pages.Settings.Sampler;

namespace Tunny.WPF.Views.Pages
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
            string i = "1";
            switch (i)
            {
                case "BayesianOptimization(TPE)":
                    OptimizeDynamicFrame.Content = new TPESettingsPage();
                    break;
                case "BayesianOptimization(GP:Optuna)":
                    OptimizeDynamicFrame.Content = new GPOptunaSettingsPage();
                    break;
                case "BayesianOptimization(GP:Botorch)":
                    OptimizeDynamicFrame.Content = new GPBoTorchSettingsPage();
                    break;
                case "GeneticAlgorithm(NSGA-II)":
                    OptimizeDynamicFrame.Content = new NSGAIISettingsPage();
                    break;
                case "GeneticAlgorithm(NSGA-III)":
                    OptimizeDynamicFrame.Content = new NSGAIIISettingsPage();
                    break;
                case "EvolutionStrategy(CMA-ES)":
                    OptimizeDynamicFrame.Content = new CmaEsSettingsPage();
                    break;
                case "Quasi-MonteCarlo":
                    OptimizeDynamicFrame.Content = new QmcSettingsPage();
                    break;
                case "Random":
                    OptimizeDynamicFrame.Content = new RandomSettingsPage();
                    break;
                case "BruteForce":
                    OptimizeDynamicFrame.Content = new BruteForceSettingsPage();
                    break;
                default:
                    OptimizeDynamicFrame.Content = new TPESettingsPage();
                    break;
            }
        }
    }
}
