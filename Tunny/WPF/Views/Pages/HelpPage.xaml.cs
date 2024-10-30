using System;
using System.Windows.Controls;

using CefSharp;

using Tunny.WPF.Common;

namespace Tunny.WPF.Views.Pages
{
    public partial class HelpPage : Page, IDisposable
    {
        private readonly CefSharp.Wpf.ChromiumWebBrowser _browser;

        public HelpPage()
        {
            InitializeComponent();
            _browser = new CefSharp.Wpf.ChromiumWebBrowser();
            HelpPageFrame.Content = _browser;
        }
        internal void OpenSite(HelpType? type)
        {
            switch (type)
            {
                case null:
                case HelpType.TunnyAbout:
                    _browser.Address = "https://tunny-docs.deno.dev/";
                    break;
                case HelpType.TunnyDocument:
                    _browser.Address = "https://tunny-docs.deno.dev/docs/getting-start";
                    break;
                case HelpType.OptunaSampler:
                    _browser.Address = "https://optuna.readthedocs.io/en/stable/reference/samplers/index.html";
                    break;
                case HelpType.OptunaHub:
                    _browser.Address = "https://hub.optuna.org/";
                    break;
                case HelpType.TunnyLicense:
                    _browser.Address = "https://github.com/hrntsm/Tunny/blob/main/LICENSE";
                    break;
                case HelpType.PythonPackagesLicense:
                    _browser.Address = "https://github.com/hrntsm/Tunny/blob/main/PYTHON_PACKAGE_LICENSES";
                    break;
                case HelpType.OtherLicense:
                    _browser.Address = "https://github.com/hrntsm/Tunny/blob/main/THIRD_PARTY_LICENSES";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        public void Dispose()
        {
            Cef.Shutdown();
            _browser?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
