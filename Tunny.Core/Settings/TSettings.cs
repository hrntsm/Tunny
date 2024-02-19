using System;
using System.IO;

using Newtonsoft.Json;

using Serilog.Events;

using Tunny.Core.TEnum;
using Tunny.Core.Util;

namespace Tunny.Core.Settings
{
    public class TSettings
    {
        public Version Version { get; set; } = TEnvVariables.Version;
        public Optimize Optimize { get; set; } = new Optimize();
        public Result Result { get; set; } = new Result();
        public string StudyName { get; set; } = string.Empty;
        public Storage Storage { get; set; } = new Storage();
        public bool CheckPythonLibraries { get; set; } = true;
        public LogEventLevel LogLevel { get; set; } = LogEventLevel.Information;

        public void Serialize(string path)
        {
            TLog.MethodStart();
            string json = JsonConvert.SerializeObject(this, Formatting.Indented);
            string dirPath = Path.GetDirectoryName(path);
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
            File.WriteAllText(path, json);
        }

        public static TSettings Deserialize(string json)
        {
            TLog.MethodStart();
            return JsonConvert.DeserializeObject<TSettings>(json);
        }

        public void CreateNewSettingsFile(string path)
        {
            TLog.MethodStart();
            Serialize(path);
        }

        public static TSettings LoadFromJson()
        {
            TSettings settings;
            string settingsPath = TEnvVariables.OptimizeSettingsPath;
            if (File.Exists(settingsPath))
            {
                TLog.Info("Load existing setting.json");
                settings = Deserialize(File.ReadAllText(settingsPath));
            }
            else
            {
                settings = new TSettings
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
