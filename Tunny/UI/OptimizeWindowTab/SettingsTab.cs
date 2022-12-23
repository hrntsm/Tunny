using System;
using System.Windows.Forms;

using Tunny.Settings;

namespace Tunny.UI
{
    public partial class OptimizationWindow : Form
    {
        private void TpeDefaultButton_Click(object sender, EventArgs e)
        {
            SetTpeSettings(new Tpe());
        }

        private void NsgaMutationProbCheckedChanged(object sender, EventArgs e)
        {
            nsgaMutationProbUpDown.Enabled = nsgaMutationProbCheckBox.Checked;
        }

        private void NsgaCrossoverCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            nsgaCrossoverComboBox.Enabled = nsgaCrossoverCheckBox.Checked;
        }

        private void NsgaDefaultButton_Click(object sender, EventArgs e)
        {
            SetNSGAIISettings(new NSGAII());
        }

        private void CmaEsSigmaCheckedChanged(object sender, EventArgs e)
        {
            cmaEsSigmaNumUpDown.Enabled = cmaEsSigmaCheckBox.Checked;
        }

        private void CmaEsRestartStrategyCheckedChanged(object sender, EventArgs e)
        {
            cmaEsIncPopSizeUpDown.Enabled = cmaEsRestartCheckBox.Checked;
            cmaEsPopulationSizeUpDown.Enabled = cmaEsRestartCheckBox.Checked;
        }

        private void CmaEsDefaultButton_Click(object sender, EventArgs e)
        {
            SetCmaEsSettings(new CmaEs());
        }

        private void CmaEsWarmStartCmaEsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            cmaEsWarmStartComboBox.Enabled = cmaEsWarmStartCmaEsCheckBox.Checked;
            cmaEsSigmaCheckBox.Enabled = !cmaEsWarmStartCmaEsCheckBox.Checked;
            cmaEsUseSaparableCmaCheckBox.Enabled = !cmaEsWarmStartCmaEsCheckBox.Checked;
        }

        private void BoTorchDefaultButton_Click(object sender, EventArgs e)
        {
            SetBoTorchSettings(new BoTorch());
        }

        private void QmcDefaultButton_Click(object sender, EventArgs e)
        {
            SetQMCSettings(new QuasiMonteCarlo());
        }

        private void InitializeSamplerSettings()
        {
            Sampler sampler = _settings.Optimize.Sampler;
            SetTpeSettings(sampler.Tpe);
            SetBoTorchSettings(sampler.BoTorch);
            SetNSGAIISettings(sampler.NsgaII);
            SetCmaEsSettings(sampler.CmaEs);
            SetQMCSettings(sampler.QMC);
        }

        private void SetTpeSettings(Tpe tpe)
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

        private void SetBoTorchSettings(BoTorch boTorch)
        {
            boTorchStartupNumUpDown.Value = boTorch.NStartupTrials;
        }

        private void SetNSGAIISettings(NSGAII nsga)
        {
            nsgaMutationProbCheckBox.Checked = nsga.MutationProb != null;
            nsgaMutationProbUpDown.Enabled = nsgaMutationProbCheckBox.Checked;
            nsgaMutationProbUpDown.Value = nsga.MutationProb != null ? (decimal)nsga.MutationProb : 0;
            nsgaCrossoverProbUpDown.Value = (decimal)nsga.CrossoverProb;
            nsgaSwappingProbUpDown.Value = (decimal)nsga.SwappingProb;
            nsgaPopulationSizeUpDown.Value = nsga.PopulationSize;
            nsgaCrossoverCheckBox.Checked = nsga.Crossover != string.Empty;
            nsgaCrossoverComboBox.Enabled = nsga.Crossover != string.Empty;
            nsgaCrossoverComboBox.SelectedIndex = SetNSGAIICrossoverSetting(nsga.Crossover);
        }

        private static int SetNSGAIICrossoverSetting(string crossover)
        {
            switch (crossover)
            {
                case "":
                case "Uniform":
                    return 0;
                case "BLXAlpha":
                    return 1;
                case "SPX":
                    return 2;
                case "SBX":
                    return 3;
                case "VSBX":
                    return 4;
                case "UNDX":
                    return 5;
                default:
                    throw new ArgumentException("Unexpected crossover method.");
            }
        }

        private void SetCmaEsSettings(CmaEs cmaEs)
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
            cmaEsPopulationSizeUpDown.Enabled = cmaEsRestartCheckBox.Checked;
            cmaEsPopulationSizeUpDown.Value = cmaEs.PopulationSize != null ? (decimal)cmaEs.PopulationSize : (decimal)1;
        }

        private void SetQMCSettings(QuasiMonteCarlo qmc)
        {
            qmcTypeComboBox.SelectedIndex = qmc.QmcType == "sobol" ? 0 : 1;

            qmcScrambleCheckBox.Checked = qmc.Scramble;
            qmcWarnIndependentSamplingCheckBox.Checked = qmc.WarnIndependentSampling;
            qmcWarnAsyncSeedingCheckBox.Checked = qmc.WarnAsynchronousSeeding;
        }

        private Sampler GetSamplerSettings()
        {
            return new Sampler
            {
                Tpe = GetTpeSettings(),
                BoTorch = GetBoTorchSettings(),
                NsgaII = GetNSGAIISettings(),
                CmaEs = GetCmaEsSettings(),
                QMC = GetQMCSettings()
            };
        }

        private Tpe GetTpeSettings()
        {
            return new Tpe
            {
                NStartupTrials = (int)tpeStartupNumUpDown.Value,
                NEICandidates = (int)tpeEINumUpDown.Value,
                PriorWeight = (double)tpePriorNumUpDown.Value,
                ConsiderPrior = tpeConsiderPriorCheckBox.Checked,
                Multivariate = tpeMultivariateCheckBox.Checked,
                ConsiderEndpoints = tpeConsiderEndpointsCheckBox.Checked,
                Group = tpeGroupCheckBox.Checked,
                ConsiderMagicClip = tpeConsiderMagicClipCheckBox.Checked,
                ConstantLiar = tpeConstantLiarCheckBox.Checked,
                WarnIndependentSampling = tpeWarnIndependentSamplingCheckBox.Checked
            };
        }

        private BoTorch GetBoTorchSettings()
        {
            return new BoTorch
            {
                NStartupTrials = (int)boTorchStartupNumUpDown.Value
            };
        }

        private NSGAII GetNSGAIISettings()
        {
            return new NSGAII
            {
                MutationProb = nsgaMutationProbCheckBox.Checked
                    ? (double?)nsgaMutationProbUpDown.Value : null,
                CrossoverProb = (double)nsgaCrossoverProbUpDown.Value,
                SwappingProb = (double)nsgaSwappingProbUpDown.Value,
                PopulationSize = (int)nsgaPopulationSizeUpDown.Value,
                Crossover = nsgaCrossoverCheckBox.Checked
                    ? nsgaCrossoverComboBox.Text : string.Empty,
            };
        }

        private CmaEs GetCmaEsSettings()
        {
            return new CmaEs
            {
                NStartupTrials = (int)cmaEsStartupNumUpDown.Value,
                Sigma0 = cmaEsSigmaCheckBox.Checked
                    ? (double?)cmaEsSigmaNumUpDown.Value : null,
                WarnIndependentSampling = cmaEsWarnIndependentSamplingCheckBox.Checked,
                ConsiderPrunedTrials = cmaEsConsiderPruneTrialsCheckBox.Checked,
                UseSeparableCma = cmaEsUseSaparableCmaCheckBox.Checked,
                RestartStrategy = cmaEsRestartCheckBox.Checked ? "ipop" : string.Empty,
                IncPopsize = (int)cmaEsIncPopSizeUpDown.Value,
                PopulationSize = cmaEsRestartCheckBox.Checked ? (int?)cmaEsPopulationSizeUpDown.Value : null,
                UseWarmStart = cmaEsWarmStartCmaEsCheckBox.Checked,
                WarmStartStudyName = cmaEsWarmStartComboBox.Text
            };
        }

        private QuasiMonteCarlo GetQMCSettings()
        {
            return new QuasiMonteCarlo
            {
                QmcType = qmcTypeComboBox.SelectedIndex == 0 ? "sobol" : "halton",
                Scramble = qmcScrambleCheckBox.Checked,
                WarnIndependentSampling = qmcWarnIndependentSamplingCheckBox.Checked,
                WarnAsynchronousSeeding = qmcWarnAsyncSeedingCheckBox.Checked
            };
        }
    }
}
