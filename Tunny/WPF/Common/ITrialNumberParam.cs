using System.Windows;

namespace Tunny.WPF.Common
{
    internal interface ITrialNumberParam
    {
        string Param1Label { get; set; }
        string Param2Label { get; set; }
        Visibility Param2Visibility { get; set; }
    }
}
