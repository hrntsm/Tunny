using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Newtonsoft.Json.Linq;

using Optuna.Study;
using Optuna.Trial;

namespace Optuna.Storage.Journal
{
    public class JournalStorage : BaseStorage
    {
        private readonly List<Study.Study> _studies = new List<Study.Study>();
        private int nextStudyId = 0;
        private readonly Dictionary<int, List<int>> _studyIdToTrialIds = new Dictionary<int, List<int>>();
        private readonly Dictionary<int, List<int>> _trialIdToStudyId = new Dictionary<int, List<int>>();
        private int _trialId = 0;

        public JournalStorage(string path)
        {
            if (File.Exists(path) == false)
            {
                throw new FileNotFoundException($"File not found: {path}");
            }

            var logs = new List<string>();
            using (FileStream fs = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (var sr = new StreamReader(fs))
                {
                    while (sr.Peek() >= 0)
                    {
                        logs.Add(sr.ReadLine());
                    }
                }
            }

            foreach (string log in logs)
            {
                if (log == "")
                {
                    continue;
                }

                var logObject = JObject.Parse(log);
                var opCode = (JournalOperation)Enum.ToObject(typeof(JournalOperation), (int)logObject["op_code"]);
                switch (opCode)
                {
                    case JournalOperation.CreateStudy:
                        StudyDirection[] studyDirections = logObject["directions"].ToObject<StudyDirection[]>();
                        string studyName = logObject["study_name"].ToString();
                        CreateNewStudy(studyDirections, studyName);
                        break;
                    case JournalOperation.DeleteStudy:
                        break;
                    case JournalOperation.SetStudyUserAttr:
                        {
                            int studyId = (int)logObject["study_id"];
                            var userAttr = (JObject)logObject["user_attr"];
                            foreach (KeyValuePair<string, JToken> item in userAttr)
                            {
                                var values = item.Value.Select(v => v.ToString()).ToList();
                                SetStudyUserAttr(studyId, item.Key, values);
                            }
                        }
                        break;
                    case JournalOperation.SetStudySystemAttr:
                        {
                            int studyId = (int)logObject["study_id"];
                            var systemAttr = (JObject)logObject["system_attr"];
                            foreach (KeyValuePair<string, JToken> item in systemAttr)
                            {
                                var values = item.Value.Select(v => v.ToString()).ToList();
                                SetStudySystemAttr((int)logObject["study_id"], item.Key, values);
                            }
                        }
                        break;
                    case JournalOperation.CreateTrial:
                        {
                            int studyId = (int)logObject["study_id"];
                            var trial = new Trial.Trial
                            {
                                DatetimeStart = (DateTime)logObject["datetime_start"]
                            };
                            CreateNewTrial(studyId, trial);
                        }
                        break;
                    case JournalOperation.SetTrialParam:
                        {
                            int trialId = (int)logObject["trial_id"];
                            string paramName = (string)logObject["param_name"];
                            double paramValueInternal = (double)logObject["param_value_internal"];
                            SetTrailParam(trialId, paramName, paramValueInternal, null);
                        }
                        break;
                    case JournalOperation.SetTrialStateValues:
                        {
                            int trialId = (int)logObject["trial_id"];
                            double[] trialValues = logObject["values"].Select(v => v.ToObject<double>()).ToArray();
                            var trialState = (TrialState)Enum.ToObject(typeof(TrialState), (int)logObject["state"]);
                            SetTrialStateValue(trialId, trialState, trialValues);
                        }
                        break;
                    case JournalOperation.SetTrialIntermediateValue:
                        break;
                    case JournalOperation.SetTrialUserAttr:
                        break;
                    case JournalOperation.SetTrialSystemAttr:
                        break;
                }
            }
        }

        public override void CheckTrialIsUpdatable(int trialId, TrialState trialState)
        {
            throw new NotImplementedException();
        }

        public override int CreateNewStudy(StudyDirection[] studyDirections, string studyName = "")
        {
            _studies.Add(new Study.Study(nextStudyId, studyName, studyDirections));
            return nextStudyId++;
        }

        public override int CreateNewTrial(int studyId, Trial.Trial templateTrial = null)
        {
            Trial.Trial trial = templateTrial != null ? templateTrial : new Trial.Trial();
            trial.TrialId = _trialId++;
            _studies[studyId].Trials.Add(trial);
            return _trialId;
        }

        public override void DeleteStudy(int studyId)
        {
            throw new NotImplementedException();
        }

        public override Study.Study[] GetAllStudies()
        {
            return _studies.ToArray();
        }

        public override Trial.Trial[] GetAllTrials(int studyId, bool deepcopy = true)
        {
            throw new NotImplementedException();
        }

        public override Trial.Trial GetBestTrial(int studyId)
        {
            throw new NotImplementedException();
        }

        public override int GetNTrials(int studyId)
        {
            throw new NotImplementedException();
        }

        public override StudyDirection[] GetStudyDirections(int studyId)
        {
            throw new NotImplementedException();
        }

        public override int GetStudyIdFromName(string studyName)
        {
            throw new NotImplementedException();
        }

        public override string GetStudyNameFromId(int studyId)
        {
            throw new NotImplementedException();
        }

        public override Dictionary<string, object> GetStudySystemAttrs(int studyId)
        {
            throw new NotImplementedException();
        }

        public override Dictionary<string, object> GetStudyUserAttrs(int studyId)
        {
            throw new NotImplementedException();
        }

        public override Trial.Trial GetTrial(int trialId)
        {
            throw new NotImplementedException();
        }

        public override int GetTrialIdFromStudyIdTrialNumber(int studyId, int trialNumber)
        {
            throw new NotImplementedException();
        }

        public override int GetTrialNumberFromId(int trialId)
        {
            Trial.Trial trial = _studies.First(s => s.Trials.Any(t => t.TrialId == trialId))
                            .Trials.First(t => t.TrialId == trialId);
            return trial.Number;
        }

        public override double GetTrialParam(int trialId, string paramName)
        {
            throw new NotImplementedException();
        }

        public override Dictionary<string, object> GetTrialParams(int trialId)
        {
            throw new NotImplementedException();
        }

        public override Dictionary<string, object> GetTrialSystemAttrs(int trialId)
        {
            throw new NotImplementedException();
        }

        public override Dictionary<string, object> GetTrialUserAttrs(int trialId)
        {
            throw new NotImplementedException();
        }

        public override void RemoveSession()
        {
            throw new NotImplementedException();
        }

        public override void SetStudySystemAttr(int studyId, string key, object value)
        {
            _studies[studyId].SystemAttrs[key] = value;
        }

        public override void SetStudyUserAttr(int studyId, string key, object value)
        {
            _studies[studyId].UserAttrs[key] = value;
        }

        public override void SetTrailParam(int trialId, string paramName, double paramValueInternal, object distribution)
        {
            Trial.Trial trial = _studies.First(s => s.Trials.Any(t => t.TrialId == trialId))
                            .Trials.First(t => t.TrialId == trialId);
            trial.Params[paramName] = paramValueInternal;
        }

        public override void SetTrialIntermediateValue(int trialId, int step, double intermediateValue)
        {
            throw new NotImplementedException();
        }

        public override bool SetTrialStateValue(int trialId, TrialState state, double[] values = null)
        {
            Trial.Trial trial = _studies.First(s => s.Trials.Any(t => t.TrialId == trialId))
                            .Trials.First(t => t.TrialId == trialId);
            trial.State = state;
            trial.Values = values;
            return true;
        }

        public override void SetTrialSystemAttr(int trialId, string key, object value)
        {
            throw new NotImplementedException();
        }

        public override void SetTrialUserAttr(int trialId, string key, object value)
        {
            throw new NotImplementedException();
        }
    }
}
