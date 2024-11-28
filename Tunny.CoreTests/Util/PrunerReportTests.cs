using Xunit;

namespace Tunny.Core.Util.Tests
{
    public class PrunerReportTests
    {
        [Fact]
        public void DeserializeTest()
        {
            var report = PrunerReport.Deserialize("TestFile/TunnyPrunerReport.json");
            Assert.Equal(1, report.IntermediateValue);
            Assert.Equal("TunnyPrunerReport", report.Attribute);
            Assert.Equal(10, report.TrialTellValue);
        }
    }
}
