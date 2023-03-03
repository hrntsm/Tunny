using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Newtonsoft.Json;

using Python.Runtime;

using Tunny.Util;

namespace Tunny.Storage
{
    public class JournalStorage : PythonInit, IStorage
    {
        [JsonProperty("op_code")]
        public int OpCode { get; set; }

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
        public Dictionary<string, string[]> SystemAttr { get; set; }

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

        [JsonIgnore]
        public dynamic Storage { get; set; }

        public JournalStorage() : base()
        {
        }

        public static JournalStorage Deserialize(string json)
        {
            return JsonConvert.DeserializeObject<JournalStorage>(json);
        }

        public static JournalStorage[] Deserialize(List<string> json)
        {
            return Deserialize(json.ToArray());
        }

        public static JournalStorage[] Deserialize(string[] json)
        {
            var journalStorage = new JournalStorage[json.Length];
            for (int i = 0; i < json.Length; i++)
            {
                journalStorage[i] = JsonConvert.DeserializeObject<JournalStorage>(json[i]);
            }

            return journalStorage;
        }

        public StudySummary[] GetStudySummaries(string storagePath)
        {
            var journalStorage = new List<string>();
            foreach (string line in File.ReadLines(storagePath))
            {
                journalStorage.Add(line);
            }
            JournalStorage[] res = Deserialize(journalStorage);
            var studyName = res.Where(x => x.OpCode == 0).Select(x => x.StudyName).Distinct().ToList();
            IEnumerable<IGrouping<int?, JournalStorage>> userAttr = res.Where(x => x.OpCode == 2)
                .GroupBy(x => x.StudyId);
            var studySummaries = new StudySummary[studyName.Count];

            int i = 0;
            foreach (IGrouping<int?, JournalStorage> group in userAttr)
            {
                var studySummary = new StudySummary
                {
                    StudyId = group.Key.Value,
                    StudyName = studyName[i],
                    UserAttributes = new Dictionary<string, string[]>(),
                    SystemAttributes = new Dictionary<string, string[]>(),
                    Trials = new List<Trial>()
                };
                foreach (JournalStorage j in group)
                {
                    foreach (KeyValuePair<string, object> item in j.UserAttr)
                    {
                        if (item.Value is string str)
                        {
                            studySummary.UserAttributes.Add(item.Key, str.Split(','));
                        }
                        else if (item.Value is string[] strArray)
                        {
                            studySummary.UserAttributes.Add(item.Key, strArray);
                        }
                    }
                }
                studySummaries[i++] = studySummary;
            }

            return studySummaries;
        }

        public dynamic CreateNewStorage(string storagePath)
        {
            PythonEngine.Initialize();
            using (Py.GIL())
            {
                dynamic optuna = Py.Import("optuna");
                string filePath = storagePath;
                dynamic lockObj = optuna.storages.JournalFileOpenLock(filePath);
                Storage = optuna.storages.JournalStorage(optuna.storages.JournalFileStorage(filePath, lock_obj: lockObj));
            }
            PythonEngine.Shutdown();

            return Storage;
        }

        public void DuplicateStudyInStorage(string fromStudyName, string toStudyName, string storagePath)
        {
            string storage = "sqlite:///" + storagePath;
            PythonEngine.Initialize();
            using (Py.GIL())
            {
                dynamic optuna = Py.Import("optuna");
                optuna.copy_study(from_study_name: fromStudyName, to_study_name: toStudyName, from_storage: storage, to_storage: storage);
            }
            PythonEngine.Shutdown();
        }
    }
}