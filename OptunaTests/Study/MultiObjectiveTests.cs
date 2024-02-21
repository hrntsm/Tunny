using System.Collections.Generic;

using Optuna.Trial;

using Xunit;

namespace Optuna.Study.Tests
{
    public class MultiObjectiveTests
    {
        [Fact]
        public void GetParetoFrontTrials2DTest()
        {
            var t1 = new Trial.Trial
            {
                TrialId = 1,
                Values = new double[] { 1, 2 },
                State = TrialState.COMPLETE
            };
            var t2 = new Trial.Trial
            {
                TrialId = 2,
                Values = new double[] { 2, 2 },
                State = TrialState.COMPLETE
            };
            var t3 = new Trial.Trial
            {
                TrialId = 3,
                Values = new double[] { 2, 3 },
                State = TrialState.COMPLETE
            };
            StudyDirection[] directions = new[] { StudyDirection.Minimize, StudyDirection.Maximize };

            Trial.Trial[] pareto = MultiObjective.GetParetoFrontTrials(new List<Trial.Trial> { t1, t2, t3 }, directions);
            Assert.Equal(2, pareto.Length);
            Assert.Equal(1, pareto[0].TrialId);
            Assert.Equal(3, pareto[1].TrialId);
        }

        [Fact]
        public void GetParetoFrontTrialsNDTest()
        {
            var t1 = new Trial.Trial
            {
                TrialId = 1,
                Values = new double[] { 1, 2, 0 },
                State = TrialState.COMPLETE
            };
            var t2 = new Trial.Trial
            {
                TrialId = 2,
                Values = new double[] { 2, 2, 0 },
                State = TrialState.COMPLETE
            };
            var t3 = new Trial.Trial
            {
                TrialId = 3,
                Values = new double[] { 2, 3, 0 },
                State = TrialState.COMPLETE
            };
            StudyDirection[] directions = new[] { StudyDirection.Minimize, StudyDirection.Maximize };

            Trial.Trial[] pareto = MultiObjective.GetParetoFrontTrials(new List<Trial.Trial> { t1, t2, t3 }, directions);
            Assert.Equal(2, pareto.Length);
            Assert.Equal(1, pareto[0].TrialId);
            Assert.Equal(3, pareto[1].TrialId);
        }
    }
}
