using System;
using System.ComponentModel;
using System.Linq;

using Python.Included;

namespace Tunny.Util
{
    public static class PythonInstaller
    {
        public static string Path = ".";
        public static void Run(object sender, DoWorkEventArgs e)
        {
            var worker = sender as BackgroundWorker;
            string[] packageList = GetTunnyPackageList();
            int installItems = packageList.Length + 2;

            Installer.InstallPath = Path;
            Installer.SetupPython();
            worker.ReportProgress(100 / installItems, "Now installing Python runtime...");
            Installer.TryInstallPip();
            worker.ReportProgress(200 / installItems, "Now installing pip...");

            InstallPackages(worker, packageList, installItems);

            worker.ReportProgress(100, "Finish!!");
        }

        private static void InstallPackages(BackgroundWorker worker, string[] packageList, int installItems)
        {
            for (int i = 0; i < packageList.Length; i++)
            {
                string packageName = packageList[i] == "plotly"
                    ? packageList[i] + "... This package will take time to install. Please wait"
                    : packageList[i];
                worker.ReportProgress((i + 2) * 100 / installItems, "Now installing " + packageName + "...");
                Installer.PipInstallModule(packageList[i]);
            }
        }

        internal static bool CheckPackagesIsInstalled()
        {
            Installer.InstallPath = Path;
            string[] packageList = GetTunnyPackageList();
            if (!Installer.IsPythonInstalled())
            {
                return false;
            }
            if (!Installer.IsPipInstalled())
            {
                return false;
            }
            foreach (string package in packageList)
            {
                string[] aa = { "bottle", "optuna-dashboard", "six", "PyYAML", "scikit-learn", "threadpoolctl" };
                if (!Installer.IsModuleInstalled(package) && !aa.Contains(package))
                {
                    return false;
                }
            }
            return true;
        }

        internal static string GetEmbeddedPythonPath()
        {
            Installer.InstallPath = Path;
            return Installer.EmbeddedPythonHome;
        }

        private static string[] GetTunnyPackageList()
        {
            return new[]
            {
                "alembic",
                "attrs",
                "autopage",
                "bottle",
                "cliff",
                "cmaes",
                "cmd2",
                "colorama",
                "colorlog",
                "greenlet",
                "joblib",
                "Mako",
                "MarkupSafe",
                "numpy",
                "optuna",
                "optuna-dashboard",
                "packaging",
                "pbr",
                "plotly",
                "prettytable",
                "pyparsing",
                "pyperclip",
                "pyreadline3",
                "PyYAML",
                "scikit-learn",
                "scipy",
                "six",
                "sklearn",
                "SQLAlchemy",
                "stevedore",
                "tenacity",
                "threadpoolctl",
                "tqdm",
                "wcwidth",
            };
        }
    }
}
