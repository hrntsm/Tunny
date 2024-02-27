using System;
using System.Collections.Generic;

namespace Optuna.Study
{
    public class StudySummary
    {
        private readonly StudyDirection[] _directions;

        public int StudyId { get; }
        public string StudyName { get; }
        public Trial.Trial BestTrial { get; }
        public Dictionary<string, object> SystemAttrs { get; }
        public Dictionary<string, object> UserAttrs { get; }
        public int NTrials { get; }
        public DateTime DatetimeStart { get; }

        public StudySummary(string studyName,
                            StudyDirection? direction,
                            Trial.Trial bestTrial,
                            Dictionary<string, object> userAttrs,
                            Dictionary<string, object> systemAttrs,
                            int nTrials,
                            DateTime datetimeStart,
                            int studyId,
                            StudyDirection[] directions = null)
        {
            StudyName = studyName;
            if (direction is null && directions is null)
            {
                throw new ArgumentException("Specify one of `direction` and `directions`.");
            }
            else if (directions != null)
            {
                _directions = directions;
            }
            else if (direction != null)
            {
                _directions = new StudyDirection[] { (StudyDirection)direction };
            }
            else
            {
                throw new ArgumentException("Specify only one of `direction` and `directions`.");
            }
            BestTrial = bestTrial;
            UserAttrs = userAttrs;
            SystemAttrs = systemAttrs;
            NTrials = nTrials;
            DatetimeStart = datetimeStart;
            StudyId = studyId;
        }

        public StudyDirection Direction
        {
            get
            {
                if (_directions.Length > 1)
                {
                    throw new InvalidOperationException("The study has multiple directions.");
                }
                return _directions[0];
            }
        }

        public StudyDirection[] Directions
        {
            get
            {
                return _directions;
            }
        }
    }
}
