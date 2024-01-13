using System.Collections.Generic;

using Tunny.Optuna.Study;
using Tunny.Optuna.Trial;

namespace Tunny.Optuna.Storage.Journal
{
    public class JournalStorage : BaseStorage
    {
        public JournalStorage()
        {
        }

        public override void CheckTrialIsUpdatable(int trialId, TrialState trialState)
        {
            throw new System.NotImplementedException();
        }

        public override int CreateNewStudy(StudyDirection[] studyDirections, string studyName)
        {
            throw new System.NotImplementedException();
        }

        public override int CreateNewTrial(int studyId, Trial.Trial templateTrial = null)
        {
            throw new System.NotImplementedException();
        }

        public override void DeleteStudy(int studyId)
        {
            throw new System.NotImplementedException();
        }

        public override Study.Study[] GetAllStudies()
        {
            throw new System.NotImplementedException();
        }

        public override Trial.Trial[] GetAllTrials(int studyId, bool deepcopy = true)
        {
            throw new System.NotImplementedException();
        }

        public override Trial.Trial GetBestTrial(int studyId)
        {
            throw new System.NotImplementedException();
        }

        public override int GetNTrials(int studyId)
        {
            throw new System.NotImplementedException();
        }

        public override StudyDirection[] GetStudyDirections(int studyId)
        {
            throw new System.NotImplementedException();
        }

        public override int GetStudyIdFromName(string studyName)
        {
            throw new System.NotImplementedException();
        }

        public override string GetStudyNameFromId(int studyId)
        {
            throw new System.NotImplementedException();
        }

        public override Dictionary<string, object> GetStudySystemAttrs(int studyId)
        {
            throw new System.NotImplementedException();
        }

        public override Dictionary<string, object> GetStudyUserAttrs(int studyId)
        {
            throw new System.NotImplementedException();
        }

        public override Trial.Trial GetTrial(int trialId)
        {
            throw new System.NotImplementedException();
        }

        public override int GetTrialIdFromStudyIdTrialNumber(int studyId, int trialNumber)
        {
            throw new System.NotImplementedException();
        }

        public override int GetTrialNumberFromId(int trialId)
        {
            throw new System.NotImplementedException();
        }

        public override double GetTrialParam(int trialId, string paramName)
        {
            throw new System.NotImplementedException();
        }

        public override Dictionary<string, object> GetTrialParams(int trialId)
        {
            throw new System.NotImplementedException();
        }

        public override Dictionary<string, object> GetTrialSystemAttrs(int trialId)
        {
            throw new System.NotImplementedException();
        }

        public override Dictionary<string, object> GetTrialUserAttrs(int trialId)
        {
            throw new System.NotImplementedException();
        }

        public override void RemoveSession()
        {
            throw new System.NotImplementedException();
        }

        public override void SetStudySystemAttr(int studyId, string key, object value)
        {
            throw new System.NotImplementedException();
        }

        public override void SetStudyUserAttr(int studyId, string key, object value)
        {
            throw new System.NotImplementedException();
        }

        public override void SetTrailParam(int trialId, string paramName, double paramValueInternal, object distribution)
        {
            throw new System.NotImplementedException();
        }

        public override void SetTrialIntermediateValue(int trialId, int step, double intermediateValue)
        {
            throw new System.NotImplementedException();
        }

        public override bool SetTrialStateValue(int trialId, TrialState state, double[] values = null)
        {
            throw new System.NotImplementedException();
        }

        public override void SetTrialSystemAttr(int trialId, string key, object value)
        {
            throw new System.NotImplementedException();
        }

        public override void SetTrialUserAttr(int trialId, string key, object value)
        {
            throw new System.NotImplementedException();
        }
    }
}
