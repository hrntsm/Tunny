using System;
using System.Windows.Forms;

using BayesOpt.Component;

using Grasshopper.GUI;

namespace BayesOpt.UI
{
    public partial class OptimizationWindow : Form
    {
        private readonly WithUI _component;

        public OptimizationWindow(WithUI component)
        {
            InitializeComponent();
            _component = component;
        }

        private void OptimizationWindow_Load(object sender, EventArgs e)
        {

        }

        private void RunOptimize_Click(object sender, EventArgs e)
        {
            GH_DocumentEditor ghCanvas = Owner as GH_DocumentEditor;
            ghCanvas.DisableUI();
        }
    }
}
