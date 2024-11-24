using System.Globalization;
using System.Windows;
using System.Windows.Controls;

using Optuna.Sampler;

using Tunny.Core.Input;
using Tunny.Core.Settings;
using Tunny.WPF.Common;

namespace Tunny.WPF.Views.Pages.Settings.Sampler
{
    public partial class GPOptunaSettingsPage : Page, ITrialNumberParam
    {
        public string Param1Label { get; set; } = "Number of Trials";
        public string Param2Label { get; set; } = "";
        public Visibility Param2Visibility { get; set; } = Visibility.Hidden;

        public GPOptunaSettingsPage()
        {
            InitializeComponent();
        }

        internal GPSampler ToSettings()
        {
            return new GPSampler
            {
                Seed = GpOptunaSeedTextBox.Text == "AUTO"
                    ? null
                    : (int?)int.Parse(GpOptunaSeedTextBox.Text, CultureInfo.InvariantCulture),
                NStartupTrials = GpOptunaStartupTrialsTextBox.Text == "AUTO"
                    ? -1
                    : int.Parse(GpOptunaStartupTrialsTextBox.Text, CultureInfo.InvariantCulture),
                DeterministicObjective = GpOptunaDeterministicObjectiveCheckBox.IsChecked ?? false,
            };
        }

        internal static GPOptunaSettingsPage FromSettings(TSettings settings)
        {
            GPSampler gpOptuna = settings.Optimize.Sampler.GP;
            var page = new GPOptunaSettingsPage();
            page.GpOptunaSeedTextBox.Text = gpOptuna.Seed == null
                ? "AUTO"
                : gpOptuna.Seed.Value.ToString(CultureInfo.InvariantCulture);
            page.GpOptunaStartupTrialsTextBox.Text = gpOptuna.NStartupTrials == -1
                ? "AUTO"
                : gpOptuna.NStartupTrials.ToString(CultureInfo.InvariantCulture);
            page.GpOptunaDeterministicObjectiveCheckBox.IsChecked = gpOptuna.DeterministicObjective;
            return page;
        }

        private void GpOptunaSeedTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = (TextBox)sender;
            string value = textBox.Text;
            textBox.Text = InputValidator.IsAutoOrInt(value) ? value : "AUTO";
        }

        private void GpOptunaStartupTrialsTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = (TextBox)sender;
            string value = textBox.Text;
            textBox.Text = InputValidator.IsAutoOrPositiveInt(value, false) ? value : "AUTO";
        }

        private void DefaultButton_Click(object sender, RoutedEventArgs e)
        {
            GpOptunaSeedTextBox.Text = "AUTO";
            GpOptunaStartupTrialsTextBox.Text = "AUTO";
            GpOptunaDeterministicObjectiveCheckBox.IsChecked = true;
        }
    }
}
