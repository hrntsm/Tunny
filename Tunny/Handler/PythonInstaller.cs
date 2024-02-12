using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;

using Tunny.Util;

namespace Tunny.Handler
{
    public static class PythonInstaller
    {
        public static void Run(object sender, DoWorkEventArgs e)
        {
            TLog.MethodStart();
            var worker = sender as BackgroundWorker;
            worker?.ReportProgress(0, "Unzip library...");
            TLog.Info("Unzip library...");
            string[] packageList = UnzipLibraries();
            InstallPackages(worker, packageList);

            worker?.ReportProgress(100, "Finish!!");
        }

        private static string[] UnzipLibraries()
        {
            TLog.MethodStart();
            string envPath = TunnyVariables.TunnyEnvPath;
            string componentFolderPath = TunnyVariables.ComponentFolder;
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
                    FileName = TunnyVariables.TunnyEnvPath + "/python/python.exe",
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

        internal static string GetEmbeddedPythonPath()
        {
            TLog.MethodStart();
            return TunnyVariables.TunnyEnvPath + "/python";
        }
    }
}
