using System.Globalization;
using System.Windows;
using System.Windows.Controls;

using Optuna.Sampler;

using Tunny.Core.Settings;
using Tunny.WPF.Common;

namespace Tunny.WPF.Views.Pages.Settings.Sampler
{
    public partial class BruteForceSettingsPage : Page, ITrialNumberParam
    {
        public string Param1Label { get; set; } = "Number of Trials";
        public string Param2Label { get; set; } = "";
        public Visibility Param2Visibility { get; set; } = Visibility.Hidden;

        public BruteForceSettingsPage()
        {
            InitializeComponent();
        }

        internal BruteForceSampler ToSettings()
        {
            return new BruteForceSampler
            {
                Seed = BruteForceSeedTextBox.Text == "AUTO"
                    ? null
                    : (int?)int.Parse(BruteForceSeedTextBox.Text, CultureInfo.InvariantCulture),
            };
        }

        internal static BruteForceSettingsPage FromSettings(TSettings settings)
        {
            BruteForceSampler bruteForce = settings.Optimize.Sampler.BruteForce;
            var page = new BruteForceSettingsPage();
            page.BruteForceSeedTextBox.Text = bruteForce.Seed == null
                ? "AUTO"
                : bruteForce.Seed.Value.ToString(CultureInfo.InvariantCulture);
            return page;
        }
    }
}
