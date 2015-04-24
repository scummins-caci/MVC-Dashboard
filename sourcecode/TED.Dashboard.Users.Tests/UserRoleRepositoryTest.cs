using System.Configuration;
using HighView.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TED.Dashboard.Users.Repositories;

namespace TED.Dashboard.Repository.Tests.Repositories
{
    [TestClass, Ignore]
    public class UserRoleRepositoryTest
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
        public void GetUserRoles_Test()
        {
            var repository = new UserRoleRepository();

            // act
            var items = repository.GetAll(254);

            // access
            Assert.IsNotNull(items);
        }
    }
}
