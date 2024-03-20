using System.IO;

using Xunit;

namespace Tunny.Core.Settings.Tests
{
    public class TSettingsTests
    {
        [Fact]
        public void CreateNewSettingsFileTest()
        {
            var settings = new TSettings();
            settings.CreateNewSettingsFile("./settings.json");
            Assert.True(File.Exists("./settings.json"));
        }
    }
}
