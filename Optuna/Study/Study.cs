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
        private readonly BaseStorage _storage;

        public Study(BaseStorage storage, int studyID, string studyName, StudyDirection[] directions)
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

            return GetParetoFrontTrials();
        }

        private Trial.Trial[] GetParetoFrontTrials()
        {
            return GetParetoFrontTrialsByTrials(Trials, Directions);
        }

        private Trial.Trial[] GetParetoFrontTrialsByTrials(List<Trial.Trial> trials, StudyDirection[] directions)
        {
            if (directions.Length == 2)
            {
                return GetParetoFrontTrials2D(trials, directions);
            }
            else
            {
                return GetParetoFrontTrialsND(trials, directions);
            }
        }

        private Trial.Trial[] GetParetoFrontTrials2D(List<Trial.Trial> trials, StudyDirection[] directions)
        {
            List<Trial.Trial> targetTrials = trials.FindAll(trial => trial.State == TrialState.COMPLETE);

            int nTrials = targetTrials.Count;
            if (nTrials == 0)
            {
                return new Trial.Trial[0];
            }

            targetTrials.Sort((trial1, trial2) =>
            {
                double trial1Value1 = NormalizeValue(trial1.Values[0], directions[0]);
                double trial1Value2 = NormalizeValue(trial1.Values[1], directions[1]);

                double trial2Value1 = NormalizeValue(trial2.Values[0], directions[0]);
                double trial2Value2 = NormalizeValue(trial2.Values[1], directions[1]);

                int compare1 = trial1Value1.CompareTo(trial2Value1);
                return compare1 == 0 ? trial1Value2.CompareTo(trial2Value2) : compare1;
            });

            Trial.Trial lastNonDominatedTrial = targetTrials[0];
            var paretoFront = new List<Trial.Trial> { lastNonDominatedTrial };
            foreach (Trial.Trial trial in targetTrials)
            {
                if (Dominates(lastNonDominatedTrial, trial, directions))
                {
                    continue;
                }
                paretoFront.Add(trial);
                lastNonDominatedTrial = trial;
            }
            paretoFront.OrderBy(trial => trial.Number);
            return paretoFront.ToArray();
        }

        private Trial.Trial[] GetParetoFrontTrialsND(List<Trial.Trial> trials, StudyDirection[] directions)
        {
            var paretoFront = new List<Trial.Trial>();
            List<Trial.Trial> targetTrials = trials.FindAll(trial => trial.State == TrialState.COMPLETE);

            foreach (Trial.Trial trial in targetTrials)
            {
                bool dominated = false;
                foreach (Trial.Trial otherTrial in targetTrials)
                {
                    if (trial == otherTrial)
                    {
                        continue;
                    }
                    if (Dominates(otherTrial, trial, directions))
                    {
                        dominated = true;
                        break;
                    }
                }
                if (!dominated)
                {
                    paretoFront.Add(trial);
                }
            }

            return paretoFront.ToArray();
        }

        /// <summary>
        /// NOTE: Optuna returns the exact same result with False, but this method returns True.
        /// Therefore, there may be fewer than Optuna results.
        /// </summary>
        private bool Dominates(Trial.Trial trial0, Trial.Trial trial1, StudyDirection[] directions)
        {
            double[] value0 = trial0.Values;
            double[] value1 = trial1.Values;

            IEnumerable<double> normalizedValues0 = value0.Zip(directions, (v, d) => NormalizeValue(v, d));
            IEnumerable<double> normalizedValues1 = value1.Zip(directions, (v, d) => NormalizeValue(v, d));

            if (normalizedValues0 == normalizedValues1)
            {
                return false;
            }

            return normalizedValues0.Zip(normalizedValues1, (v0, v1) => v0 <= v1).All(x => x);
        }

        private double NormalizeValue(double? value, StudyDirection direction)
        {
            if (value == null)
            {
                value = double.PositiveInfinity;
            }

            if (direction == StudyDirection.Maximize)
            {
                value = -value.Value;
            }
            return value.Value;
        }
    }
}
