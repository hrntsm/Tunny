using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;

namespace Tunny.Handler
{
    public static class PythonInstaller
    {
        public static string TunnyEnvPath { get; private set; } = System.Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile) + "/.tunny_env";
        public static string ComponentFolderPath { get; set; }
        public static void Run(object sender, DoWorkEventArgs e)
        {
            var worker = sender as BackgroundWorker;
            worker?.ReportProgress(0, "Unzip library...");
            string[] packageList = UnzipLibraries();
            InstallPackages(worker, packageList);

            worker?.ReportProgress(100, "Finish!!");
        }

        private static string[] UnzipLibraries()
        {
            if (Directory.Exists(TunnyEnvPath + "/python"))
            {
                Directory.Delete(TunnyEnvPath + "/python", true);
            }
            ZipFile.ExtractToDirectory(ComponentFolderPath + "/Lib/python.zip", TunnyEnvPath + "/python");

            if (Directory.Exists(TunnyEnvPath + "/Lib/whl"))
            {
                Directory.Delete(TunnyEnvPath + "/Lib/whl", true);
            }
            ZipFile.ExtractToDirectory(ComponentFolderPath + "/Lib/whl.zip", TunnyEnvPath + "/Lib/whl");
            return Directory.GetFiles(TunnyEnvPath + "/Lib/whl");
        }

        private static void InstallPackages(BackgroundWorker worker, string[] packageList)
        {
            int num = packageList.Length;
            for (int i = 0; i < num; i++)
            {
                double progress = (double)i / num * 100d;
                string packageName = Path.GetFileName(packageList[i]).Split('-')[0];
                worker.ReportProgress((int)progress, "Now installing " + packageName + "...");
                var startInfo = new ProcessStartInfo
                {
                    FileName = TunnyEnvPath + "/python/python.exe",
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
        }

        internal static string GetEmbeddedPythonPath()
        {
            return TunnyEnvPath + "/python";
        }
    }
}
