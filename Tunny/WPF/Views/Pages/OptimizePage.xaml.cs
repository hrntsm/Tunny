using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

using Tunny.Core.TEnum;
using Tunny.WPF.Common;
using Tunny.WPF.Views.Pages.Settings.Sampler;

namespace Tunny.WPF.Views.Pages
{
    public partial class OptimizePage : Page
    {
        private readonly LongRunningProcess _process;
        private readonly ProgressBar _progressBar;

        public OptimizePage()
        {
            InitializeComponent();
            ChangeFrameContent(SamplerType.TPE);
            _process = new LongRunningProcess();
        }

        public OptimizePage(SamplerType samplerType, ProgressBar progressBar)
        {
            InitializeComponent();
            ChangeFrameContent(samplerType);
            _process = new LongRunningProcess();
            _progressBar = progressBar;
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

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            OptimizeRunButton.IsEnabled = false;
            _progressBar.Value = 0;

            var progress = new Progress<int>(value =>
            {
                _progressBar.Value = value;
            });

            try
            {
                await _process.RunAsync(progress);
            }
            finally
            {
                OptimizeRunButton.IsEnabled = true;
            }
        }
    }

    public class LongRunningProcess
    {
        public async Task RunAsync(IProgress<int> progress)
        {
            for (int i = 0; i <= 100; i++)
            {
                await Task.Delay(100); // Simulate work
                progress.Report(i);
            }
        }
    }
}
