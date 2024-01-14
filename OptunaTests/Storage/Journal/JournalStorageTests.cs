using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Optuna.Storage.Journal.Tests
{
    [TestClass()]
    public class JournalStorageTests
    {
        [TestMethod()]
        public void MakeFileIfNotExistTest()
        {
            string path = @"TestFile/created.log";
            _ = new JournalStorage(path, true);
            Assert.IsTrue(File.Exists(path));
            File.Delete(path);
        }

        [TestMethod()]
        public void JournalStorageTest()
        {
            var storage = new JournalStorage(@"TestFile/journal.log");

            // check studies
            Study.Study[] studies = storage.GetAllStudies();
            Assert.AreEqual(3, studies.Length);

            // check single objective study BestTrial
            Trial.Trial bestTrial = studies[1].BestTrial;
            Assert.AreEqual(152, bestTrial.TrialId);
            Assert.AreEqual(0.0, bestTrial.Values[0]);
            Assert.AreEqual(52, bestTrial.Number);
            Assert.AreEqual(0.0, studies[1].BestValue);
            Assert.AreEqual(0.0, studies[1].BestParams["x"]);

            // check 2 objective study BestTrials
            Trial.Trial[] best2ObjsTrials = studies[0].BestTrials;
            Assert.AreEqual(2, best2ObjsTrials.Length);
            Assert.AreEqual(36, best2ObjsTrials[0].TrialId);
            Assert.AreEqual(104, best2ObjsTrials[0].Values[0]);
            Assert.AreEqual(16, best2ObjsTrials[0].Values[1]);

            // check 3 objective study BestTrial
            Trial.Trial[] best3ObjsTrials = studies[2].BestTrials;
            Assert.AreEqual(8, best3ObjsTrials.Length);

            // check study system attributes
            string[] metricName = studies[0].SystemAttrs["study:metric_names"] as string[];
            Assert.AreEqual("v0", metricName[0]);

            //check study user attributes
            string[] tunnyVersion = studies[0].UserAttrs["tunny_version"] as string[];
            Assert.AreEqual("0.9.1", tunnyVersion[0]);

            // check trials
            Trial.Trial[] trials = storage.GetAllTrials(0);
            Assert.AreEqual(100, trials.Length);

            // check trial 0
            Trial.Trial trial0 = trials[0];
            Assert.AreEqual(0, trial0.TrialId);
            Assert.AreEqual(1780, trial0.Values[0]);
            Assert.AreEqual(Trial.TrialState.COMPLETE, trial0.State);
            Assert.AreEqual(-11d, trial0.Params["x"]);
            string[] trialSystemAttr = trial0.SystemAttrs["constraints"] as string[];
            Assert.AreEqual("-780", trialSystemAttr[0]);
            string[] trialUserAttr = trial0.UserAttrs["Constraint"] as string[];
            Assert.AreEqual("75", trialUserAttr[1]);
        }
    }
}
