using System;
using System.Windows.Forms;

using Tunny.Settings;

namespace Tunny.UI
{
    public partial class OptimizationWindow : Form
    {
        private void NsgaMutationProbCheckedChanged(object sender, EventArgs e)
        {
            nsgaMutationProbUpDown.Enabled = nsgaMutationProbCheckBox.Checked;
        }

        private void CmaEsSigmaCheckedChanged(object sender, EventArgs e)
        {
            cmaEsSigmaNumUpDown.Enabled = cmaEsSigmaCheckBox.Checked;
        }

        private void CmaEsRestartStrategyCheckedChanged(object sender, EventArgs e)
        {
            cmaEsIncPopSizeUpDown.Enabled = cmaEsRestartCheckBox.Checked;
        }

        private void InitializeSamplerSettings()
        {
            Sampler sampler = _settings.Optimize.Sampler;
            TpeSettingInitialize(sampler.Tpe);
            BoTorchSettingInitialize(sampler.BoTorch);
            NSGAIISettingsInitialize(sampler.NsgaII);
            CmaEsSettingInitialize(sampler.CmaEs);
            QMCSettingInitialize(sampler.QMC);
        }

        private void TpeSettingInitialize(Tpe tpe)
        {
            tpeStartupNumUpDown.Value = tpe.NStartupTrials;
            tpeEINumUpDown.Value = tpe.NEICandidates;
            tpePriorNumUpDown.Value = (decimal)tpe.PriorWeight;

            tpeConsiderPriorCheckBox.Checked = tpe.ConsiderPrior;
            tpeMultivariateCheckBox.Checked = tpe.Multivariate;
            tpeConsiderEndpointsCheckBox.Checked = tpe.ConsiderEndpoints;
            tpeGroupCheckBox.Checked = tpe.Group;
            tpeConsiderMagicClipCheckBox.Checked = tpe.ConsiderMagicClip;
            tpeConstantLiarCheckBox.Checked = tpe.ConstantLiar;
            tpeWarnIndependentSamplingCheckBox.Checked = tpe.WarnIndependentSampling;
        }

        private void BoTorchSettingInitialize(BoTorch boTorch)
        {
            boTorchStartupNumUpDown.Value = boTorch.NStartupTrials;
        }

        private void NSGAIISettingsInitialize(NSGAII nsga)
        {
            nsgaMutationProbCheckBox.Checked = nsga.MutationProb != null;
            nsgaMutationProbUpDown.Enabled = nsgaMutationProbCheckBox.Checked;
            nsgaMutationProbUpDown.Value = nsga.MutationProb != null ? (decimal)nsga.MutationProb : 0;
            nsgaCrossoverProbUpDown.Value = (decimal)nsga.CrossoverProb;
            nsgaSwappingProbUpDown.Value = (decimal)nsga.SwappingProb;
            nsgaPopulationSizeUpDown.Value = nsga.PopulationSize;
        }

        private void CmaEsSettingInitialize(CmaEs cmaEs)
        {
            cmaEsStartupNumUpDown.Value = cmaEs.NStartupTrials;
            cmaEsSigmaCheckBox.Checked = cmaEs.Sigma0 != null;
            cmaEsSigmaNumUpDown.Value = cmaEs.Sigma0 != null ? (decimal)cmaEs.Sigma0 : 0;
            cmaEsSigmaNumUpDown.Enabled = cmaEsSigmaCheckBox.Checked;

            cmaEsWarnIndependentSamplingCheckBox.Checked = cmaEs.WarnIndependentSampling;
            cmaEsConsiderPruneTrialsCheckBox.Checked = cmaEs.ConsiderPrunedTrials;
            cmaEsUseSaparableCmaCheckBox.Checked = cmaEs.UseSeparableCma;

            cmaEsRestartCheckBox.Checked = cmaEs.RestartStrategy != string.Empty;
            cmaEsIncPopSizeUpDown.Enabled = cmaEsRestartCheckBox.Checked;
            cmaEsIncPopSizeUpDown.Value = cmaEs.IncPopsize;
        }

        private void QMCSettingInitialize(QuasiMonteCarlo qmc)
        {
            qmcTypeComboBox.SelectedIndex = qmc.QmcType == "sobol" ? 0 : 1;

            qmcScrambleCheckBox.Checked = qmc.Scramble;
            qmcWarnIndependentSamplingCheckBox.Checked = qmc.WarnIndependentSampling;
            qmcWarnAsyncSeedingCheckBox.Checked = qmc.WarnAsynchronousSeeding;
        }
    }
}
