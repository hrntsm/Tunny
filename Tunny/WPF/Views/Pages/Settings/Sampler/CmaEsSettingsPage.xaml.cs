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
    public partial class CmaEsSettingsPage : Page, ITrialNumberParam
    {
        public string Param1Label { get; set; } = "Number of Trials";
        public string Param2Label { get; set; } = "";
        public Visibility Param2Visibility { get; set; } = Visibility.Hidden;

        public CmaEsSettingsPage()
        {
            InitializeComponent();
            CmaEsRestartStrategyComboBox.ItemsSource = Enum.GetValues(typeof(CmaEsRestartStrategyType));
            CmaEsRestartStrategyComboBox.SelectedIndex = 0;
        }

        internal CmaEsSampler ToSettings()
        {
            return new CmaEsSampler
            {
                Seed = CmaEsSeedTextBox.Text == "AUTO"
                    ? null
                    : (int?)int.Parse(CmaEsSeedTextBox.Text, CultureInfo.InvariantCulture),
                Sigma0 = CmaEsSigma0TextBox.Text == "AUTO"
                    ? null
                    : (double?)double.Parse(CmaEsSigma0TextBox.Text, CultureInfo.InvariantCulture),
                UseSeparableCma = CmaEsUseSepCMACheckBox.IsChecked ?? false,
                WithMargin = CmaEsWithMarginCheckBox.IsChecked ?? false,
                RestartStrategy = (CmaEsRestartStrategyType)CmaEsRestartStrategyComboBox.SelectedIndex == CmaEsRestartStrategyType.NoRestart
                    ? string.Empty
                    : ((CmaEsRestartStrategyType)CmaEsRestartStrategyComboBox.SelectedIndex).ToString(),
                PopulationSize = int.Parse(CmaEsPopulationSizeTextBox.Text, CultureInfo.InvariantCulture),
                IncPopsize = int.Parse(CmaEsIncreasingPopulationSizeTextBox.Text, CultureInfo.InvariantCulture),
                WarmStartStudyName = (bool)CmaEsWarmStartCmaEsCheckBox.IsChecked
                    ? CmaEsWarnStartCmaEsComboBox.SelectedItem.ToString()
                    : string.Empty,
                UseFirstEggToX0 = (bool)CmaEsUseFirstFishEggToX0CheckBox.IsChecked,
                LrAdapt = (bool)CmaEsLrAdaptCheckBox.IsChecked,
                ConsiderPrunedTrials = (bool)CmaEsConsiderPrunedTrialsCheckBox.IsChecked,
            };
        }

        internal static CmaEsSettingsPage FromSettings(TSettings settings)
        {
            CmaEsSampler cmaEs = settings.Optimize.Sampler.CmaEs;
            var page = new CmaEsSettingsPage();
            page.CmaEsSeedTextBox.Text = cmaEs.Seed == null
                ? "AUTO"
                : cmaEs.Seed.Value.ToString(CultureInfo.InvariantCulture);
            page.CmaEsSigma0TextBox.Text = cmaEs.Sigma0 == null
                ? "AUTO"
                : cmaEs.Sigma0.Value.ToString(CultureInfo.InvariantCulture);
            page.CmaEsUseSepCMACheckBox.IsChecked = cmaEs.UseSeparableCma;
            page.CmaEsWithMarginCheckBox.IsChecked = cmaEs.WithMargin;
            page.CmaEsRestartStrategyComboBox.SelectedIndex = cmaEs.RestartStrategy == string.Empty
                ? 0
                : (int)Enum.Parse(typeof(CmaEsRestartStrategyType), cmaEs.RestartStrategy);
            page.CmaEsPopulationSizeTextBox.Text = cmaEs.PopulationSize == null
                ? "2"
                : cmaEs.PopulationSize?.ToString(CultureInfo.InvariantCulture);
            page.CmaEsIncreasingPopulationSizeTextBox.Text = cmaEs.IncPopsize.ToString(CultureInfo.InvariantCulture);
            page.CmaEsWarmStartCmaEsCheckBox.IsChecked = cmaEs.WarmStartStudyName != string.Empty;
            page.CmaEsWarnStartCmaEsComboBox.SelectedItem = cmaEs.WarmStartStudyName;
            page.CmaEsUseFirstFishEggToX0CheckBox.IsChecked = cmaEs.UseFirstEggToX0;
            page.CmaEsConsiderPrunedTrialsCheckBox.IsChecked = cmaEs.ConsiderPrunedTrials;
            page.CmaEsLrAdaptCheckBox.IsChecked = cmaEs.LrAdapt;
            return page;
        }

        private void CmaEsSeedTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = (TextBox)sender;
            string value = textBox.Text;
            textBox.Text = InputValidator.IsAutoOrInt(value) ? value : "AUTO";
        }

        private void CmaEsSigma0TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = (TextBox)sender;
            string value = textBox.Text;
            textBox.Text = InputValidator.IsAutoOrPositiveDouble(value, false) ? value : "AUTO";
        }

        private void CmaEsPopulationSizeTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = (TextBox)sender;
            string value = textBox.Text;
            textBox.Text = InputValidator.IsPositiveInt(value, false) ? value : "1";
        }

        private void CmaEsIncreasingPopulationSizeTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = (TextBox)sender;
            string value = textBox.Text;
            textBox.Text = InputValidator.IsPositiveInt(value, false) ? value : "2";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var defaultSettings = new CmaEsSampler();
            CmaEsSeedTextBox.Text = "AUTO";
            CmaEsSigma0TextBox.Text = "AUTO";
            CmaEsUseSepCMACheckBox.IsChecked = defaultSettings.UseSeparableCma;
            CmaEsWithMarginCheckBox.IsChecked = defaultSettings.WithMargin;
            CmaEsLrAdaptCheckBox.IsChecked = defaultSettings.LrAdapt;
            CmaEsConsiderPrunedTrialsCheckBox.IsChecked = defaultSettings.ConsiderPrunedTrials;
            CmaEsRestartStrategyComboBox.SelectedIndex = 0;
            CmaEsPopulationSizeTextBox.Text = defaultSettings.PopulationSize == null
                ? "2"
                : defaultSettings.PopulationSize.Value.ToString(CultureInfo.InvariantCulture);
            CmaEsIncreasingPopulationSizeTextBox.Text = defaultSettings.IncPopsize.ToString(CultureInfo.InvariantCulture);
            CmaEsWarmStartCmaEsCheckBox.IsChecked = defaultSettings.UseWarmStart;
            CmaEsUseFirstFishEggToX0CheckBox.IsChecked = defaultSettings.UseFirstEggToX0;
        }
    }
}
