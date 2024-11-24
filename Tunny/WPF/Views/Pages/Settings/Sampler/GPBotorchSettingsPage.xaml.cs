using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

using Optuna.Sampler;

using Tunny.Core.Input;
using Tunny.Core.Settings;
using Tunny.WPF.Common;

namespace Tunny.WPF.Views.Pages.Settings.Sampler
{
    public partial class GPBoTorchSettingsPage : Page, ITrialNumberParam
    {
        public string Param1Label { get; set; } = "Number of Trials";
        public string Param2Label { get; set; } = "";
        public Visibility Param2Visibility { get; set; } = Visibility.Hidden;

        public GPBoTorchSettingsPage()
        {
            InitializeComponent();
        }

        internal BoTorchSampler ToSettings()
        {
            return new BoTorchSampler
            {
                Seed = GpBoTorchSeedTextBox.Text == "AUTO"
                    ? null
                    : (int?)int.Parse(GpBoTorchSeedTextBox.Text, CultureInfo.InvariantCulture),
                NStartupTrials = GpStartupTrialsTextBox.Text == "AUTO"
                    ? -1
                    : int.Parse(GpStartupTrialsTextBox.Text, CultureInfo.InvariantCulture),
            };
        }

        internal static GPBoTorchSettingsPage FromSettings(TSettings settings)
        {
            BoTorchSampler gpBoTorch = settings.Optimize.Sampler.BoTorch;
            var page = new GPBoTorchSettingsPage();
            page.GpBoTorchSeedTextBox.Text = gpBoTorch.Seed == null
                ? "AUTO"
                : gpBoTorch.Seed.Value.ToString(CultureInfo.InvariantCulture);
            page.GpStartupTrialsTextBox.Text = gpBoTorch.NStartupTrials == -1
                ? "AUTO"
                : gpBoTorch.NStartupTrials.ToString(CultureInfo.InvariantCulture);
            return page;
        }

        private void GpBoTorchSeedTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = (TextBox)sender;
            string value = textBox.Text;
            textBox.Text = InputValidator.IsAutoOrInt(value) ? value : "AUTO";
        }

        private void GpStartupTrialsTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = (TextBox)sender;
            string value = textBox.Text;
            textBox.Text = InputValidator.IsAutoOrPositiveInt(value, false) ? value : "AUTO";
        }

        private void DefaultButton_Click(object sender, RoutedEventArgs e)
        {
            GpBoTorchSeedTextBox.Text = "AUTO";
            GpStartupTrialsTextBox.Text = "AUTO";
        }
    }
}
