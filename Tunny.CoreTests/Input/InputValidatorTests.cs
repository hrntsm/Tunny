using Xunit;

namespace Tunny.Core.Input
{
    public class InputValidatorTests
    {
        [Theory]
        [InlineData("1", true, true)]
        [InlineData("0", true, true)]
        [InlineData("0", false, false)]
        [InlineData("-1", true, false)]
        [InlineData("abc", true, false)]
        [InlineData("", true, false)]
        [InlineData(null, true, false)]
        [InlineData(" ", true, false)]
        public void IsPositiveInt_ValidatesCorrectly(string input, bool includeZero, bool expectedResult)
        {
            bool result = InputValidator.IsPositiveInt(input, includeZero);
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData("1", true, true)]
        [InlineData("0", true, true)]
        [InlineData("0", false, false)]
        [InlineData("auto", true, true)]
        [InlineData("AUTO", true, true)]
        [InlineData("Auto", false, true)]
        [InlineData("-1", true, false)]
        [InlineData("abc", true, false)]
        [InlineData("", true, false)]
        [InlineData(null, true, false)]
        public void IsAutoOrPositiveInt_ValidatesCorrectly(string input, bool includeZero, bool expectedResult)
        {
            bool result = InputValidator.IsAutoOrPositiveInt(input, includeZero);
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData("1", true)]
        [InlineData("0", true)]
        [InlineData("-1", true)]
        [InlineData("auto", true)]
        [InlineData("AUTO", true)]
        [InlineData("abc", false)]
        [InlineData("", false)]
        [InlineData(null, false)]
        public void IsAutoOrInt_ValidatesCorrectly(string input, bool expectedResult)
        {
            bool result = InputValidator.IsAutoOrInt(input);
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData("1.0", true, true)]
        [InlineData("0.0", true, true)]
        [InlineData("0.0", false, false)]
        [InlineData("-1.0", true, false)]
        [InlineData("abc", true, false)]
        [InlineData("", true, false)]
        [InlineData(null, true, false)]
        public void IsPositiveDouble_ValidatesCorrectly(string input, bool includeZero, bool expectedResult)
        {
            bool result = InputValidator.IsPositiveDouble(input, includeZero);
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData("1.0", true, true)]
        [InlineData("0.0", true, true)]
        [InlineData("0.0", false, false)]
        [InlineData("auto", true, true)]
        [InlineData("AUTO", true, true)]
        [InlineData("-1.0", true, false)]
        [InlineData("abc", true, false)]
        [InlineData("", true, false)]
        [InlineData(null, true, false)]
        public void IsAutoOrPositiveDouble_ValidatesCorrectly(string input, bool includeZero, bool expectedResult)
        {
            bool result = InputValidator.IsAutoOrPositiveDouble(input, includeZero);
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData("0.5", true)]
        [InlineData("1.0", true)]
        [InlineData("0.0", false)]
        [InlineData("1.1", false)]
        [InlineData("-0.5", false)]
        [InlineData("abc", false)]
        [InlineData("", false)]
        [InlineData(null, false)]
        public void Is0to1_ValidatesCorrectly(string input, bool expectedResult)
        {
            bool result = InputValidator.Is0to1(input);
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData("0.5", true)]
        [InlineData("1.0", true)]
        [InlineData("auto", true)]
        [InlineData("AUTO", true)]
        [InlineData("0.0", false)]
        [InlineData("1.1", false)]
        [InlineData("-0.5", false)]
        [InlineData("abc", false)]
        [InlineData("", false)]
        [InlineData(null, false)]
        public void IsAutoOr0to1_ValidatesCorrectly(string input, bool expectedResult)
        {
            bool result = InputValidator.IsAutoOr0to1(input);
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData("1,2,3", true)]
        [InlineData("1.0,2.0,3.0", true)]
        [InlineData("-1,0,1", true)]
        [InlineData("1, 2, 3", true)]
        [InlineData("", false)]
        [InlineData("1,,2", false)]
        [InlineData("abc,def", false)]
        [InlineData("1,abc,2", false)]
        [InlineData(null, false)]
        public void IsCommaSeparatedNumbers_ValidatesCorrectly(string input, bool expectedResult)
        {
            bool result = InputValidator.IsCommaSeparatedNumbers(input);
            Assert.Equal(expectedResult, result);
        }
    }
}
