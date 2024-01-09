using System;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace Optuna.Storage.Journal
{
    public class JournalStorageFileJson
    {
        [JsonProperty("op_code")]
        public JournalOperation OpCode { get; set; }

        [JsonProperty("worker_id")]
        public string WorkerId { get; set; }

        [JsonProperty("study_name")]
        public string StudyName { get; set; }

        [JsonProperty("study_id")]
        public int? StudyId { get; set; }

        [JsonProperty("trial_id")]
        public int? TrialId { get; set; }

        [JsonProperty("datetime_start")]
        public DateTime? DatetimeStart { get; set; }

        [JsonProperty("datetime_complete")]
        public DateTime? DatetimeComplete { get; set; }

        [JsonProperty("directions")]
        public int[] Directions { get; set; }

        [JsonProperty("system_attr")]
        public Dictionary<string, object> SystemAttr { get; set; }

        [JsonProperty("user_attr")]
        public Dictionary<string, object> UserAttr { get; set; }

        [JsonProperty("param_name")]
        public string ParamName { get; set; }

        [JsonProperty("param_value_internal")]
        public double? ParamValueInternal { get; set; }

        [JsonProperty("distribution")]
        public string Distribution { get; set; }

        [JsonProperty("state")]
        public int? State { get; set; }

        [JsonProperty("values")]
        public double?[] Values { get; set; }

        public static JournalStorageFileJson Deserialize(string json)
        {
            return JsonConvert.DeserializeObject<JournalStorageFileJson>(json);
        }

        public static JournalStorageFileJson[] Deserialize(List<string> json)
        {
            return Deserialize(json.ToArray());
        }

        public static JournalStorageFileJson[] Deserialize(string[] json)
        {
            var journalStorageFileJson = new JournalStorageFileJson[json.Length];
            for (int i = 0; i < json.Length; i++)
            {
                journalStorageFileJson[i] = JsonConvert.DeserializeObject<JournalStorageFileJson>(json[i]);
            }

            return journalStorageFileJson;
        }
    }
}
