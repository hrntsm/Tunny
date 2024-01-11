using System.Collections.Generic;

namespace Optuna.Study
{
    public class Study
    {
        public Dictionary<string, double> BestParams { get; set; }
        public Trial.Trial BestTrial { get; set; }
        public Trial.Trial[] BestTrials { get; set; }
        public double BestValue { get; set; }
        public StudyDirection Direction { get; set; }
        public StudyDirection[] Directions { get; set; }
        public string StudyName { get; private set; }
        public Dictionary<string, object> SystemAttrs { get; set; }
        public Dictionary<string, object> UserAttrs { get; set; }
        public List<Trial.Trial> Trials { get; set; }

        private readonly int _studyId;

        public Study(int studyID, string studyName, StudyDirection[] directions)
        {
            _studyId = studyID;
            StudyName = studyName;
            Directions = directions;
            SystemAttrs = new Dictionary<string, object>();
            UserAttrs = new Dictionary<string, object>();
            Trials = new List<Trial.Trial>();
        }
    }
}
