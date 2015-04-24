using System.Configuration;
using HighView.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TED.Dashboard.Search.Repositories;

namespace TED.Dashboard.Search.Tests.Repositories
{
    [TestClass, Ignore]
    public class FilterCountRepositoryTest
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
            var repository = new FilterCountRepository();

            var items = repository.GetAll();

            Assert.IsNotNull(items);

        }
    }
}
