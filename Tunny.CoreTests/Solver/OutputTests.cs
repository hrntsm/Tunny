using Optuna.Trial;

using Xunit;

namespace Tunny.Core.Solver.Tests
{
    public class OutputTests
    {
        private readonly Output _output;

        public OutputTests()
        {
            _output = new Output("TestFile/journal.log");
        }

        [Fact]
        public void GetParetoFrontTrialTest()
        {
            Trial[] trials = _output.GetTargetTrial(new[] { -1 }, "output_test");
            Assert.Equal(2, trials.Length);
        }

        [Fact]
        public void GetAllTargetTrialTest()
        {
            Trial[] trials = _output.GetTargetTrial(new[] { -10 }, "output_test");
            Assert.Equal(10, trials.Length);
        }

        [Fact]
        public void GetMetricNamesTest()
        {
            string[] metricNames = _output.GetMetricNames("output_test");
            Assert.Equal(2, metricNames.Length);
            Assert.Equal("v0", metricNames[0]);
            Assert.Equal("v1", metricNames[1]);
        }

        [Fact]
        public void NullMetricNamesTest()
        {
            string[] metricNames = _output.GetMetricNames("null_test");
            Assert.Empty(metricNames);
        }

        [Fact]
        public void GetSpecificTargetTrialWithNullTest()
        {
            Trial[] trials = _output.GetTargetTrial(new[] { 100 }, "output_test");
            Assert.Empty(trials);
        }

        [Fact]
        public void NonExistentStudyTest()
        {
            Trial[] trials = _output.GetTargetTrial(new[] { -1 }, "nonexistent_study");
            Assert.Empty(trials);
        }
    }
}
