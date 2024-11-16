using System;
using System.Windows;

using Tunny.Core.Util;

namespace Tunny.WPF.Common
{
    internal static partial class TunnyMessageBox
    {
        internal static void Warn_SettingsJsonFileLoadFail()
        {
            TLog.MethodStart();
            Show(
                "Failed to load settings file. Start with default settings.",
                "Tunny",
                MessageBoxButton.OK,
                MessageBoxImage.Warning
            );
        }

        internal static void Warn_VariableMustLargerThanZeroInLogScale()
        {
            TLog.MethodStart();
            Show(
                "Variable value must be larger than 0 if LogScale is True.",
                "Tunny",
                MessageBoxButton.OK,
                MessageBoxImage.Warning
            );
        }
    }
}
