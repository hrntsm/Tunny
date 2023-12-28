using System.Diagnostics;

namespace Tunny.Handler
{
    public static class DashboardHandler
    {
        public static void RunDashboardProcess(string dashboardPath, string storageArgument)
        {
            CheckExistDashboardProcess();
            var dashboard = new Process();
            dashboard.StartInfo.FileName = dashboardPath;
            dashboard.StartInfo.Arguments = storageArgument;
            dashboard.StartInfo.UseShellExecute = false;
            dashboard.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;
            dashboard.Start();

            var browser = new Process();
            browser.StartInfo.FileName = @"http://127.0.0.1:8080/";
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
