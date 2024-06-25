using System;
using System.IO;
using System.Reflection;

namespace Tunny.Core.Util
{
    public static class TEnvVariables
    {
        public static Version Version { get; } = Assembly.GetExecutingAssembly().GetName().Version;
        public static string DefaultStoragePath { get; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "fish.log");
        public static string TunnyEnvPath { get; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".tunny_env");
        public static string LogPath { get; } = Path.Combine(TunnyEnvPath, "logs");
        public static string DesignExplorerPath { get; } = Path.Combine(TunnyEnvPath, "TT-DesignExplorer");
        public static string OptimizeSettingsPath { get; } = Path.Combine(TunnyEnvPath, "settings.json");
        public static string PythonPath { get; } = Path.Combine(TunnyEnvPath, "python");
        public static string ComponentFolder { get; } = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        public static string ExampleDirPath { get; } = Path.Combine(ComponentFolder, "Examples", "Grasshopper");
        public static Version OldStorageVersion { get; } = new Version("0.9.1");
        public static IntPtr GrasshopperWindowHandle { get; set; }

        public static string TmpDirPath
        {
            get
            {
                string path = Path.Combine(TunnyEnvPath, "tmp");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                return path;
            }
        }
    }
}
