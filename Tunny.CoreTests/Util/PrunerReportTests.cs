using Xunit;

namespace Tunny.Core.Util.Tests
{
    public class PrunerReportTests
    {
        [Fact]
        public void DeserializeTest()
        {
            var report = PrunerReport.Deserialize("TestFile/TunnyPrunerReport.json");
            Assert.Equal(1, report.Value);
            Assert.Equal("TunnyPrunerReport", report.Attribute);
        }
    }
}
