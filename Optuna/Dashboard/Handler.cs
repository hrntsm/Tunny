using System;
using System.Diagnostics;
using System.IO;

namespace Optuna.Dashboard
{
    public class Handler
    {
        private readonly string _dashboardPath;
        private readonly string _storage;
        private readonly string _host;
        private readonly string _port;
        private readonly string _artifactDir;

        public Handler(string dashboardPath, string storagePath, string artifactDir = null, string host = "127.0.0.1", string port = "8080")
        {
            _dashboardPath = dashboardPath;
            _storage = GetStorageArgument(storagePath);
            _host = host;
            _port = port;
            _artifactDir = artifactDir ?? Path.GetDirectoryName(storagePath) + "/artifacts";
            if (!Directory.Exists(_artifactDir))
            {
                Directory.CreateDirectory(_artifactDir);
            }
        }

        private static string GetStorageArgument(string path)
        {
            switch (Path.GetExtension(path))
            {
                case ".sqlite3":
                case ".db":
                    return @"sqlite:///" + $"\"{path}\"";
                case ".log":
                    return $"\"{path}\"";
                default:
                    throw new NotImplementedException();
            }
        }

        public void Run()
        {
            CheckExistDashboardProcess();
            string argument = $"{_storage} --host {_host} --port {_port} --artifact-dir {_artifactDir}";

            var dashboard = new Process();
            dashboard.StartInfo.FileName = _dashboardPath;
            dashboard.StartInfo.Arguments = argument;
            dashboard.StartInfo.UseShellExecute = false;
            dashboard.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;
            dashboard.Start();

            var browser = new Process();
            browser.StartInfo.FileName = $@"http://{_host}:{_port}/";
            browser.StartInfo.UseShellExecute = true;
            browser.Start();
        }

        private static void CheckExistDashboardProcess()
        {
            Process[] dashboardProcess = Process.GetProcessesByName("optuna-dashboard");
            if (dashboardProcess.Length > 0)
            {
                foreach (Process p in dashboardProcess)
                {
                    p.Kill();
                }
            }
        }
    }
}
