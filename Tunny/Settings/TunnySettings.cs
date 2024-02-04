using System;
using System.IO;
using System.Reflection;

using Newtonsoft.Json;

using Serilog;

using Tunny.Enum;
using Tunny.Util;

namespace Tunny.Settings
{
    public class TunnySettings
    {
        public string Version { get; set; } = Assembly.GetExecutingAssembly().GetName().Version.ToString(3);
        public Optimize Optimize { get; set; } = new Optimize();
        public Result Result { get; set; } = new Result();
        public string StudyName { get; set; } = string.Empty;
        public Storage Storage { get; set; } = new Storage();
        public bool CheckPythonLibraries { get; set; } = true;

        public void Serialize(string path)
        {
            string json = JsonConvert.SerializeObject(this, Formatting.Indented);
            string dirPath = Path.GetDirectoryName(path);
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
            File.WriteAllText(path, json);
        }

        public static TunnySettings Deserialize(string json)
        {
            return JsonConvert.DeserializeObject<TunnySettings>(json);
        }

        public void CreateNewSettingsFile(string path)
        {
            Serialize(path);
        }

        public static TunnySettings LoadFromJson()
        {
            TunnySettings settings;
            string settingsPath = TunnyVariables.OptimizeSettingsPath;
            if (File.Exists(settingsPath))
            {
                Log.Information("Load existing setting.json");
                settings = Deserialize(File.ReadAllText(settingsPath));
            }
            else
            {
                settings = new TunnySettings
                {
                    Storage = new Storage
                    {
                        Path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "fish.log"),
                        Type = StorageType.Journal
                    }
                };
                settings.CreateNewSettingsFile(settingsPath);
            }
            return settings;
        }
    }
}
