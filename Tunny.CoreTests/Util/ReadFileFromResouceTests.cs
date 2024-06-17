using System;
using System.Reflection;

using Xunit;

namespace Tunny.Core.Util.Tests
{
    public class ReadFileFromResourceTests
    {
        [Fact]
        public void TextInAssemblyTest()
        {
            string resourceName = "Tunny.CoreTests.TestFile.ReadFileFromResourceTest.txt";
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
