using System.Windows.Controls;

using Tunny.WPF.Common;

namespace Tunny.WPF.Views.Pages
{
    public partial class HelpPage : Page
    {
        public HelpPage()
        {
            InitializeComponent();
        }

        public HelpPage(HelpType type)
        {
            InitializeComponent();
            var browser = new CefSharp.Wpf.ChromiumWebBrowser();
            switch (type)
            {
                case HelpType.TunnyAbout:
                    browser.Address = "https://tunny-docs.deno.dev/";
                    break;
                case HelpType.TunnyDocument:
                    browser.Address = "https://tunny-docs.deno.dev/docs/getting-start";
                    break;
                case HelpType.OptunaSampler:
                    browser.Address = "https://optuna.readthedocs.io/en/stable/reference/samplers/index.html";
                    break;
                case HelpType.OptunaHub:
                    browser.Address = "https://hub.optuna.org/";
                    break;
                case HelpType.TunnyLicense:
                    browser.Address = "https://github.com/hrntsm/Tunny/blob/main/LICENSE";
                    break;
                case HelpType.PythonPackagesLicense:
                    browser.Address = "https://github.com/hrntsm/Tunny/blob/main/PYTHON_PACKAGE_LICENSES";
                    break;
                case HelpType.OtherLicense:
                    browser.Address = "https://github.com/hrntsm/Tunny/blob/main/THIRD_PARTY_LICENSES";
                    break;
                default:
                    throw new System.ArgumentOutOfRangeException(nameof(type), type, null);
            }
            HelpPageFrame.Content = browser;
        }
    }
}
