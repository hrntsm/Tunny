using Newtonsoft.Json;

namespace Tunny.Settings
{
    class TunnySettings
    {
        public Optimize Optimize { get; set; } = new Optimize();
        public Result Result { get; set; } = new Result();

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
            var settings = new TunnySettings();
            settings.Serialize(path);
        }
    }

    class Optimize
    {
        public Random Random { get; set; } = new Random();
        public Tpe Tpe { get; set; } = new Tpe();
        public CmaEs CmaEs { get; set; } = new CmaEs();
        public NSGAII NsgaII { get; set; } = new NSGAII();
        public int NumberOfTrials { get; set; } = 100;
        public bool LoadExistStudy { get; set; } = true;
        public string StudyName { get; set; } = "study1";
        public int SelectSampler { get; set; } = 0;
    }

    class Result
    {
        public string RestoreNumberString { get; set; } = "-1";
        public int SelectVisualizeType { get; set; } = 3;
    }
}