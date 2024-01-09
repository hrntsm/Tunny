using System;
using System.Collections.Generic;

using Optuna.Trial;

namespace Optuna.Storage.Journal
{
    public class JournalStorageReplayResult
    {
        private readonly int _logNumberRead;
        private readonly string _workerIdPrefix;
        private readonly Dictionary<int, Study.Study> _studies;
        private readonly Dictionary<int, Trial.Trial> _trials;
        private readonly Dictionary<int, int[]> _studyIdToTrialIds;
        private readonly Dictionary<int, int> _trialIdToStudyId;
        private readonly int _nextStudyId;
        private readonly Dictionary<string, int> _workerIdToOwnedTrialId;

        public JournalStorageReplayResult(string workerIdPrefix)
        {
            _logNumberRead = 0;
            _workerIdPrefix = workerIdPrefix;
            _studies = new Dictionary<int, Study.Study>();
            _trials = new Dictionary<int, Trial.Trial>();
            _studyIdToTrialIds = new Dictionary<int, int[]>();
            _trialIdToStudyId = new Dictionary<int, int>();
            _nextStudyId = 0;
            _workerIdToOwnedTrialId = new Dictionary<string, int>();
        }

        public void ApplyLogs(JournalStorageFileJson[] logs)
        {
            foreach (JournalStorageFileJson log in logs)
            {
                switch (log.OpCode)
                {
                    case JournalOperation.CreateStudy:
                        ApplyCrateStudy(log);
                        break;
                    case JournalOperation.DeleteStudy:
                        ApplyDeleteStudy(log);
                        break;
                    case JournalOperation.SetStudyUserAttr:
                        ApplySetStudyUserAttr(log);
                        break;
                    case JournalOperation.SetStudySystemAttr:
                        ApplySetStudySystemAttr(log);
                        break;
                    case JournalOperation.CreateTrial:
                        ApplyCreateTrial(log);
                        break;
                    case JournalOperation.SetTrialParam:
                        ApplySetTrialParam(log);
                        break;
                    case JournalOperation.SetTrialStateValues:
                        ApplySetTrialStateValues(log);
                        break;
                    case JournalOperation.SetTrialIntermediateValue:
                        ApplySetTrialIntermediateValue(log);
                        break;
                    case JournalOperation.SetTrialUserAttr:
                        ApplySetTrialUserAttr(log);
                        break;
                    case JournalOperation.SetTrialSystemAttr:
                        ApplySetTrialSystemAttr(log);
                        break;
                }
            }
        }

        public Study.Study GetStudy(int studyId)
        {
            if (!_studies.TryGetValue(studyId, out Study.Study value))
            {
                throw new KeyNotFoundException($"Study ID {studyId} not found.");
            }
            return value;
        }

        public Study.Study[] GetAllStudies()
        {
            var studies = new Study.Study[_studies.Count];
            _studies.Values.CopyTo(studies, 0);
            return studies;
        }

        public Trial.Trial GetTrial(int trialId)
        {
            if (!_trials.TryGetValue(trialId, out Trial.Trial value))
            {
                throw new KeyNotFoundException($"Trial ID {trialId} not found.");
            }
            return value;
        }

        public Trial.Trial[] GetAllTrials(int studyId, TrialState[] states = null)
        {
            if (!_studies.ContainsKey(studyId))
            {
                throw new KeyNotFoundException($"Study ID {studyId} not found.");
            }

            var trials = new List<Trial.Trial>();
            foreach (int trialId in _studyIdToTrialIds[studyId])
            {
                Trial.Trial trial = _trials[trialId];
                if (states != null && !Array.Exists(states, state => state == trial.State))
                {
                    trials.Add(trial);
                }
            }
            return trials.ToArray();
        }

        private void ApplyCrateStudy(JournalStorageFileJson log)
        {
            throw new NotImplementedException();
        }

        private void ApplyDeleteStudy(JournalStorageFileJson log)
        {
            throw new NotImplementedException();
        }

        private void ApplySetStudyUserAttr(JournalStorageFileJson log)
        {
            throw new NotImplementedException();
        }

        private void ApplySetStudySystemAttr(JournalStorageFileJson log)
        {
            throw new NotImplementedException();
        }

        private void ApplyCreateTrial(JournalStorageFileJson log)
        {
            throw new NotImplementedException();
        }

        private void ApplySetTrialParam(JournalStorageFileJson log)
        {
            throw new NotImplementedException();
        }

        private void ApplySetTrialStateValues(JournalStorageFileJson log)
        {
            throw new NotImplementedException();
        }

        private void ApplySetTrialIntermediateValue(JournalStorageFileJson log)
        {
            throw new NotImplementedException();
        }

        private void ApplySetTrialUserAttr(JournalStorageFileJson log)
        {
            throw new NotImplementedException();
        }

        private void ApplySetTrialSystemAttr(JournalStorageFileJson log)
        {
            throw new NotImplementedException();
        }
    }
}
