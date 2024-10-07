using System.Windows.Controls;

namespace Tunny.WPF.Views.Pages
{
    public partial class VisualizePage : Page
    {
        public VisualizePage()
        {
            InitializeComponent();
            var browser = new CefSharp.Wpf.ChromiumWebBrowser(@"https://github.com/hrntsm/Tunny/blob/main/LICENSE");
            VisualizePlotFrame.Content = browser;
        }
    }
}
