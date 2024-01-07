using System.Collections.Generic;

using Tunny.Optuna.Study;
using Tunny.Optuna.Trial;

namespace Tunny.Optuna.Storage
{
    public abstract class BaseStorage
    {
        // Basic study manipulation
        public abstract int CreateNewStudy(StudyDirection[] studyDirections, string studyName);
        public abstract void DeleteStudy(int studyId);
        public abstract void SetStudyUserAttr(int studyId, string key, object value);
        public abstract void SetStudySystemAttr(int studyId, string key, object value);

        // Basic study access
        public abstract int GetStudyIdFromName(string studyName);
        public abstract string GetStudyNameFromId(int studyId);
        public abstract StudyDirection[] GetStudyDirections(int studyId);
        public abstract Dictionary<string, object> GetStudyUserAttrs(int studyId);
        public abstract Dictionary<string, object> GetStudySystemAttrs(int studyId);
        public abstract Study.Study[] GetAllStudies();

        // Basic trial manipulation
        public abstract int CreateNewTrial(int studyId, Trial.Trial templateTrial = null);
        public abstract void SetTrailParam(int trialId, string paramName, double paramValueInternal, object distribution);
        public abstract int GetTrialIdFromStudyIdTrialNumber(int studyId, int trialNumber);
        public abstract int GetTrialNumberFromId(int trialId);
        public abstract double GetTrialParam(int trialId, string paramName);
        public abstract bool SetTrialStateValue(int trialId, TrialState state, double[] values = null);
        public abstract void SetTrialIntermediateValue(int trialId, int step, double intermediateValue);
        public abstract void SetTrialUserAttr(int trialId, string key, object value);
        public abstract void SetTrialSystemAttr(int trialId, string key, object value);

        // Basic trial access
        public abstract Trial.Trial GetTrial(int trialId);
        public abstract Trial.Trial[] GetAllTrials(int studyId, bool deepcopy = true);
        public abstract int GetNTrials(int studyId);
        public abstract Trial.Trial GetBestTrial(int studyId);
        public abstract Dictionary<string, object> GetTrialParams(int trialId);
        public abstract Dictionary<string, object> GetTrialUserAttrs(int trialId);
        public abstract Dictionary<string, object> GetTrialSystemAttrs(int trialId);
        public abstract void RemoveSession();
        public abstract void CheckTrialIsUpdatable(int trialId, TrialState trialState);
    }
}
