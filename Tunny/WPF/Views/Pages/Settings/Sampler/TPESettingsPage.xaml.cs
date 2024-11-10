using System.Globalization;
using System.Windows;
using System.Windows.Controls;

using Optuna.Sampler;

using Tunny.Core.Settings;
using Tunny.WPF.Common;

namespace Tunny.WPF.Views.Pages.Settings.Sampler
{
    public partial class TPESettingsPage : Page, ITrialNumberParam
    {
        public string Param1Label { get; set; } = "Number of Trials";
        public string Param2Label { get; set; } = "";
        public Visibility Param2Visibility { get; set; } = Visibility.Hidden;

        public TPESettingsPage()
        {
            InitializeComponent();
        }

        internal TpeSampler ToSettings()
        {
            return new TpeSampler
            {
                Seed = TpeSeedTextBox.Text == "AUTO"
                    ? null
                    : (int?)int.Parse(TpeSeedTextBox.Text, CultureInfo.InvariantCulture),
                NStartupTrials = TpeStartupTrialsTextBox.Text == "AUTO"
                    ? -1
                    : int.Parse(TpeStartupTrialsTextBox.Text, CultureInfo.InvariantCulture),
                NEICandidates = int.Parse(TpeEICandidateTextBox.Text, CultureInfo.InvariantCulture),
                Gamma = int.Parse(TpeGammaTextBox.Text, CultureInfo.InvariantCulture),
                PriorWeight = double.Parse(TpePriorWeightTextBox.Text, CultureInfo.InvariantCulture),
                ConsiderPrior = TpeConsiderPriorCheckBox.IsChecked ?? false,
                ConsiderEndpoints = TpeConsiderEndpointsCheckBox.IsChecked ?? false,
                ConsiderMagicClip = TpeConsiderMagicClipCheckBox.IsChecked ?? false,
                Multivariate = TpeMultivariateCheckBox.IsChecked ?? false,
                Group = TpeGroupCheckBox.IsChecked ?? false,
                ConstantLiar = TpeConstantLiarCheckBox.IsChecked ?? false,
            };
        }

        internal static TPESettingsPage FromSettings(TSettings settings)
        {
            TpeSampler tpe = settings.Optimize.Sampler.Tpe;
            var page = new TPESettingsPage();
            page.TpeSeedTextBox.Text = tpe.Seed == null
                ? "AUTO"
                : tpe.Seed.Value.ToString(CultureInfo.InvariantCulture);
            page.TpeStartupTrialsTextBox.Text = tpe.NStartupTrials == -1
                ? "AUTO"
                : tpe.NStartupTrials.ToString(CultureInfo.InvariantCulture);
            page.TpeEICandidateTextBox.Text = tpe.NEICandidates.ToString(CultureInfo.InvariantCulture);
            page.TpeGammaTextBox.Text = tpe.Gamma.ToString(CultureInfo.InvariantCulture);
            page.TpePriorWeightTextBox.Text = tpe.PriorWeight.ToString(CultureInfo.InvariantCulture);
            page.TpeConsiderPriorCheckBox.IsChecked = tpe.ConsiderPrior;
            page.TpeConsiderEndpointsCheckBox.IsChecked = tpe.ConsiderEndpoints;
            page.TpeConsiderMagicClipCheckBox.IsChecked = tpe.ConsiderMagicClip;
            page.TpeMultivariateCheckBox.IsChecked = tpe.Multivariate;
            page.TpeGroupCheckBox.IsChecked = tpe.Group;
            page.TpeConstantLiarCheckBox.IsChecked = tpe.ConstantLiar;
            return page;
        }
    }
}
