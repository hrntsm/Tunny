using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;

using Serilog;

using Tunny.Util;

namespace Tunny.Handler
{
    public static class PythonInstaller
    {
        public static string ComponentFolderPath { get; set; }
        public static void Run(object sender, DoWorkEventArgs e)
        {
            var worker = sender as BackgroundWorker;
            worker?.ReportProgress(0, "Unzip library...");
            Log.Information("Unzip library...");
            string[] packageList = UnzipLibraries();
            InstallPackages(worker, packageList);

            worker?.ReportProgress(100, "Finish!!");
        }

        private static string[] UnzipLibraries()
        {
            string envPath = TunnyVariables.TunnyEnvPath;
            Log.Information("Unzip Python libraries: " + envPath);
            if (Directory.Exists(envPath + "/python"))
            {
                Directory.Delete(envPath + "/python", true);
            }
            ZipFile.ExtractToDirectory(ComponentFolderPath + "/Lib/python.zip", envPath + "/python");

            if (Directory.Exists(envPath + "/Lib/whl"))
            {
                Directory.Delete(envPath + "/Lib/whl", true);
            }
            ZipFile.ExtractToDirectory(ComponentFolderPath + "/Lib/whl.zip", envPath + "/Lib/whl");
            return Directory.GetFiles(envPath + "/Lib/whl");
        }

        private static void InstallPackages(BackgroundWorker worker, string[] packageList)
        {
            int num = packageList.Length;
            for (int i = 0; i < num; i++)
            {
                double progress = (double)i / num * 100d;
                string packageName = Path.GetFileName(packageList[i]).Split('-')[0];
                string state = "Installing " + packageName + "...";
                worker.ReportProgress((int)progress, state);
                Log.Information(state);
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
            Log.Information("Finish to install Python");
        }

        internal static string GetEmbeddedPythonPath()
        {
            return TunnyVariables.TunnyEnvPath + "/python";
        }
    }
}
