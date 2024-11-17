using System.Globalization;
using System.Windows;
using System.Windows.Controls;

using Optuna.Sampler.OptunaHub;

using Tunny.Core.Settings;
using Tunny.WPF.Common;

namespace Tunny.WPF.Views.Pages.Settings.Sampler
{
    public partial class MoCmaEsSettingsPage : Page, ITrialNumberParam
    {
        public string Param1Label { get; set; } = "Number of Trials";
        public string Param2Label { get; set; } = "";
        public Visibility Param2Visibility { get; set; } = Visibility.Hidden;

        public MoCmaEsSettingsPage()
        {
            InitializeComponent();
        }

        internal MoCmaEsSampler ToSettings()
        {
            return new MoCmaEsSampler
            {
                Seed = MoCmaEsSeedTextBox.Text == "AUTO"
                    ? null
                    : (int?)int.Parse(MoCmaEsSeedTextBox.Text, CultureInfo.InvariantCulture),
                PopulationSize = MoCmaEsPopulationSizeTextBox.Text == "AUTO"
                    ? null
                    : (int?)int.Parse(MoCmaEsPopulationSizeTextBox.Text, CultureInfo.InvariantCulture),
            };
        }

        internal static MoCmaEsSettingsPage FromSettings(TSettings settings)
        {
            MoCmaEsSampler cmaEs = settings.Optimize.Sampler.MoCmaEs;
            var page = new MoCmaEsSettingsPage();
            page.MoCmaEsSeedTextBox.Text = cmaEs.Seed == null
                ? "AUTO"
                : cmaEs.Seed.Value.ToString(CultureInfo.InvariantCulture);
            page.MoCmaEsPopulationSizeTextBox.Text = cmaEs.PopulationSize == null
                ? "AUTO"
                : cmaEs.PopulationSize?.ToString(CultureInfo.InvariantCulture);
            return page;
        }
    }
}
