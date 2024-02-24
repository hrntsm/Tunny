using System.IO;

using Xunit;

namespace Optuna.Storage.Journal.Tests
{
    public class JournalStorageTests
    {
        [Fact()]
        public void MakeFileIfNotExistTest()
        {
            string path = @"TestFile/created.log";
            _ = new JournalStorage(path, true);
            Assert.True(File.Exists(path));
            File.Delete(path);
        }

        [Fact()]
        public void LoadNewJournalTest()
        {
            string path = @"TestFile/journal2.log";
            var storage = new JournalStorage(path, false);
            Study.Study[] studies = storage.GetAllStudies();

            Assert.Equal(3, studies.Length);
            Assert.Equal("MOwC", studies[0].StudyName);
            Assert.Equal("SO", studies[1].StudyName);
            Assert.Equal("cat", studies[2].StudyName);

            Trial.Trial[] bestTrial = studies[0].BestTrials;
            Assert.Equal(6, bestTrial[0].TrialId);
            Assert.Equal(52, bestTrial[0].Values[0]);
            Assert.Equal(13, bestTrial[0].Values[1]);
            Assert.Equal(2.0, bestTrial[0].Params["x"]);
            Assert.Equal(3.0, bestTrial[0].Params["y"]);
            string[] trialSystemAttr = bestTrial[0].SystemAttrs["constraints"] as string[];
            Assert.Equal(948d, double.Parse(trialSystemAttr[0]));
            string[] trialUserAttr = bestTrial[0].UserAttrs["Constraint"] as string[];
            Assert.Equal(487d, double.Parse(trialUserAttr[1]));
        }
    }
}
