using System;
using System.Collections.Generic;
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
        public void LoadJournalWithInitTrialParamsTest()
        {
            string path = @"TestFile/journal2.log";
            var storage = new JournalStorage(path, false);
            Study.Study[] studies = storage.GetAllStudies();
            Assert.Equal("wInitValues", studies[3].StudyName);

            //Check wInitValues study
            List<Trial.Trial> trials = studies[3].Trials;
            Assert.Equal(0.834, trials[0].Params["x0"]);
            Assert.Equal(3.985, trials[0].Params["x1"]);
        }

        [Fact()]
        public void LoadNewJournalTest()
        {
            string path = @"TestFile/journal2.log";
            var storage = new JournalStorage(path, false);
            Study.Study[] studies = storage.GetAllStudies();

            Assert.Equal(4, studies.Length);
            Assert.Equal("MOwC", studies[0].StudyName);
            Assert.Equal("SO", studies[1].StudyName);
            Assert.Equal("cat", studies[2].StudyName);
            Assert.Equal("wInitValues", studies[3].StudyName);

            // Check MOwC study
            Trial.Trial[] bestTrials = studies[0].BestTrials;
            Assert.Equal(6, bestTrials[0].TrialId);
            Assert.Equal(52, bestTrials[0].Values[0]);
            Assert.Equal(13, bestTrials[0].Values[1]);
            Assert.Equal(2.0, bestTrials[0].Params["x"]);
            Assert.Equal(3.0, bestTrials[0].Params["y"]);
            string[] trialSystemAttr = bestTrials[0].SystemAttrs["constraints"] as string[];
            Assert.Equal(948d, double.Parse(trialSystemAttr[0]));
            string[] trialUserAttr = bestTrials[0].UserAttrs["Constraint"] as string[];
            Assert.Equal(487d, double.Parse(trialUserAttr[1]));

            // Check SO study
            Assert.Throws<ArgumentException>(() => studies[1].BestTrials);
            Trial.Trial bestTrial = studies[1].BestTrial;
            Assert.Equal(19, bestTrial.TrialId);
            Assert.Equal(5.7, bestTrial.Values[0], 2);
        }
    }
}
