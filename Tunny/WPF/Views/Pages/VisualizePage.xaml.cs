using System.Windows.Controls;

namespace Tunny.WPF.Views.Pages
{
    public partial class VisualizePage : Page
    {
        public VisualizePage()
        {
            InitializeComponent();
            var browser = new CefSharp.Wpf.ChromiumWebBrowser(@"C:\Users\dev\Desktop\my_plot.html");
            VisualizePlotFrame.Content = browser;
        }
    }
}
