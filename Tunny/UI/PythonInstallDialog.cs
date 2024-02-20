using System;
using System.ComponentModel;
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
            installProgressBar.Value = e.ProgressPercentage;
            installItemLabel.Text = e.UserState.ToString();
            installProgressBar.Update();
        }
    }
}
