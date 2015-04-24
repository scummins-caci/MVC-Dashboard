using System.Configuration;
using HighView.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TED.Dashboard.Search.Repositories;

namespace TED.Dashboard.Repository.Tests.Repositories
{
    [TestClass, Ignore]
    public class SearchCountRepositoryTest
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
        public void SearchCountGetAll_Test()
        {
            var repository = new SearchCountRepository();

            // act
            var counts = repository.GetAll();

            // access
            Assert.IsNotNull(counts);
        }
    }
}
