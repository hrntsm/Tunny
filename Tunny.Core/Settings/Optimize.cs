using Tunny.Core.TEnum;

namespace Tunny.Core.Settings
{
    public class Optimize
    {
        public Sampler Sampler { get; set; } = new Sampler();
        public int NumberOfTrials { get; set; } = 100;
        public bool ContinueStudy { get; set; }
        public bool CopyStudy { get; set; }
        public SamplerType SelectSampler { get; set; }
        public double Timeout { get; set; }
        public GcAfterTrial GcAfterTrial { get; set; } = GcAfterTrial.HasGeometry;
        public bool ShowRealtimeResult { get; set; }
        public bool IgnoreDuplicateSampling { get; set; }
        public bool DisableViewportDrawing { get; set; }
    }
}
