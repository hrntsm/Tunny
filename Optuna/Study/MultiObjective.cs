using System;
using System.Collections.Generic;
using System.Linq;

using Optuna.Trial;

namespace Optuna.Study
{
    public class MultiObjective
    {
        public static Trial.Trial[] GetParetoFrontTrials(List<Trial.Trial> trials, StudyDirection[] directions)
        {
            return GetParetoFrontTrialsByTrials(trials, directions);
        }

        private static Trial.Trial[] GetParetoFrontTrialsByTrials(List<Trial.Trial> trials, StudyDirection[] directions)
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

        private static Trial.Trial[] GetParetoFrontTrials2D(List<Trial.Trial> trials, StudyDirection[] directions)
        {
            List<Trial.Trial> targetTrials = trials.FindAll(trial => trial.State == TrialState.COMPLETE);

            int nTrials = targetTrials.Count;
            if (nTrials == 0)
            {
                return Array.Empty<Trial.Trial>();
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
            IOrderedEnumerable<Trial.Trial> sortedTrials = paretoFront.OrderBy(trial => trial.Number);
            return sortedTrials.ToArray();
        }

        private static Trial.Trial[] GetParetoFrontTrialsND(List<Trial.Trial> trials, StudyDirection[] directions)
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
        private static bool Dominates(Trial.Trial trial0, Trial.Trial trial1, StudyDirection[] directions)
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

        private static double NormalizeValue(double? value, StudyDirection direction)
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
