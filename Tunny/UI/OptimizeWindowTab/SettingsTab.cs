using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace Tunny.UI
{
    public partial class OptimizationWindow : Form
    {
        private void SettingsOpenAPIPage_Click(object sender, EventArgs e)
        {
            int apiIndex = settingsAPIComboBox.SelectedIndex;
            switch (apiIndex)
            {
                case 0: // TPE
                    Process.Start("https://optuna.readthedocs.io/en/stable/reference/generated/optuna.samplers.TPESampler.html");
                    break;
                case 1: // NSGA2
                    Process.Start("https://optuna.readthedocs.io/en/stable/reference/generated/optuna.samplers.NSGAIISampler.html");
                    break;
                case 2: // CMA-ES
                    Process.Start("https://optuna.readthedocs.io/en/stable/reference/generated/optuna.samplers.CmaEsSampler.html");
                    break;
                case 3: // Random
                    Process.Start("https://optuna.readthedocs.io/en/stable/reference/generated/optuna.samplers.RandomSampler.html");
                    break;
            }
        }

        private void SettingsFromJson_Click(object sender, EventArgs e)
        {
            LoadSettingJson();
            InitializeUIValues();
        }

        private void SettingsToJson_Click(object sender, EventArgs e)
        {
            SaveUIValues();
        }

        private void SettingsFolderOpen_Click(object sender, EventArgs e)
        {
            SaveUIValues();
            Process.Start("EXPLORER.EXE", _component.GhInOut.ComponentFolder);
        }
    }
}
