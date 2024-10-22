using System;
using System.Windows;
using System.Windows.Controls;

using Optuna.Sampler;

using Tunny.Core.Settings;
using Tunny.WPF.Common;

namespace Tunny.WPF.Views.Pages.Settings.Sampler
{
    public partial class NSGAIIISettingsPage : Page, ITrialNumberParam
    {
        public string Param1Label { get; set; } = "Number of Generation";
        public string Param2Label { get; set; } = "Population Size";
        public Visibility Param2Visibility { get; set; } = Visibility.Visible;

        public NSGAIIISettingsPage()
        {
            InitializeComponent();
            NsgaiiiCrossoverComboBox.ItemsSource = Enum.GetNames(typeof(NsgaCrossoverType));
            NsgaiiiCrossoverComboBox.SelectedIndex = 0;
        }

        internal NSGAIIISampler ToSettings()
        {
            return new NSGAIIISampler
            {
                Seed = NsgaiiiSeedTextBox.Text == "AUTO"
                    ? null
                    : (int?)int.Parse(NsgaiiiSeedTextBox.Text, System.Globalization.CultureInfo.InvariantCulture),
                MutationProb = NsgaiiiMutationProbabilityTextBox.Text == "AUTO"
                    ? null
                    : (double?)double.Parse(NsgaiiiMutationProbabilityTextBox.Text, System.Globalization.CultureInfo.InvariantCulture),
                CrossoverProb = double.Parse(NsgaiiiCrossoverProbabilityTextBox.Text, System.Globalization.CultureInfo.InvariantCulture),
                SwappingProb = double.Parse(NsgaiiiSwappingProbabilityTextBox.Text, System.Globalization.CultureInfo.InvariantCulture),
                Crossover = ((NsgaCrossoverType)NsgaiiiCrossoverComboBox.SelectedIndex).ToString()
            };
        }

        internal static NSGAIIISettingsPage FromSettings(TSettings settings)
        {
            NSGAIIISampler nsgaiii = settings.Optimize.Sampler.NsgaIII;
            var page = new NSGAIIISettingsPage();
            page.NsgaiiiSeedTextBox.Text = nsgaiii.Seed == null
                ? "AUTO"
                : nsgaiii.Seed.Value.ToString(System.Globalization.CultureInfo.InvariantCulture);
            page.NsgaiiiMutationProbabilityTextBox.Text = nsgaiii.MutationProb == null
                ? "AUTO"
                : nsgaiii.MutationProb.Value.ToString(System.Globalization.CultureInfo.InvariantCulture);
            page.NsgaiiiCrossoverProbabilityTextBox.Text = nsgaiii.CrossoverProb.ToString(System.Globalization.CultureInfo.InvariantCulture);
            page.NsgaiiiSwappingProbabilityTextBox.Text = nsgaiii.SwappingProb.ToString(System.Globalization.CultureInfo.InvariantCulture);
            page.NsgaiiiCrossoverComboBox.SelectedIndex = string.IsNullOrEmpty(nsgaiii.Crossover)
                ? 0 : (int)Enum.Parse(typeof(NsgaCrossoverType), nsgaiii.Crossover);
            return page;
        }
    }
}
