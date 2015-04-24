using System.Configuration;
using HighView.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TED.Dashboard.Workflow.Repositories;

namespace TED.Dashboard.Workflow.Tests
{
    [TestClass, Ignore]
    public class QueueRepositoryTest
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
        public void GetAll_Test()
        {
            var repository = new QueueRepository();

            var queues = repository.GetAll(201);

            Assert.IsNotNull(queues);

        }

        [TestMethod]
        public void GetFilterByQueue_Test()
        {
            var repository = new QueueRepository();

            var queues = repository.GetAll(201);

            Assert.IsNotNull(queues);            
        }
    }
}
