using System.Configuration;
using HighView.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TED.Dashboard.Workflow.Repositories;

namespace TED.Dashboard.Workflow.Tests.Inbasket
{
    [TestClass, Ignore]
    public class QueueUserRepositoryTest
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
        public void GetQueueUser_Test()
        {
            var repository = new QueueUserRepository();

            var queues = repository.GetQueueUsers(1);

            Assert.IsNotNull(queues);
        }
    }
}
