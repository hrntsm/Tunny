using System.Collections.Generic;
using System.Linq.Expressions;

using Xunit;

namespace Tunny.Core.Handler.Tests
{
    public class ProgressStateTests
    {
        [Fact]
        public void ProgressStateCtorTest()
        {
            var progressState = new ProgressState();
            Assert.NotNull(progressState);
        }

        [Fact]
        public void ProgressStatePropertiesTest()
        {
            var param = new List<Input.Parameter>();
            double[][] bests = new double[1][];
            var progressState = new ProgressState
            {
                Parameter = param,
                TrialNumber = 0,
                ObjectiveNum = 0,
                BestValues = bests,
                HypervolumeRatio = 0,
                EstimatedTimeRemaining = new System.TimeSpan(),
                IsReportOnly = false
            };

            Assert.Equal(param, progressState.Parameter);
            Assert.Equal(0, progressState.TrialNumber);
            Assert.Equal(0, progressState.ObjectiveNum);
            Assert.Equal(bests, progressState.BestValues);
            Assert.Equal(0, progressState.HypervolumeRatio);
            Assert.Equal(new System.TimeSpan(), progressState.EstimatedTimeRemaining);
            Assert.False(progressState.IsReportOnly);
        }

        [Fact]
        public void ProgressStateConstructorTest()
        {
            var param = new List<Input.Parameter>();
            var noArg = new ProgressState();
            var oneArg = new ProgressState(param);
            var twoArg = new ProgressState(param, true);

            Assert.Equal(param, oneArg.Parameter);
            Assert.Equal(0, noArg.TrialNumber);
            Assert.Equal(0, noArg.ObjectiveNum);
            Assert.True(twoArg.IsReportOnly);
        }
    }
}
