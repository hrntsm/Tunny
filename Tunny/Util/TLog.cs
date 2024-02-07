using System;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;

using Serilog;

namespace Tunny.Util
{
    public static class TLog
    {
        public static void InitializeLogger()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console(formatProvider: CultureInfo.InvariantCulture)
                .WriteTo.File(path: TunnyVariables.LogPath + "/log_.txt", rollingInterval: RollingInterval.Day, formatProvider: CultureInfo.InvariantCulture)
                .CreateLogger();
            Log.Information("Tunny is loaded.");
            CheckAndDeleteOldLogFiles();
        }

        private static void CheckAndDeleteOldLogFiles()
        {
            string logDirectory = TunnyVariables.LogPath;
            string logFilePattern = "*.txt";

            DateTime threshold = DateTime.Now.AddDays(-7);

            var directory = new DirectoryInfo(logDirectory);
            FileInfo[] logFiles = directory.GetFiles(logFilePattern);
            foreach (FileInfo file in logFiles)
            {
                if (file.LastWriteTime < threshold)
                {
                    file.Delete();
                }
            }
        }

        public static void CloseAndFlush()
        {
            Log.CloseAndFlush();
        }

        public static void Verbose(string message)
        {
            Log.Verbose(message);
        }

        public static void Debug(string message)
        {
            Log.Debug(message);
        }

        public static void Info(string message)
        {
            Log.Information(message);
        }

        public static void Warning(string message)
        {
            Log.Warning(message);
        }

        public static void Error(string message)
        {
            Log.Error(message);
        }

        public static void Fatal(string message)
        {
            Log.Fatal(message);
        }

        public static void MethodStart(
            string message = "started.",
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = -1)
        {
            string fileName = Path.GetFileName(filePath);
            Log.Verbose($"|{fileName}|{memberName}|{lineNumber}|{message}");
        }
    }
}
