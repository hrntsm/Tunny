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
    public partial class RandomSettingsPage : Page, ITrialNumberParam
    {
        public string Param1Label { get; set; } = "Number of Trials";
        public string Param2Label { get; set; } = "";
        public Visibility Param2Visibility { get; set; } = Visibility.Hidden;

        public RandomSettingsPage()
        {
            InitializeComponent();
        }

        internal RandomSampler ToSettings()
        {
            return new RandomSampler
            {
                Seed = RandomSeedTextBox.Text == "AUTO"
                    ? null
                    : (int?)int.Parse(RandomSeedTextBox.Text, CultureInfo.InvariantCulture),
            };
        }

        internal static RandomSettingsPage FromSettings(TSettings settings)
        {
            RandomSampler random = settings.Optimize.Sampler.Random;
            var page = new RandomSettingsPage();
            page.RandomSeedTextBox.Text = random.Seed == null
                ? "AUTO"
                : random.Seed.Value.ToString(CultureInfo.InvariantCulture);
            return page;
        }

        private void RandomSeedTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = (TextBox)sender;
            string value = textBox.Text;
            textBox.Text = InputValidator.IsAutoOrInt(value) ? value : "AUTO";
        }

        private void DefaultButton_Click(object sender, RoutedEventArgs e)
        {
            RandomSeedTextBox.Text = "AUTO";
        }
    }
}
