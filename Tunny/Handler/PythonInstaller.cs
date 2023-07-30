using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;

namespace Tunny.Handler
{
    public static class PythonInstaller
    {
        public static string Path { get; set; } = ".";
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
            if (Directory.Exists(Path + "/python"))
            {
                Directory.Delete(Path + "/python", true);
            }
            ZipFile.ExtractToDirectory(Path + "/Lib/python.zip", Path + "/python");

            if (Directory.Exists(Path + "/Lib/whl"))
            {
                Directory.Delete(Path + "/Lib/whl", true);
            }
            ZipFile.ExtractToDirectory(Path + "/Lib/whl.zip", Path + "/Lib/whl");
            return Directory.GetFiles(Path + "/Lib/whl");
        }

        private static void InstallPackages(BackgroundWorker worker, string[] packageList)
        {
            int num = packageList.Length;
            for (int i = 0; i < num; i++)
            {
                double progress = (double)i / num * 100d;
                string packageName = System.IO.Path.GetFileName(packageList[i]).Split('-')[0];
                worker.ReportProgress((int)progress, "Now installing " + packageName + "...");
                var startInfo = new ProcessStartInfo
                {
                    FileName = Path + "/python/python.exe",
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
            return Path + "/python";
        }
    }
}
