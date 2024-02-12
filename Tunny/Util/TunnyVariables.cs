using System;
using System.IO;
using System.Reflection;

namespace Tunny.Util
{
    public static class TunnyVariables
    {
        public static Version Version { get; } = Assembly.GetExecutingAssembly().GetName().Version;
        public static string TunnyEnvPath { get; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".tunny_env");
        public static string LogPath { get; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".tunny_env", "logs");
        public static string OptimizeSettingsPath { get; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".tunny_env", "settings.json");
        public static string ComponentFolder { get; } = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
    }
}
