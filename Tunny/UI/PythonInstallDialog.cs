using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Forms;

using Tunny.Core.Handler;

namespace Tunny.UI
{
    public partial class PythonInstallDialog : Form
    {
        public PythonInstallDialog()
        {
            InitializeComponent();

            installBackgroundWorker.DoWork += PythonInstaller.Run;
            installBackgroundWorker.ProgressChanged += InstallerProgressChangedHandler;
            installBackgroundWorker.WorkerReportsProgress = true;
        }

        private void OptimizationWindow_Load(object sender, EventArgs e)
        {
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;

            installBackgroundWorker.RunWorkerAsync();
        }

        private void FormClosingXButton(object sender, FormClosingEventArgs e)
        {
        }

        private void InstallerProgressChangedHandler(object sender, ProgressChangedEventArgs e)
        {
            string txt = e.UserState.ToString();
            installProgressBar.Value = e.ProgressPercentage;
            installItemLabel.Text = txt;
            installProgressBar.Update();

            if (txt == "Finish!!")
            {
                Close();
            }
            else if (txt == "Killed process: optuna-dashboard")
            {
                WPF.Common.TunnyMessageBox.Show(
                 "Stopped the Optuna Dashboard process to prevent the installation of Python libraries.",
                 "Warning",
                 MessageBoxButton.OK,
                 MessageBoxImage.Warning);
            }
        }
    }
}
