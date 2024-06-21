using System;
using System.Reflection;

using Xunit;

namespace Optuna.Util.Tests
{
    public class ReadFileFromResourceTests
    {
        [Fact]
        public void TextInAssemblyTest()
        {
            string resourceName = "OptunaTests.TestFile.ReadFileFromResourceTest.txt";
            var assembly = Assembly.GetExecutingAssembly();
            string text = ReadFileFromResource.Text(assembly, resourceName);
            Assert.Equal("Hello, World!", text);
        }

        [Fact]
        public void TextNotInAssemblyTest()
        {
            string resourceName = "no_text.txt";
            Assert.Throws<ArgumentNullException>(() => ReadFileFromResource.Text(resourceName));
        }
    }
}
