using System.Web.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TED.Dashboard.Controllers;
using TED.Dashboard.Models;
using TED.Dashboard.Routes;
using MvcRouteTester;
using Moq;

namespace TED.Dashboard.Tests.Routing
{
    [TestClass]
    public class MVCRouteTest
    {
        private RouteCollection routes;

        // Use TestInitialize to run code before running each test 
        [TestInitialize]
        public void Setup()
        {
            routes = MvcRoutes.Routes;
        }

        #region Account controller paths

        [TestMethod]
        public void AccountLoginGet_Test()
        {
            routes.ShouldMap("/account/login")
                    .To<AccountController>(x => x.Login(It.IsAny<string>()));
        }

        [TestMethod]
        public void AccountLoginPost_Test()
        {
            var vm = new LoginViewModel()
                {
                    Password = "test",
                    Username = "test"
                };

            routes.ShouldMap("/account/login")
                  .WithFormUrlBody("Username=test&Password=test")
                  .To<AccountController>(x => x.Login(vm, It.IsAny<string>()));
        }

        [TestMethod]
        public void AccountLogoutPost_Test()
        {
            routes.ShouldMap("/account/logoff")
                  .To<AccountController>(x => x.LogOff());
        }

        #endregion

        #region Dashboard controller paths

        [TestMethod]
        public void DashboardDefaultGet_Test()
        {
            routes.ShouldMap("/")
                    .To<DashboardController>(x => x.Index());
        }

        [TestMethod]
        public void DashboardInbasketGet_Test()
        {
            routes.ShouldMap("/dashboard/inbasket")
                    .To<DashboardController>(x => x.Inbasket());
        }

        [TestMethod]
        public void DashboardDataflowGet_Test()
        {
            routes.ShouldMap("/dashboard/dataflow")
                    .To<DashboardController>(x => x.Dataflow());
        }

        [TestMethod]
        public void DashboardSearchGet_Test()
        {
            routes.ShouldMap("/dashboard/search")
                    .To<DashboardController>(x => x.Search());
        }

        [TestMethod]
        public void DashboardCustomGet_Test()
        {
            routes.ShouldMap("/dashboard/custom/test1")
                    .To<DashboardController>(x => x.Custom("test1"));
        }

        [TestMethod]
        public void DashboardCustomUrlBuild_Test()
        {
            RouteAssert.GeneratesActionUrl(routes, "/dashboard/custom/test1", 
                new { action = "Custom", controller = "Dashboard", id = "test1"});
        }

        [TestMethod]
        public void DashboardManagement_Test()
        {
            routes.ShouldMap("/dashboard/manage")
                    .To<DashboardController>(x => x.Manage());
        }

        [TestMethod]
        public void IgnoringAxdFileServing_Test()
        {
            routes.ShouldMap("example.axd").ToIgnoredRoute();
        }

        #endregion

        [TestMethod]
        public void ErrorAccessDeniedGet_Test()
        {
            routes.ShouldMap("/error/accessdenied")
                    .To<ErrorController>(x => x.AccessDenied());
        }

    }
}
