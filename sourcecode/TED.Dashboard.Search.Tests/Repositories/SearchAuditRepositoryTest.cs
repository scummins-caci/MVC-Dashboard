using System.Configuration;
using HighView.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TED.Dashboard.Search.Repositories;

namespace TED.Dashboard.Repository.Tests.Repositories
{
    [TestClass, Ignore]
    public class SearchAuditRepositoryTest
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
        public void SearchAuditGet100_Test()
        {
            var repository = new SearchAuditRepository();

            // act
            var counts = repository.GetPage(1, 100);

            // access
            Assert.IsNotNull(counts);
        }

        [TestMethod]
        public void SearchAuditGet200_Test()
        {
            var repository = new SearchAuditRepository();

            // act
            var counts = repository.GetPage(1, 200);

            // access
            Assert.IsNotNull(counts);
        }

        [TestMethod]
        public void SearchAuditGet500_Test()
        {
            var repository = new SearchAuditRepository();

            // act
            var counts = repository.GetPage(1, 500);

            // access
            Assert.IsNotNull(counts);
        }

        [TestMethod]
        public void SearchAuditGet1000_Test()
        {
            var repository = new SearchAuditRepository();

            // act
            var counts = repository.GetPage(1, 1000);

            // access
            Assert.IsNotNull(counts);
        }

        [TestMethod]
        public void SearchAuditGet5000_Test()
        {
            var repository = new SearchAuditRepository();

            // act
            var counts = repository.GetPage(1, 5000);

            // access
            Assert.IsNotNull(counts);
        }
    }
}
