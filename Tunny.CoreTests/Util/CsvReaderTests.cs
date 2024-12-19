using System;
using System.Collections.Generic;
using System.IO;

using Xunit;

namespace Tunny.Core.Util.Tests
{
    public class CsvReaderTests
    {
        [Fact]
        public void DashboardCsvTest()
        {
            var reader = new CsvReader("TestFile/Dashboard.csv");
            int[] result = reader.ReadSelectionCsv(CsvType.Dashboard);

            Assert.Equal(4, result.Length);
            Assert.Equal(1, result[0]);
            Assert.Equal(8, result[1]);
            Assert.Equal(10, result[2]);
            Assert.Equal(11, result[3]);
        }

        [Fact]
        public void DesignExplorerCsvTest()
        {
            var reader = new CsvReader("TestFile/DesignExplorer.csv");
            int[] result = reader.ReadSelectionCsv(CsvType.DesignExplorer);

            Assert.Equal(4, result.Length);
            Assert.Equal(4, result[0]);
            Assert.Equal(5, result[1]);
            Assert.Equal(9, result[2]);
            Assert.Equal(16, result[3]);
        }

        [Fact]
        public void NullFilePathTest()
        {
            Assert.Throws<ArgumentException>(() => new CsvReader(null));
        }

        [Fact]
        public void FileNotFoundTest()
        {
            Assert.Throws<FileNotFoundException>(() => new CsvReader("TestFile/NotFound.csv"));
        }

        [Fact]
        public void NoLineTest()
        {
            var reader = new CsvReader("TestFile/empty.csv");
            int[] result = reader.ReadSelectionCsv(CsvType.Dashboard);

            Assert.Empty(result);
        }

        [Fact]
        public void NoTargetColumnTest()
        {
            var reader = new CsvReader("TestFile/ColumnsError.csv");
            int[] result = reader.ReadSelectionCsv(CsvType.Dashboard);

            Assert.Empty(result);
        }

        [Fact]
        public void ReadFishEggCsvTest()
        {
            var reader = new CsvReader("TestFile/EggTest1.csv");
            List<Dictionary<string, string>> result = reader.ReadFishEggCsv();

            Assert.Equal(4, result.Count);
            Assert.Equal("0.5", result[0]["x0"]);
            Assert.Equal("0.5", result[0]["x1"]);
            Assert.False(result[3].ContainsKey("x0"));
            Assert.Equal("0.5", result[0]["x1"]);
        }

        [Fact]
        public void ReadFishEggCsvErrorTest()
        {
            var reader = new CsvReader("TestFile/EggTest2.csv");
            Assert.Throws<InvalidDataException>(() => reader.ReadFishEggCsv());
        }
    }
}
