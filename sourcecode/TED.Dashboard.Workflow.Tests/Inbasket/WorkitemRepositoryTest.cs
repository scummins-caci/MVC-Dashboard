using System.Configuration;
using HighView.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TED.Dashboard.Workflow.Repositories;

namespace TED.Dashboard.Workflow.Tests
{
    [TestClass]
    public class WorkitemRepositoryTest
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
        public void WorkitemGetPageQueueIdOnly_Test()
        {
            var repository = new WorkItemRepository();
            var items = repository.GetPage(38, 1, 25);

            Assert.IsNotNull(items);
        }

        [TestMethod]
        public void WorkitemGetPageQueueAndProcessId_Test()
        {
            var repository = new WorkItemRepository();
            var items = repository.GetPage(202, 210, 1, 25);

            Assert.IsNotNull(items);
        }

        [TestMethod]
        public void WorkitemGetPageQueueAndProcessId2_Test()
        {
            var repository = new WorkItemRepository();
            var items = repository.GetPage(41, 1, 1, 25);

            Assert.IsNotNull(items);
        }

        [TestMethod]
        public void WorkitemGetPageQueueIdWithFilter_Test()
        {
            var repository = new WorkItemRepository();
            var items = repository.GetPage(44, 1, 25);

            Assert.IsNotNull(items);            
        }

        [TestMethod]
        public void WorkitemGetCountQueueIdOnly_Test()
        {
            var repository = new WorkItemRepository();
            var count = repository.GetCount(38);

            Assert.IsTrue(count > 0);
        }

        [TestMethod]
        public void WorkitemGetCountQueueAndProcessId_Test()
        {
            var repository = new WorkItemRepository();
            var count = repository.GetCount(202, 210);

            Assert.IsTrue(count > 0);
        }
    }
}
