using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TED.Dashboard.Authentication.Models;
using TestingHelpers;

namespace TED.Dashboard.Authentication.Tests
{
    [TestClass]
    public class UserAuthenticationHelperTest
    {
        private HttpContextBase context;
        private UserInformation testUser;

        [TestInitialize]
        public void Setup()
        {
            context = MVCMockHelpers.FakeHttpContext();

            testUser = new UserInformation
            {
                UserId = 12345,
                UserName = "TEST-USER-1",
                Dashboards = new List<string>
                        {
                            "CUSTOM-1",
                            "CUSTOM-2"
                        },
                Roles = new List<string>
                        {
                            "WorkflowAdmin",
                            "DataFlowAdmin",
                            "DashboardManager"
                        }
            };
        }

        [TestCleanup]
        public void Cleanup()
        {
            context = null;
        }

        [TestMethod]
        public void CreateAndGetUserSessionTicket_Test()
        {
            // setup
            var authHelper = new UserAuthenticationHelper(MVCMockHelpers.FakeHttpContext());
            
            // act
            authHelper.CreateUserSessionTicket(testUser);
            var returnedUserInfo = authHelper.GetUserInformationFromTicket();

            // assert
            Assert.AreEqual(testUser.UserName, returnedUserInfo.UserName);
            Assert.AreEqual(testUser.Dashboards.Count(), returnedUserInfo.Dashboards.Count());
            Assert.AreEqual(testUser.Roles.Count(), returnedUserInfo.Roles.Count());

        }

        [TestMethod]
        public void DestroyUserSessionTicket_Test()
        {
            // setup
            var authHelper = new UserAuthenticationHelper(context);
            authHelper.CreateUserSessionTicket(testUser);

            // act
            authHelper.DestroyUserSessionTicket();

            // assert
            var returnedInfo = authHelper.GetUserInformationFromTicket();
            Assert.IsNull(returnedInfo);
        }
    }
}
