using System.Reflection;

using Newtonsoft.Json;

namespace Tunny.Settings
{
    public class TunnySettings
    {
        public string Version { get; set; } = Assembly.GetExecutingAssembly().GetName().Version.ToString(3);
        public Optimize Optimize { get; set; } = new Optimize();
        public Result Result { get; set; } = new Result();
        public string StudyName { get; set; } = "study1";
        public string StoragePath { get; set; } = "/Fish.db";

        public void Serialize(string path)
        {
            string json = JsonConvert.SerializeObject(this, Formatting.Indented);
            System.IO.File.WriteAllText(path, json);
        }

        public static TunnySettings Deserialize(string json)
        {
            return JsonConvert.DeserializeObject<TunnySettings>(json);
        }

        public void CreateNewSettingsFile(string path)
        {
            Serialize(path);
        }
    }
}
