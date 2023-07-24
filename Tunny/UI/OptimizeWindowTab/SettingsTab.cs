using System;

using Tunny.Settings.Sampler;
using Tunny.Solver;

namespace Tunny.UI
{
    public partial class OptimizationWindow
    {
        private void TpeDefaultButton_Click(object sender, EventArgs e)
        {
            SetTpeSettings(new Tpe());
        }

        private void Nsga2MutationProbCheckedChanged(object sender, EventArgs e)
        {
            nsga2MutationProbUpDown.Enabled = nsga2MutationProbCheckBox.Checked;
        }

        private void Nsga2CrossoverCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            nsga2CrossoverComboBox.Enabled = nsga2CrossoverCheckBox.Checked;
        }

        private void Nsga2DefaultButton_Click(object sender, EventArgs e)
        {
            SetNsgaSettings(new NSGAII());
        }

        private void Nsga3MutationProbCheckedChanged(object sender, EventArgs e)
        {
            nsga3MutationProbUpDown.Enabled = nsga3MutationProbCheckBox.Checked;
        }

        private void Nsga3CrossoverCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            nsga3CrossoverComboBox.Enabled = nsga3CrossoverCheckBox.Checked;
        }

        private void Nsga3DefaultButton_Click(object sender, EventArgs e)
        {
            SetNsgaSettings(new NSGAIII());
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

        private void CmaEsWithMarginCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            cmaEsUseSaparableCmaCheckBox.Enabled = !cmaEsWithMarginCheckBox.Checked;
        }

        private void CmaEsUseSaparableCmaCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            cmaEsWithMarginCheckBox.Enabled = !cmaEsUseSaparableCmaCheckBox.Checked;
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
            SamplerSettings sampler = _settings.Optimize.Sampler;
            SetTpeSettings(sampler.Tpe);
            SetBoTorchSettings(sampler.BoTorch);
            SetNsgaSettings(sampler.NsgaII);
            SetNsgaSettings(sampler.NsgaIII);
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

        private void SetNsgaSettings(NSGAII nsga)
        {
            nsga2MutationProbCheckBox.Checked = nsga.MutationProb != null;
            nsga2MutationProbUpDown.Enabled = nsga2MutationProbCheckBox.Checked;
            nsga2MutationProbUpDown.Value = nsga.MutationProb != null
                ? (decimal)nsga.MutationProb : 0;
            nsga2CrossoverProbUpDown.Value = (decimal)nsga.CrossoverProb;
            nsga2SwappingProbUpDown.Value = (decimal)nsga.SwappingProb;
            nsga2PopulationSizeUpDown.Value = nsga.PopulationSize;
            nsga2CrossoverCheckBox.Checked = nsga.Crossover != string.Empty;
            nsga2CrossoverComboBox.Enabled = nsga.Crossover != string.Empty;
            nsga2CrossoverComboBox.SelectedIndex = SetNsgaCrossoverSetting(nsga.Crossover);
        }

        private static int SetNsgaCrossoverSetting(string crossover)
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
            cmaEsSigmaNumUpDown.Value = cmaEs.Sigma0 != null
                ? (decimal)cmaEs.Sigma0 : 0;
            cmaEsSigmaNumUpDown.Enabled = cmaEsSigmaCheckBox.Checked;

            cmaEsWarnIndependentSamplingCheckBox.Checked = cmaEs.WarnIndependentSampling;
            cmaEsConsiderPruneTrialsCheckBox.Checked = cmaEs.ConsiderPrunedTrials;
            cmaEsUseSaparableCmaCheckBox.Checked = cmaEs.UseSeparableCma;

            cmaEsRestartCheckBox.Checked = cmaEs.RestartStrategy != string.Empty;
            cmaEsIncPopSizeUpDown.Enabled = cmaEsRestartCheckBox.Checked;
            cmaEsIncPopSizeUpDown.Value = cmaEs.IncPopsize;
            cmaEsPopulationSizeUpDown.Enabled = cmaEsRestartCheckBox.Checked;
            cmaEsPopulationSizeUpDown.Value = cmaEs.PopulationSize != null
                ? (decimal)cmaEs.PopulationSize : 1;
        }

        private void SetQMCSettings(QuasiMonteCarlo qmc)
        {
            qmcTypeComboBox.SelectedIndex = qmc.QmcType == "sobol" ? 0 : 1;
            qmcScrambleCheckBox.Checked = qmc.Scramble;
            qmcWarnIndependentSamplingCheckBox.Checked = qmc.WarnIndependentSampling;
            qmcWarnAsyncSeedingCheckBox.Checked = qmc.WarnAsynchronousSeeding;
        }

        private SamplerSettings GetSamplerSettings()
        {
            return new SamplerSettings
            {
                Tpe = GetTpeSettings(),
                BoTorch = GetBoTorchSettings(),
                NsgaII = GetNsga2Settings(),
                NsgaIII = GetNsga3Settings(),
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

        private NSGAII GetNsga2Settings()
        {
            return new NSGAII
            {
                MutationProb = nsga2MutationProbCheckBox.Checked
                    ? (double?)nsga2MutationProbUpDown.Value : null,
                CrossoverProb = (double)nsga2CrossoverProbUpDown.Value,
                SwappingProb = (double)nsga2SwappingProbUpDown.Value,
                PopulationSize = (int)nsga2PopulationSizeUpDown.Value,
                Crossover = nsga2CrossoverCheckBox.Checked
                    ? nsga2CrossoverComboBox.Text : string.Empty,
            };
        }

        private NSGAIII GetNsga3Settings()
        {
            return new NSGAIII
            {
                MutationProb = nsga3MutationProbCheckBox.Checked
                    ? (double?)nsga3MutationProbUpDown.Value : null,
                CrossoverProb = (double)nsga3CrossoverProbUpDown.Value,
                SwappingProb = (double)nsga3SwappingProbUpDown.Value,
                PopulationSize = (int)nsga3PopulationSizeUpDown.Value,
                Crossover = nsga3CrossoverCheckBox.Checked
                    ? nsga3CrossoverComboBox.Text : string.Empty,
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
                RestartStrategy = cmaEsRestartCheckBox.Checked
                    ? "ipop" : string.Empty,
                IncPopsize = (int)cmaEsIncPopSizeUpDown.Value,
                PopulationSize = cmaEsRestartCheckBox.Checked
                    ? (int?)cmaEsPopulationSizeUpDown.Value : null,
                UseWarmStart = cmaEsWarmStartCmaEsCheckBox.Checked,
                WarmStartStudyName = cmaEsWarmStartComboBox.Text,
                WithMargin = cmaEsWithMarginCheckBox.Checked,
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

        private void RunGarbageCollectionComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            _settings.Optimize.GcAfterTrial = (GcAfterTrial)runGarbageCollectionComboBox.SelectedIndex;
        }
    }
}
