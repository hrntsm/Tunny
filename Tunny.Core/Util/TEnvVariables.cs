using System;
using System.IO;
using System.Reflection;

namespace Tunny.Core.Util
{
    public static class TEnvVariables
    {
        public static Version Version { get; } = Assembly.GetExecutingAssembly().GetName().Version;
        public static string TunnyEnvPath { get; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".tunny_env");
        public static string LogPath { get; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".tunny_env", "logs");
        public static string OptimizeSettingsPath { get; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".tunny_env", "settings.json");
        public static string TmpDirPath { get; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".tunny_env", "tmp");
        public static string ComponentFolder { get; } = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        public static Version OldStorageVersion { get; } = new Version("0.9.1");
    }
}
