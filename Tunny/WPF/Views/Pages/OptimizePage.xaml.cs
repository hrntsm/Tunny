using System;
using System.Windows.Controls;

using Tunny.Core.TEnum;
using Tunny.WPF.Common;
using Tunny.WPF.Views.Pages.Settings.Sampler;

namespace Tunny.WPF.Views.Pages
{
    public partial class OptimizePage : Page
    {
        public OptimizePage()
        {
            InitializeComponent();
            ChangeFrameContent(SamplerType.TPE);
        }

        public OptimizePage(SamplerType samplerType)
        {
            InitializeComponent();
            ChangeFrameContent(samplerType);
        }

        private void ChangeFrameContent(SamplerType samplerType)
        {
            ITrialNumberParam param;
            switch (samplerType)
            {
                case SamplerType.TPE:
                    var tpePage = new TPESettingsPage();
                    param = tpePage;
                    OptimizeDynamicFrame.Content = tpePage;
                    break;
                case SamplerType.GP:
                    var gpOptunaPage = new GPOptunaSettingsPage();
                    param = gpOptunaPage;
                    OptimizeDynamicFrame.Content = gpOptunaPage;
                    break;
                case SamplerType.BoTorch:
                    var gpBoTorchPage = new GPBoTorchSettingsPage();
                    param = gpBoTorchPage;
                    OptimizeDynamicFrame.Content = gpBoTorchPage;
                    break;
                case SamplerType.NSGAII:
                    var nsgaiiPage = new NSGAIISettingsPage();
                    param = nsgaiiPage;
                    OptimizeDynamicFrame.Content = nsgaiiPage;
                    break;
                case SamplerType.NSGAIII:
                    OptimizeDynamicFrame.Content = new NSGAIIISettingsPage();
                    var nsgaiiiPage = new NSGAIIISettingsPage();
                    param = nsgaiiiPage;
                    OptimizeDynamicFrame.Content = nsgaiiiPage;
                    break;
                case SamplerType.CmaEs:
                    var cmaesPage = new CmaEsSettingsPage();
                    param = cmaesPage;
                    OptimizeDynamicFrame.Content = cmaesPage;
                    break;
                case SamplerType.Random:
                    var randomPage = new RandomSettingsPage();
                    param = randomPage;
                    OptimizeDynamicFrame.Content = randomPage;
                    break;
                case SamplerType.QMC:
                    var qmcPage = new QmcSettingsPage();
                    param = qmcPage;
                    OptimizeDynamicFrame.Content = qmcPage;
                    break;
                case SamplerType.BruteForce:
                    var bruteForcePage = new BruteForceSettingsPage();
                    param = bruteForcePage;
                    OptimizeDynamicFrame.Content = bruteForcePage;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(samplerType), samplerType, null);
            }
            OptimizeTrialNumberParam1Label.Content = param.Param1Label;
            OptimizeTrialNumberParam2Label.Content = param.Param2Label;
            OptimizeTrialNumberParam2Label.Visibility = param.Param2Visibility;
            OptimizeTrialNumberParam2TextBox.Visibility = param.Param2Visibility;
        }
    }
}
