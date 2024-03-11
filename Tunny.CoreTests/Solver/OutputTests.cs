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

        [Fact()]
        public void GetParetoFrontTrialTest()
        {
            Trial[] trials = _output.GetTargetTrial(new int[] { -1 }, "output_test");
            Assert.Equal(2, trials.Length);
        }

        [Fact()]
        public void GetAllTargetTrialTest()
        {
            Trial[] trials = _output.GetTargetTrial(new int[] { -10 }, "output_test");
            Assert.Equal(10, trials.Length);
        }

        [Fact()]
        public void GetSpecificTargetTrialTest()
        {
            Trial[] trials = _output.GetTargetTrial(new int[] { 3, 2, 9 }, "output_test");
            Assert.Equal(3, trials.Length);
            Assert.Equal(3, trials[0].Number);
            Assert.Equal(2, trials[1].Number);
            Assert.Equal(9, trials[2].Number);
        }

        [Fact()]
        public void GetSpecificTargetTrialWithNullTest()
        {
            Trial[] trials = _output.GetTargetTrial(new int[] { 100 }, "output_test");
            Assert.Empty(trials);
        }

        [Fact()]
        public void NonExistentStudyTest()
        {
            Trial[] trials = _output.GetTargetTrial(new int[] { -1 }, "nonexistent_study");
            Assert.Empty(trials);
        }
    }
}
