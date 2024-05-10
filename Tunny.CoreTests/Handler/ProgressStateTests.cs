using System.Collections.Generic;

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
    }
}
