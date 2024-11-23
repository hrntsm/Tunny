using System;
using System.Globalization;
using System.Linq;

namespace Tunny.Core.Input
{
    public static class InputValidator
    {
        public static bool IsPositiveInt(string input, bool includeZero)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return false;
            }

            bool isInt = int.TryParse(input, NumberStyles.Integer, CultureInfo.InvariantCulture, out int result);
            return isInt &&
                (includeZero ? result >= 0 : result > 0);
        }

        public static bool IsAutoOrPositiveInt(string input, bool includeZero)
        {
            if (IsPositiveInt(input, includeZero))
            {
                return true;
            }

            return input.Equals("auto", StringComparison.OrdinalIgnoreCase);
        }

        public static bool IsAutoOrInt(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return false;
            }

            if (input.Equals("auto", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            return int.TryParse(input, NumberStyles.Integer, CultureInfo.InvariantCulture, out _);
        }

        public static bool IsPositiveDouble(string input, bool includeZero)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return false;
            }

            bool isdouble = double.TryParse(input, NumberStyles.Integer, CultureInfo.InvariantCulture, out double result);
            return isdouble &&
                (includeZero ? result >= 0 : result > 0);
        }

        public static bool IsAutoOrPositiveDouble(string input, bool includeZero)
        {
            if (IsPositiveDouble(input, includeZero))
            {
                return true;
            }

            return input.Equals("auto", StringComparison.OrdinalIgnoreCase);
        }

        public static bool Is0to1(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return false;
            }

            bool isdouble = double.TryParse(input, NumberStyles.Integer, CultureInfo.InvariantCulture, out double result);
            return isdouble && result > 0 && result <= 1;
        }

        public static bool IsAutoOr0to1(string input)
        {
            if (Is0to1(input))
            {
                return true;
            }

            return input.Equals("auto", StringComparison.OrdinalIgnoreCase);
        }

        public static bool IsCommaSeparatedNumbers(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return false;
            }
            string[] numbers = input.Split(',');
            if (numbers.Any(string.IsNullOrWhiteSpace))
            {
                return false;
            }
            return numbers.All(n => double.TryParse(n.Trim(), out _));
        }
    }
}
