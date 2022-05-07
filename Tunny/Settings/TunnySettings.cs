using Newtonsoft.Json;

namespace Tunny.Settings
{
    public class TunnySettings
    {
        public Optimize Optimize { get; set; } = new Optimize();
        public Result Result { get; set; } = new Result();
        public string StudyName { get; set; } = "study1";
        public string Storage { get; set; } = "/Tunny_Opt_Result.db";

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