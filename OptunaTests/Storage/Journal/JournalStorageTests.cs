using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Optuna.Storage.Journal.Tests
{
    [TestClass()]
    public class JournalStorageTests
    {
        [TestMethod()]
        public void JournalStorageTest()
        {
            var aa = new JournalStorage(@"C:\Users\hiroa\Documents\Repo\Tunny\OptunaTests\File\fish.log");
            Assert.Fail();
        }
    }
}
