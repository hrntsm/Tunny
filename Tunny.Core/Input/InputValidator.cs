using System;
using System.Globalization;

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

            if (input.Equals("auto", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            return false;
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
    }
}
