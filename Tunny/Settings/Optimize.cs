namespace Tunny.Settings
{
    public class Optimize
    {
        public Sampler Sampler { get; set; } = new Sampler();
        public int NumberOfTrials { get; set; } = 100;
        public bool LoadExistStudy { get; set; } = true;
        public int SelectSampler { get; set; }
    }
}
