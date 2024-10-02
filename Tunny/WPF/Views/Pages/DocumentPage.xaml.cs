using System.Windows.Controls;

using Microsoft.Web.WebView2.Wpf;

namespace Tunny.WPF.Pages
{
    /// <summary>
    /// Interaction logic for DocumentPage.xaml
    /// </summary>
    public partial class DocumentPage : Page
    {
        public DocumentPage()
        {
            InitializeComponent();
            var webView = new WebView2
            {
                Source = new System.Uri("https://tunny-docs.deno.dev/")
            };
            DocumentWebViewFrame.Content = webView;
        }
    }
}
