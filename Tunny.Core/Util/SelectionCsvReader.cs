using System;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Tunny.Core.Util
{
    public class SelectionCsvReader
    {
        private readonly string _filePath;

        public SelectionCsvReader(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(filePath));
            }

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("File not found", filePath);
            }

            _filePath = filePath;
        }

        public int[] ReadSelection(CsvType csvType)
        {
            string key = csvType == CsvType.Dashboard
                ? key = "Number"
                : key = "trial_id";

            var lines = File.ReadLines(_filePath).ToList();
            if (lines.Count == 0)
            {
                return Array.Empty<int>();
            }

            string[] headers = lines[0].Split(',');
            int numberColumnIndex = Array.FindIndex(headers, h => h.Trim()
                .Equals(key, StringComparison.OrdinalIgnoreCase));

            if (numberColumnIndex == -1)
            {
                return Array.Empty<int>();
            }

            var numbers = lines.Skip(1)
                .Select(line =>
                {
                    string[] columns = line.Split(',');
                    return int.Parse(columns[numberColumnIndex], CultureInfo.InvariantCulture);
                })
                .ToList();

            return numbers.ToArray();
        }
    }

    public enum CsvType
    {
        Dashboard,
        DesignExplorer
    }
}
