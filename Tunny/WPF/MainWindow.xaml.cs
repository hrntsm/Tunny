using System.ComponentModel;
using System.Windows.Controls.Ribbon;

using Grasshopper.GUI;

using Tunny.Component.Optimizer;
using Tunny.Core.Settings;
using Tunny.Core.Util;
using Tunny.Process;
using Tunny.WPF.Common;

namespace Tunny.WPF
{
    public partial class MainWindow : RibbonWindow
    {
        internal readonly GH_DocumentEditor DocumentEditor;

        public MainWindow(OptimizeComponentBase component)
        {
            TLog.MethodStart();
            OptimizeProcess.TunnyWindow = this;
            component.GhInOutInstantiate();
            if (!component.GhInOut.IsLoadCorrectly)
            {
                TunnyMessageBox.Error_ComponentLoadFail();
                Close();
            }
            OptimizeProcess.Component = component;

            if (!TSettings.TryLoadFromJson(out OptimizeProcess.Settings))
            {
                TunnyMessageBox.Warn_SettingsJsonFileLoadFail();
            }

            InitializeComponent();
        }
    }
}
