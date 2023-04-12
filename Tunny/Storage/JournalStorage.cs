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

        [JsonIgnore]
        public dynamic Storage { get; set; }

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
            if (File.Exists(storagePath) == false)
            {
                return Array.Empty<StudySummary>();
            }
            var journalStorageString = new List<string>();
            using (FileStream fs = File.Open(storagePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (var sr = new StreamReader(fs))
                {
                    while (sr.Peek() >= 0)
                    {
                        journalStorageString.Add(sr.ReadLine());
                    }
                }
            }

            JournalStorage[] storage = Deserialize(journalStorageString);
            var studyName = storage.Where(x => x.OpCode == 0).Select(x => x.StudyName).Distinct().ToList();
            IEnumerable<IGrouping<int?, JournalStorage>> userAttr = storage.Where(x => x.OpCode == 2)
                .GroupBy(x => x.StudyId);
            var studySummaries = new StudySummary[studyName.Count];

            SetStudySummaries(studyName, userAttr, studySummaries);

            return studySummaries;
        }

        private static void SetStudySummaries(IReadOnlyList<string> studyName, IEnumerable<IGrouping<int?, JournalStorage>> userAttr, IList<StudySummary> studySummaries)
        {
            int i = 0;
            foreach (IGrouping<int?, JournalStorage> group in userAttr)
            {
                if (group.Key == null)
                {
                    continue;
                }

                var studySummary = new StudySummary
                {
                    StudyId = group.Key.Value,
                    StudyName = studyName[i],
                    UserAttributes = new Dictionary<string, string[]>(),
                    SystemAttributes = new Dictionary<string, string[]>(),
                    Trials = new List<Trial>()
                };
                SetStudySummaryValue(group, studySummary);
                studySummaries[i++] = studySummary;
            }
        }

        private static void SetStudySummaryValue(IEnumerable<JournalStorage> group, StudySummary studySummary)
        {
            foreach (JournalStorage journal in group)
            {
                foreach (KeyValuePair<string, object> item in journal.UserAttr)
                {
                    string[] values = Array.Empty<string>();
                    if (item.Value is string str)
                    {
                        values = str.Split(',');
                    }
                    else if (item.Value is string[] strArray)
                    {
                        values = strArray;
                    }

                    if (studySummary.UserAttributes.TryGetValue(item.Key, out string[] value))
                    {
                        _ = value.Union(values);
                    }
                    else
                    {
                        studySummary.UserAttributes.Add(item.Key, values);
                    }
                }
            }
        }

        public dynamic CreateNewStorage(bool useInnerPythonEngine, Settings.Storage storageSetting)
        {
            string storagePath = storageSetting.GetOptunaStoragePath();
            if (useInnerPythonEngine)
            {
                PythonEngine.Initialize();
                using (Py.GIL())
                {
                    CreateStorageProcess(storagePath);
                }
                PythonEngine.Shutdown();
            }
            else
            {
                CreateStorageProcess(storagePath);
            }

            return Storage;
        }

        private void CreateStorageProcess(string storagePath)
        {
            dynamic optuna = Py.Import("optuna");
            dynamic lockObj = optuna.storages.JournalFileOpenLock(storagePath);
            Storage = optuna.storages.JournalStorage(optuna.storages.JournalFileStorage(storagePath, lock_obj: lockObj));
        }

        public void DuplicateStudyInStorage(string fromStudyName, string toStudyName, Settings.Storage storageSetting)
        {
            PythonEngine.Initialize();
            using (Py.GIL())
            {
                dynamic optuna = Py.Import("optuna");
                dynamic storage = CreateNewStorage(false, storageSetting);
                optuna.copy_study(from_study_name: fromStudyName, to_study_name: toStudyName, from_storage: storage, to_storage: storage);
            }
            PythonEngine.Shutdown();
        }
    }
}
