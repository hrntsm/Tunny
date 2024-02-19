using System.Globalization;

using Tunny.Core.Util;

namespace Tunny.Core.Input
{
    public class Parameter
    {
        public string Category { get; }
        public double Number { get; }
        public bool HasNumber => !HasCategory;
        public bool HasCategory => !string.IsNullOrEmpty(Category);

        public Parameter(double number)
        {
            TLog.MethodStart();
            Number = number;
        }

        public Parameter(string category)
        {
            TLog.MethodStart();
            Category = category;
        }

        public override string ToString()
        {
            TLog.MethodStart();
            if (HasCategory)
            {
                return Category;
            }
            else
            {
                return Number.ToString(CultureInfo.InvariantCulture);
            }
        }
    }
}
