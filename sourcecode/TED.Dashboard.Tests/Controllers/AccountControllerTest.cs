using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TED.Dashboard.Authentication;
using TED.Dashboard.Controllers;
using TED.Dashboard.Models;
using TestingHelpers;

namespace TED.Dashboard.Tests.Controllers
{
    [TestClass]
    public class AccountControllerTest
    {
        private const string testUserName = "TestUserName";
        private const string testFailPassword = "FailPassword";
        private const string testSuccessPassword = "SuccessPassword";

        /// <summary>
        /// Create an instance of a mock authenticator for controller
        /// </summary>
        private IAuthenticate mockAuthenticator;

        [TestInitialize()]
        public void Setup()
        {
            var authenticator = new Mock<IAuthenticate>();

            // add authentication success case
            authenticator.Setup(
                a => a.AuthenticateUser(testUserName, testSuccessPassword))
                .Returns(true);

            // add authentication fail case
            authenticator.Setup(
                a => a.AuthenticateUser(testUserName, testFailPassword))
                .Returns(false);

            authenticator.Setup(
                a => a.ApplicationAuthType).Returns(AuthType.PasswordAuthentication);

            mockAuthenticator = authenticator.Object;
        }

        [TestCleanup()]
        public void Cleanup()
        {
            mockAuthenticator = null;
        }

        [TestMethod]
        public void SuccessfulAuthenticationNoRedirect_Test()
        {
            // setup
            var viewModel = new LoginViewModel { Username = testUserName, Password = testSuccessPassword };
            var controller = new AccountController(mockAuthenticator);
            controller.SetFakeControllerContext("~/account/login");

            // act
            var result = controller.Login(viewModel, null);

            // assess
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));

            var routeResult = result as RedirectToRouteResult;
            if (routeResult == null) Assert.Fail("Returned route was null");

            Assert.AreEqual(routeResult.RouteValues["controller"], "Dashboard");
            Assert.AreEqual(routeResult.RouteValues["action"], "Index");
        }

        [TestMethod]
        public void SuccessfulAuthenticationOutsideRedirect_Test()
        {
            // setup
            var viewModel = new LoginViewModel { Username = testUserName, Password = testSuccessPassword };
            var controller = new AccountController(mockAuthenticator);
            controller.SetFakeControllerContext("~/account/login");

            // act
            var result = controller.Login(viewModel, "www.google.com");

            // assess
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));

            var routeResult = result as RedirectToRouteResult;
            if (routeResult == null) Assert.Fail("Returned route was null");

            Assert.AreEqual(routeResult.RouteValues["controller"], "Dashboard");
            Assert.AreEqual(routeResult.RouteValues["action"], "Index");
        }

        [TestMethod]
        public void SuccessfulAuthenticationWithRedirect_Test()
        {
            // setup
            var viewModel = new LoginViewModel {Username = testUserName, Password = testSuccessPassword};
            var controller = new AccountController(mockAuthenticator);
            controller.SetFakeControllerContext("~/account/login");

            // act
            var result = controller.Login(viewModel, "/Dashboard/Dataflow");

            // assess
            var routeResult = result as RedirectResult;
            if (routeResult == null) Assert.Fail("Returned route was null");

            Assert.AreEqual(routeResult.Url, "/Dashboard/Dataflow");
        }

        [TestMethod]
        public void FailedAuthentication_Test()
        {
            // setup
            var viewModel = new LoginViewModel { Username = testUserName, Password = testFailPassword };
            var controller = new AccountController(mockAuthenticator);
            controller.SetFakeControllerContext("~/account/login");

            // act
            var result = controller.Login(viewModel, "/");

            // assess
            Assert.IsInstanceOfType(result, typeof(ViewResult));

            var viewResult = result as ViewResult;

            Assert.IsTrue(viewResult != null && viewResult.ViewData.ModelState.Keys.Contains("LoginError"));
            Assert.AreEqual("", viewResult.ViewName);
        }

        [TestMethod]
        public void LogoutPassword_Test()
        {
            // setup
            var controller = new AccountController(mockAuthenticator);
            controller.SetFakeControllerContext("~/account/login");

            // act
            var result = controller.LogOff();

            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));

            var routeResult = result as RedirectToRouteResult;
            if (routeResult == null) return;

            Assert.AreEqual(routeResult.RouteValues["action"], "Login");
            Assert.AreEqual(routeResult.RouteValues["controller"], "Account");
        }

        [TestMethod]
        public void LogoutPki_Test()
        {
            // setup
            var authenticator = new Mock<IAuthenticate>();

            // add authentication success case
            authenticator.Setup(
                a => a.AuthenticateUser(testUserName, testSuccessPassword))
                .Returns(true);

            // add authentication fail case
            authenticator.Setup(
                a => a.AuthenticateUser(testUserName, testFailPassword))
                .Returns(false);

            authenticator.Setup(
                a => a.ApplicationAuthType).Returns(AuthType.PKIAuthentication);

            var pkiAuthenticator = authenticator.Object;
            
            var controller = new AccountController(pkiAuthenticator);
            controller.SetFakeControllerContext("~/account/login");

            // act
            var result = controller.LogOff();

            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));

            var routeResult = result as RedirectToRouteResult;
            if (routeResult == null) return;

            Assert.AreEqual(routeResult.RouteValues["action"], "LoggedOut");
            Assert.AreEqual(routeResult.RouteValues["controller"], "Account");
        }
    }
}
