using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;

using Tunny.Core.Util;

namespace Tunny.Core.Handler
{
    public static class PythonInstaller
    {
        public static void Run(object sender, DoWorkEventArgs e)
        {
            TLog.MethodStart();
            var worker = sender as BackgroundWorker;
            CheckProcess(worker);
            string[] packageList = UnzipLibraries(worker);
            InstallPackages(worker, packageList);

            worker?.ReportProgress(100, "Finish!!");
        }

        private static void CheckProcess(BackgroundWorker worker)
        {
            TLog.MethodStart();
            worker.ReportProgress(0, "Check process...");
            TLog.Info("Check optuna-dashboard process...");
            if (Optuna.Dashboard.Handler.KillExistDashboardProcess())
            {
                string txt = "Killed process: optuna-dashboard";
                TLog.Info(txt);
                worker.ReportProgress(0, txt);
            }
        }

        private static string[] UnzipLibraries(BackgroundWorker worker)
        {
            TLog.MethodStart();
            worker.ReportProgress(0, "Unzip library...");
            TLog.Info("Unzip library...");
            string envPath = TEnvVariables.TunnyEnvPath;
            string componentFolderPath = TEnvVariables.ComponentFolder;
            TLog.Info("Unzip Python libraries: " + envPath);
            if (Directory.Exists(envPath + "/python"))
            {
                Directory.Delete(envPath + "/python", true);
            }
            ZipFile.ExtractToDirectory(componentFolderPath + "/Lib/python.zip", envPath + "/python");

            if (Directory.Exists(envPath + "/Lib/whl"))
            {
                Directory.Delete(envPath + "/Lib/whl", true);
            }
            ZipFile.ExtractToDirectory(componentFolderPath + "/Lib/whl.zip", envPath + "/Lib/whl");
            return Directory.GetFiles(envPath + "/Lib/whl");
        }

        private static void InstallPackages(BackgroundWorker worker, string[] packageList)
        {
            TLog.MethodStart();
            int num = packageList.Length;
            for (int i = 0; i < num; i++)
            {
                double progress = (double)i / num * 100d;
                string packageName = Path.GetFileName(packageList[i]).Split('-')[0];
                string state = "Installing " + packageName + "...";
                worker.ReportProgress((int)progress, state);
                TLog.Info(state);
                var startInfo = new ProcessStartInfo
                {
                    FileName = TEnvVariables.TunnyEnvPath + "/python/python.exe",
                    Arguments = "-m pip install --no-deps " + packageList[i],
                    WindowStyle = ProcessWindowStyle.Hidden,
                    CreateNoWindow = true,
                    UseShellExecute = false,
                };
                using (var process = new Process())
                {
                    process.StartInfo = startInfo;
                    process.Start();
                    process.WaitForExit();
                }
            }
            TLog.Info("Finish to install Python");
        }
    }
}
