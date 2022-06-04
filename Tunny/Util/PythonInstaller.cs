using Python.Included;

namespace Tunny.Util
{
    public static class PythonInstaller
    {
        public static void Install(string path)
        {
            Installer.InstallPath = path;
            Installer.SetupPython();
            Installer.TryInstallPip();

            string[] packageList = GetTunnyPackageList();
            foreach (string package in packageList)
            {
                Installer.PipInstallModule(package);
            }
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
                "wcwidth"
            };
        }
    }
}
