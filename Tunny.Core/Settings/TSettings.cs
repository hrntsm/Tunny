using System;
using System.Collections.Generic;
using System.IO;

using Newtonsoft.Json;

using Serilog.Events;

using Tunny.Core.TEnum;
using Tunny.Core.Util;

namespace Tunny.Core.Settings
{
    public class TSettings
    {
        public string Version { get; set; } = TEnvVariables.Version.ToString();
        public Optimize Optimize { get; set; } = new Optimize();
        public Result Result { get; set; } = new Result();
        public string StudyName { get; set; } = string.Empty;
        public Storage Storage { get; set; } = new Storage();
        public bool CheckPythonLibraries { get; set; } = true;
        public LogEventLevel LogLevel { get; set; } = LogEventLevel.Verbose;
        public OptunaHub OptunaHub { get; set; } = new OptunaHub();

        public TSettings()
        {
        }

        public TSettings(string settingsPath, string storagePath, StorageType storageType, bool createNewFile)
        {
            Storage = new Storage
            {
                Path = storagePath,
                Type = storageType
            };

            if (createNewFile)
            {
                CreateNewSettingsFile(settingsPath);
            }
        }

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

        public static TSettings Deserialize(string settingsPath)
        {
            TLog.MethodStart();
            try
            {
                return JsonConvert.DeserializeObject<TSettings>(File.ReadAllText(settingsPath));
            }
            catch (Exception e)
            {
                TLog.Error(e.Message);
                TLog.Warning("Create new settings.json");
                File.Delete(settingsPath);
                return new TSettings(settingsPath, TEnvVariables.DefaultStoragePath, StorageType.Journal, true);
            }
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
                settings = Deserialize(settingsPath);
            }
            else
            {
                settings = new TSettings(TEnvVariables.OptimizeSettingsPath, TEnvVariables.DefaultStoragePath, StorageType.Journal, true);
            }
            return settings;
        }
    }

    public class OptunaHub
    {
        public Dictionary<string, string> Sampler { get; set; } = new Dictionary<string, string>();
        public Dictionary<string, string> Pruner { get; set; } = new Dictionary<string, string>();
        public Dictionary<string, string> Visualization { get; set; } = new Dictionary<string, string>();

        public OptunaHub()
        {
            Sampler.Clear();
            Sampler.Add(
        "MOEA/D",
        "def sampler():\n" +
                "    mod = optunahub.load_module(\"samplers/moead\")\n" +
                "    return mod.MOEADSampler(\n" +
                "        population_size=50,\n" +
                "        scalar_aggregation_func=\"tchebycheff\",\n" +
                "        n_neighbors=5,\n" +
                "    )");
        }

        public void CheckInput(HubType type, string key)
        {
            TLog.MethodStart();
            switch (type)
            {
                case HubType.Sampler:
                    break;
                case HubType.Pruner:
                    break;
                case HubType.Visualization:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }

    public enum HubType
    {
        Sampler,
        Pruner,
        Visualization
    }
}
