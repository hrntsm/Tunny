using System;
using System.Windows.Forms;

namespace Tunny.UI
{
    public partial class PythonInstallDialog : Form
    {
        public PythonInstallDialog()
        {
            InitializeComponent();
        }

        private void OptimizationWindow_Load(object sender, EventArgs e)
        {
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
        }

        private void FormClosingXButton(object sender, FormClosingEventArgs e)
        {
        }
    }
}

