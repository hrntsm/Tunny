using System.Data;
using System.Diagnostics;

namespace Tunny.Core.Util
{
    public static class OpenBrowser
    {
        public static void TunnyDocumentPage()
        {
            TLog.MethodStart();
            Process.Start($@"https://tunny-docs.deno.dev/");
        }

        public static void OptunaSamplerPage()
        {
            TLog.MethodStart();
            Process.Start($@"https://optuna.readthedocs.io/en/stable/reference/samplers/index.html");
        }

        public static void OptunaHubPage()
        {
            TLog.MethodStart();
            Process.Start($@"https://hub.optuna.org/");
        }

        public static void TunnyLicense()
        {
            TLog.MethodStart();
            Process.Start("https://github.com/hrntsm/Tunny/blob/main/LICENSE");
        }
        public static void PythonPackagesLicense()
        {
            TLog.MethodStart();
            Process.Start("https://github.com/hrntsm/Tunny/blob/main/PYTHON_PACKAGE_LICENSES");
        }

        public static void OtherLicense()
        {
            TLog.MethodStart();
            Process.Start("https://github.com/hrntsm/DesignExplorer/blob/gh-pages/license.txt");
        }
    }
}
