using Tunny.Enum;
using Tunny.Settings.Sampler;

namespace Tunny.Settings
{
    public class Optimize
    {
        public SamplerSettings Sampler { get; set; } = new SamplerSettings();
        public int NumberOfTrials { get; set; } = 100;
        public bool ContinueStudy { get; set; }
        public bool CopyStudy { get; set; }
        public int SelectSampler { get; set; }
        public double Timeout { get; set; }
        public GcAfterTrial GcAfterTrial { get; set; } = GcAfterTrial.HasGeometry;
        public bool ShowRealtimeResult { get; set; }
    }
}
