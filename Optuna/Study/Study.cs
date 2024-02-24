using System;
using System.Collections.Generic;
using System.Linq;

using Optuna.Storage;
using Optuna.Trial;

namespace Optuna.Study
{
    public class Study
    {
        public List<Trial.Trial> Trials { get; }
        public Dictionary<string, object> SystemAttrs { get; }
        public Dictionary<string, object> UserAttrs { get; }
        public StudyDirection Direction { get; }
        public StudyDirection[] Directions { get; }
        public string StudyName { get; }
        public int StudyId { get; }

        private readonly bool _isMultiObjective;
        private readonly IOptunaStorage _storage;

        public Study(IOptunaStorage storage, int studyID, string studyName, StudyDirection[] directions)
        {
            StudyId = studyID;
            _storage = storage;

            StudyName = studyName;
            if (directions.Length == 1)
            {
                Direction = directions[0];
                Directions = directions;
                _isMultiObjective = false;
            }
            else
            {
                Directions = directions;
                _isMultiObjective = true;
            }
            Trials = new List<Trial.Trial>();
            SystemAttrs = new Dictionary<string, object>();
            UserAttrs = new Dictionary<string, object>();
        }

        public Dictionary<string, object> BestParams
        {
            get
            {
                Trial.Trial bestTrial = BestTrial;
                return bestTrial.Params.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            }
        }

        public double BestValue
        {
            get
            {
                Trial.Trial bestTrial = BestTrial;
                return bestTrial.Values[0];
            }
        }
        public Trial.Trial BestTrial
        {
            get
            {
                return GetBestTrial();
            }
        }

        public Trial.Trial[] BestTrials
        {
            get
            {
                return GetBestTrials();
            }
        }

        private Trial.Trial GetBestTrial()
        {
            if (_isMultiObjective)
            {
                throw new ArgumentException("Study is multi-objective.");
            }

            return _storage.GetBestTrial(StudyId);
        }

        private Trial.Trial[] GetBestTrials()
        {
            if (!_isMultiObjective)
            {
                throw new ArgumentException("Study is not multi-objective.");
            }

            return MultiObjective.GetParetoFrontTrials(Trials, Directions);
        }

        public static StudySummary[] GetAllStudySummaries(IOptunaStorage storage)
        {
            Study[] studies = storage.GetAllStudies();
            var studySummaries = new StudySummary[studies.Length];
            for (int i = 0; i < studies.Length; i++)
            {
                Trial.Trial[] allTrials = storage.GetAllTrials(studies[i].StudyId);
                IEnumerable<Trial.Trial> completeTrials = allTrials.Where(trial => trial.State == TrialState.COMPLETE);

                StudyDirection direction = StudyDirection.NotSet;
                StudyDirection[] directions = Array.Empty<StudyDirection>();
                Trial.Trial bestTrial = null;
                if (studies[i].Directions.Length == 1)
                {
                    direction = studies[i].Directions[0];
                    bestTrial = storage.GetBestTrial(studies[i].StudyId);
                }
                else
                {
                    directions = studies[i].Directions;
                }

                DateTime datetimeStart = DateTime.MinValue;
                if (allTrials.Length != 0)
                {
                    datetimeStart = allTrials.Min(trial => trial.DatetimeStart);
                }

                studySummaries[i] = new StudySummary(studies[i].StudyName,
                                                     direction,
                                                     bestTrial,
                                                     studies[i].UserAttrs,
                                                     studies[i].SystemAttrs,
                                                     allTrials.Length,
                                                     datetimeStart,
                                                     studies[i].StudyId,
                                                     directions);
            }
            return studySummaries;
        }

        public static dynamic CreateStudy(dynamic optuna, string studyName, dynamic sampler, string[] directions, dynamic storage, bool loadIfExists = true)
        {
            return optuna.create_study(
                sampler: sampler,
                directions: directions,
                storage: storage,
                study_name: studyName,
                load_if_exists: loadIfExists
            );
        }

        public static dynamic LoadStudy(dynamic optuna, dynamic storage, string studyName)
        {
            return optuna.load_study(storage: storage, study_name: studyName);
        }
    }
}
