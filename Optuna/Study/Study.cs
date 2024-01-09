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
        public string StudyName { get; set; }
    }
}
