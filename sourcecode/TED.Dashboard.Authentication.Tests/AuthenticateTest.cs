using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TED.Dashboard.Authentication.HV5;
using TestingHelpers;
using Moq;
using TED.Dashboard.Users.Services;

namespace TED.Dashboard.Authentication.Tests
{
    [TestClass]
    public class AuthenticateTest
    {
        private IUserInfoService infoService;

        [TestInitialize]
        public void Setup()
        {
            var roles = new List<string>
                {
                    "WorkflowAdmin",
                    "DataFlowAdmin",
                    "DashboardManager"
                };

            var dashboards = new List<string>
                {
                    "CUSTOM-1",
                    "CUSTOM-2"
                };

            var mock = new Mock<IUserInfoService>();
            mock.Setup(x => x.GetUserDashboards(It.IsAny<uint>())).Returns(dashboards);
            mock.Setup(x => x.GetUserRoles(It.IsAny<uint>())).Returns(roles);

            infoService = mock.Object;
        }

        [TestCleanup]
        public void Cleanup()
        {
            infoService = null;
        }

        [TestMethod, Ignore]
        public void AuthenticateExistingUser()
        {
            // arrange
            var mockContext = MVCMockHelpers.FakeCurrentContext();
            IAuthenticate hv5Authenticator = new Authenticate(infoService, MVCMockHelpers.FakeHttpContext());

            // act
            var result = hv5Authenticator.AuthenticateUser(mockContext, "admin", "admin");

            // assert
            Assert.AreEqual(true, result);
        }
    }
}
