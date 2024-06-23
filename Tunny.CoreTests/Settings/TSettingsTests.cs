using System.IO;

using Tunny.Core.TEnum;
using Tunny.Core.Util;

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
            File.Delete("./settings.json");
        }

        [Fact]
        public void CreateNewSettingsWithStorageTest()
        {
            _ = new TSettings("./settings.json", "./fish.log", StorageType.Journal, true);
            Assert.True(File.Exists("./settings.json"));
            File.Delete("./settings.json");
        }

        [Fact]
        public void CreateNewSettingsInstanceTest()
        {
            var settings = new TSettings("settings.json", "./fish.log", StorageType.Journal, false);
            Assert.False(File.Exists("./settings.json"));
            Assert.Equal("./fish.log", settings.Storage.Path);
            Assert.Equal(StorageType.Journal, settings.Storage.Type);
        }
    }
}
