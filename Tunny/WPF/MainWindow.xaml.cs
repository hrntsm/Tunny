using System.Windows.Controls.Ribbon;

using Tunny.Component.Optimizer;
using Tunny.Core.Settings;
using Tunny.Core.Util;
using Tunny.WPF.Common;
using Tunny.WPF.ViewModels;

namespace Tunny.WPF
{
    public partial class MainWindow : RibbonWindow
    {
        private static SharedItems SharedItems => SharedItems.Instance;

        public MainWindow(OptimizeComponentBase component)
        {
            TLog.MethodStart();
            SharedItems.Clear();
            SharedItems.TunnyWindow = this;
            component.GhInOutInstantiate();
            if (!component.GhInOut.IsLoadCorrectly)
            {
                TunnyMessageBox.Error_ComponentLoadFail();
                Close();
            }
            SharedItems.Component = component;

            if (!TSettings.TryLoadFromJson(out TSettings settings))
            {
                TunnyMessageBox.Warn_SettingsJsonFileLoadFail();
            }
            else
            {
                SharedItems.Settings = settings;
            }

            InitializeComponent();
            Closing += (sender, e) => ((MainWindowViewModel)DataContext).SaveSettingsFile();
        }
    }
}
