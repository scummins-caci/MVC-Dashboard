using System.Collections.Generic;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TED.Dashboard.Controllers;
using TED.Dashboard.Models;
using TED.Dashboard.User.Services;
using TED.Dashboard.Users.Models;

namespace TED.Dashboard.Tests.Controllers
{
    [TestClass]
    public class DashboardControllerTest
    {
        private IDashboardService service;
        private CustomDashboard testDashboard;

        [TestInitialize()]
        public void Setup()
        {
            testDashboard = new CustomDashboard()
            {
                DashboardTitle = "Custom Dashboard 1",
                WidgetControls = new List<string>()
                {
                    "_WorkflowLogsPartial",
                    "_WorkitemCountsPartial"
                }
            };

            var mockService = new Mock<IDashboardService>();
            mockService.Setup(x => x.GetDashboardById(It.IsAny<ulong>())).Returns(testDashboard);
            mockService.Setup(x => x.GetDashboardByName(It.IsAny<string>())).Returns(testDashboard);

            service = mockService.Object;
        }

        // Use TestCleanup to run code after each test has run
        [TestCleanup()]
        public void Cleanup()
        {
            service = null;
        }


        [TestMethod]
        public void DisplayCustomDashboard_Test()
        {
            // setup
            var controller = new DashboardController(service);
            
            // act
            var result = controller.Custom("Custom Dashboard 1");

            // assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = result as ViewResult;

            Assert.IsInstanceOfType(viewResult.Model, typeof(CustomDashboardViewModel));
            var viewModel = viewResult.Model as CustomDashboardViewModel;
            
            Assert.AreEqual(testDashboard.DashboardTitle, viewModel.DashboardTitle);
            Assert.AreEqual(testDashboard.WidgetControls, viewModel.WidgetControls);
        }
    }
}
