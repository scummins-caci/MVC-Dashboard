using System.Configuration;
using System.Linq;
using HighView.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TED.Dashboard.Search.Repositories;

namespace TED.Dashboard.Repository.Tests.Repositories
{
    [TestClass, Ignore]
    public class LogRepositoryTest
    {

        [ClassInitialize]
        public static void MyClassInitialize(TestContext testContext)
        {
            WinModule.Initialize();
            WinModule.CreateNewSession(ConfigurationManager.AppSettings["AuthenticationConnectString"],
                                       ConfigurationManager.AppSettings["UserName"],
                                       ConfigurationManager.AppSettings["Password"]);
        }

        //
        // Use ClassCleanup to run code after all tests in a class have run
        [ClassCleanup]
        public static void MyClassCleanup()
        {
            WinModule.DestroySession();
        }


        [TestMethod]
        public void GetFirstPage_Test()
        {
            // setup
            var repository = new LogRepository();

            // act
            var items = repository.GetPage(1, 20);

            // access
            Assert.AreEqual(20, items.Count());
        }

        [TestMethod]
        public void GetNextPage_Test()
        {
            // setup
            var repository = new LogRepository();

            // act
            var items = repository.GetPage(2, 20);

            // access
            Assert.AreEqual(20, items.Count());
        }

        [TestMethod]
        public void GetLogCount_Test()
        {
            // setup
            var repository = new LogRepository();

            // act
            var count = repository.GetCount();

            // access
            Assert.IsTrue(count > 0);
        }
    }
}
