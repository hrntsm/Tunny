using System;
using System.IO;

using Xunit;

namespace Tunny.Core.Util.Tests
{
    public class SelectionCsvReaderTests
    {
        [Fact]
        public void DashboardCsvTest()
        {
            var reader = new SelectionCsvReader("TestFile/Dashboard.csv");
            int[] result = reader.ReadSelection(CsvType.Dashboard);

            Assert.Equal(4, result.Length);
            Assert.Equal(1, result[0]);
            Assert.Equal(8, result[1]);
            Assert.Equal(10, result[2]);
            Assert.Equal(11, result[3]);
        }

        [Fact]
        public void DesignExplorerCsvTest()
        {
            var reader = new SelectionCsvReader("TestFile/DesignExplorer.csv");
            int[] result = reader.ReadSelection(CsvType.DesignExplorer);

            Assert.Equal(4, result.Length);
            Assert.Equal(4, result[0]);
            Assert.Equal(5, result[1]);
            Assert.Equal(9, result[2]);
            Assert.Equal(16, result[3]);
        }

        [Fact]
        public void NullFilePathTest()
        {
            Assert.Throws<ArgumentException>(() => new SelectionCsvReader(null));
        }

        [Fact]
        public void FileNotFoundTest()
        {
            Assert.Throws<FileNotFoundException>(() => new SelectionCsvReader("TestFile/NotFound.csv"));
        }

        [Fact]
        public void NoLineTest()
        {
            var reader = new SelectionCsvReader("TestFile/empty.csv");
            int[] result = reader.ReadSelection(CsvType.Dashboard);

            Assert.Empty(result);
        }

        [Fact]
        public void NoTargetColumnTest()
        {
            var reader = new SelectionCsvReader("TestFile/ColumnsError.csv");
            int[] result = reader.ReadSelection(CsvType.Dashboard);

            Assert.Empty(result);
        }
    }
}
