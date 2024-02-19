using System;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;

using Serilog;
using Serilog.Core;
using Serilog.Events;

using Tunny.Core.Settings;

namespace Tunny.Core.Util
{
    public static class TLog
    {
        private static readonly LoggingLevelSwitch LevelSwitch = new LoggingLevelSwitch();

        public static void InitializeLogger()
        {
            SetInitialLogLevels();
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.ControlledBy(LevelSwitch)
                .WriteTo.Console(formatProvider: CultureInfo.InvariantCulture)
                .WriteTo.File(path: TEnvVariables.LogPath + "/log_.txt", rollingInterval: RollingInterval.Day, formatProvider: CultureInfo.InvariantCulture)
                .CreateLogger();
            Log.Information("Tunny is loaded.");
            CheckAndDeleteOldLogFiles();
        }

        private static void SetInitialLogLevels()
        {
            var settings = TSettings.LoadFromJson();
            LevelSwitch.MinimumLevel = settings.LogLevel;
        }

        public static void SetLoggingLevel(LogEventLevel level)
        {
            LevelSwitch.MinimumLevel = level;
        }

        private static void CheckAndDeleteOldLogFiles()
        {
            string logDirectory = TEnvVariables.LogPath;
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
