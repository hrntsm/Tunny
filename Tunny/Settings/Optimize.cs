using Tunny.Solver;

namespace Tunny.Settings
{
    public class Optimize
    {
        public Sampler Sampler { get; set; } = new Sampler();
        public int NumberOfTrials { get; set; } = 100;
        public bool ContinueStudy { get; set; }
        public bool CopyStudy { get; set; }
        public int SelectSampler { get; set; }
        public double Timeout { get; set; }
        public GcAfterTrial GcAfterTrial { get; set; } = GcAfterTrial.HasGeometry;
    }
}
