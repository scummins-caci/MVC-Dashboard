using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestingHelpers;
using TED.Dashboard.Repository.Repositories;


namespace TED.Dashboard.Repository.Tests
{
    [TestClass, Ignore]
    public class HV5RepositoryTest
    {
        [ClassInitialize]
        public static void RepositoryTestInitialize(TestContext context)
        {
            HV5SessionHelper.CreateHV5Session();
        }


        [TestMethod]
        public void GetConnectors_Test()
        {
            // arrange
            var repository = new HV5Repository();

            // act
            var result = repository.GetConnectors();

            // assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any());
        }

        [TestMethod]
        public void GetWorkflowHosts_Test()
        {
            // arrange
            var repository = new HV5Repository();

            // act
            var result = repository.GetWorkflowHosts();

            // assert
            Assert.IsNotNull(result);
            //Assert.IsTrue(result.Any());
        }

        [TestMethod]
        public void GetServiceHosts_Test()
        {
            // arrange
            var repository = new HV5Repository();

            // act
            var result = repository.GetServiceHosts();

            // assert
            Assert.IsNotNull(result);
            //Assert.IsTrue(result.Any());
        }


        [TestMethod]
        public void GetConnectorLogs_Test()
        {
            // arrange
            var repository = new LogRepository();

            // act
            var result = repository.GetPage(1, 50);

            // assert
            Assert.IsTrue(result.Any());
        }

        [TestMethod]
        public void GetServiceHostLogs_Test()
        {
            // arrange
            var repository = new HV5Repository();

            // act
            var result = repository.GetServiceHostLogs(50);

            // assert
            Assert.IsTrue(result.Any());
        }

        [TestMethod]
        public void GetWorkflowHostLogs_Test()
        {
            // arrange
            var repository = new HV5Repository();

            // act
            var result = repository.GetWorkflowHostLogs(10);

            // assert
            Assert.IsTrue(result.Any());
        }


        [ClassCleanup]
        public static void RepositoryTestCleanup()
        {
            HV5SessionHelper.DestroyHV5Session();
        }
    }
}
