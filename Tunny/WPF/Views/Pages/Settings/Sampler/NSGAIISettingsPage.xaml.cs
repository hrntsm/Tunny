using System;
using System.Windows;
using System.Windows.Controls;

using Optuna.Sampler;

using Tunny.Core.Settings;
using Tunny.WPF.Common;

namespace Tunny.WPF.Views.Pages.Settings.Sampler
{
    public partial class NSGAIISettingsPage : Page, ITrialNumberParam
    {
        public string Param1Label { get; set; } = "Number of Generation";
        public string Param2Label { get; set; } = "Population Size";
        public Visibility Param2Visibility { get; set; } = Visibility.Visible;

        public NSGAIISettingsPage()
        {
            InitializeComponent();
            NsgaiiCrossoverComboBox.ItemsSource = Enum.GetNames(typeof(NsgaCrossoverType));
            NsgaiiCrossoverComboBox.SelectedIndex = 0;
        }

        internal NSGAIISampler ToSettings()
        {
            return new NSGAIISampler
            {
                Seed = NsgaiiSeedTextBox.Text == "AUTO"
                    ? null
                    : (int?)int.Parse(NsgaiiSeedTextBox.Text, System.Globalization.CultureInfo.InvariantCulture),
                MutationProb = NsgaiiMutationProbabilityTextBox.Text == "AUTO"
                    ? null
                    : (double?)double.Parse(NsgaiiMutationProbabilityTextBox.Text, System.Globalization.CultureInfo.InvariantCulture),
                CrossoverProb = double.Parse(NsgaiiCrossoverProbabilityTextBox.Text, System.Globalization.CultureInfo.InvariantCulture),
                SwappingProb = double.Parse(NsgaiiSwappingProbabilityTextBox.Text, System.Globalization.CultureInfo.InvariantCulture),
                Crossover = ((NsgaCrossoverType)NsgaiiCrossoverComboBox.SelectedIndex).ToString()
            };
        }

        internal static NSGAIISettingsPage FromSettings(TSettings settings)
        {
            NSGAIISampler nsgaii = settings.Optimize.Sampler.NsgaII;
            var page = new NSGAIISettingsPage();
            page.NsgaiiSeedTextBox.Text = nsgaii.Seed == null
                ? "AUTO"
                : nsgaii.Seed.Value.ToString(System.Globalization.CultureInfo.InvariantCulture);
            page.NsgaiiMutationProbabilityTextBox.Text = nsgaii.MutationProb == null
                ? "AUTO"
                : nsgaii.MutationProb.Value.ToString(System.Globalization.CultureInfo.InvariantCulture);
            page.NsgaiiCrossoverProbabilityTextBox.Text = nsgaii.CrossoverProb.ToString(System.Globalization.CultureInfo.InvariantCulture);
            page.NsgaiiSwappingProbabilityTextBox.Text = nsgaii.SwappingProb.ToString(System.Globalization.CultureInfo.InvariantCulture);
            page.NsgaiiCrossoverComboBox.SelectedIndex = (int)Enum.Parse(typeof(NsgaCrossoverType), nsgaii.Crossover);
            return page;
        }
    }
}
