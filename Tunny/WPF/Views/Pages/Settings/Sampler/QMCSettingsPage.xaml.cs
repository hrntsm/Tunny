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
    public partial class QmcSettingsPage : Page, ITrialNumberParam
    {
        public string Param1Label { get; set; } = "Number of Trials";
        public string Param2Label { get; set; } = "";
        public Visibility Param2Visibility { get; set; } = Visibility.Hidden;

        public QmcSettingsPage()
        {
            InitializeComponent();
            QmcTypeComboBox.ItemsSource = Enum.GetNames(enumType: typeof(QmcType));
            QmcTypeComboBox.SelectedIndex = 0;
        }

        internal QMCSampler ToSettings()
        {
            return new QMCSampler
            {
                Seed = QmcSeedTextBox.Text == "AUTO"
                    ? null
                    : (int?)int.Parse(QmcSeedTextBox.Text, CultureInfo.InvariantCulture),
                QmcType = ((QmcType)QmcTypeComboBox.SelectedIndex).ToString(),
                Scramble = QmcScrambleCheckBox.IsChecked ?? false,
            };
        }

        internal static QmcSettingsPage FromSettings(TSettings settings)
        {
            QMCSampler qmc = settings.Optimize.Sampler.QMC;
            var page = new QmcSettingsPage();
            page.QmcSeedTextBox.Text = qmc.Seed == null
                ? "AUTO"
                : qmc.Seed.Value.ToString(CultureInfo.InvariantCulture);
            page.QmcTypeComboBox.SelectedIndex = (int)Enum.Parse(typeof(QmcType), qmc.QmcType);
            page.QmcScrambleCheckBox.IsChecked = qmc.Scramble;
            return page;
        }

        private void QmcSeedTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = (TextBox)sender;
            string value = textBox.Text;
            textBox.Text = InputValidator.IsAutoOrInt(value) ? value : "AUTO";
        }
    }
}
