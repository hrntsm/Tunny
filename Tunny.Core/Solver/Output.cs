using System;
using System.Collections.Generic;
using System.Linq;

using Optuna.Storage;
using Optuna.Study;
using Optuna.Trial;

using Tunny.Core.Storage;
using Tunny.Core.Util;

namespace Tunny.Core.Solver
{
    public class Output
    {
        private readonly string _storagePath;

        public Output(string storagePath)
        {
            _storagePath = storagePath;
        }

        public Trial[] GetTargetTrial(int[] targetNumbers, string studyName)
        {
            TLog.MethodStart();
            IOptunaStorage storage = StorageHandler.GetStorage(_storagePath);
            Study targetStudy = storage.GetAllStudies().FirstOrDefault(s => s.StudyName == studyName);
            return targetStudy == null ? Array.Empty<Trial>() : GetTargetTrials(targetNumbers, targetStudy);
        }

        public string[] GetMetricNames(string studyName)
        {
            TLog.MethodStart();
            IOptunaStorage storage = StorageHandler.GetStorage(_storagePath);
            Study targetStudy = storage.GetAllStudies().FirstOrDefault(s => s.StudyName == studyName);
            if (targetStudy == null)
            {
                return Array.Empty<string>();
            }
            else
            {
                targetStudy.SystemAttrs.TryGetValue("study:metric_names", out object metricNames);
                return metricNames as string[] ?? Array.Empty<string>();
            }
        }

        private static Trial[] GetTargetTrials(int[] targetNumbers, Study study)
        {
            TLog.MethodStart();
            if (targetNumbers[0] == -1)
            {
                return study.BestTrials;
            }
            else if (targetNumbers[0] == -10)
            {
                return study.Trials.ToArray();
            }
            else
            {
                return UseTrialNumber(targetNumbers, study);
            }
        }

        private static Trial[] UseTrialNumber(IReadOnlyList<int> targetNumbers, Study study)
        {
            TLog.MethodStart();
            var trials = new List<Trial>();
            for (int i = 0; i < targetNumbers.Count; i++)
            {
                int target = targetNumbers[i];
                Trial trial = study.Trials.FirstOrDefault(t => t.Number == target);
                if (trial != null)
                {
                    trials.Add(trial);
                }
            }
            return trials.ToArray();
        }
    }
}
