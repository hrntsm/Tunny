using System.IO;

using Xunit;

namespace Tunny.Core.Util.Tests
{
    public class GooConverterTests
    {
        [Fact]
        public void GetTmpDirPathTest()
        {
            string path = TEnvVariables.TmpDirPath;
            Assert.True(Directory.Exists(path));
        }
    }
}
