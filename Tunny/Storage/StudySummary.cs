using System.Collections.Generic;

using Tunny.Optuna.Trial;

namespace Tunny.Storage
{
    public class StudySummary
    {
        public int StudyId { get; set; }
        public string StudyName { get; set; }
        public Dictionary<string, string[]> UserAttributes { get; set; }
        public Dictionary<string, string[]> SystemAttributes { get; set; }
        public int NTrials { get; set; }
        public List<Trial> Trials { get; set; }
    }
}
