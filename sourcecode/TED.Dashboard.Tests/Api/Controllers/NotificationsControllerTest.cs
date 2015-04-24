using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TED.Dashboard.Api.Models;
using TED.Dashboard.Api.Controllers;
using Moq;
using TED.Dashboard.Search.Models;
using TED.Dashboard.Search.Services;
using TED.Dashboard.Services.Models;

namespace TED.Dashboard.Tests.Api.Controllers
{
    [TestClass]
    public class NotificationsControllerTest
    {
        private INotificationsService service;

        // Use TestInitialize to run code before running each test 
        [TestInitialize]
        public void Setup()
        {
          
            var logEntries = new List<LogEntry>
                {
                    new LogEntry
                        {
                            ClientMachine = "TEST-MACHINE-1",
                            LogId = "d852ec6d-954e-4683-84f3-52183f0d46f8",
                            Location = "TEST-LOCATION",
                            Message = "This is a test log entry",
                            Severity = "WARN",
                            TimeStamp = DateTime.Now.AddDays(-4)
                        },
                    new LogEntry
                        {
                            ClientMachine = "TEST-MACHINE-1",
                            LogId = "d852ec6d-954e-4683-84f3-52erwf0d46f8",
                            Location = "TEST-LOCATION",
                            Message = "This is a test log entry, it is an error",
                            Severity = "ERROR",
                            TimeStamp = DateTime.Now.AddMinutes(-45)
                        },
                    new LogEntry
                        {
                            ClientMachine = "TEST-MACHINE-1",
                            LogId = "d852ec6d-954e-4683-84f3-5dfd83f0d46f8",
                            Location = "TEST-LOCATION",
                            Message = "This is a test log entry, it's severity isn't valid",
                            Severity = "NOT A TYPE",
                            TimeStamp = DateTime.Now.AddDays(-365)
                        }
                };

            var data = new PageableItem<LogEntry>
                {
                    ItemCount = logEntries.Count(),
                    Items = logEntries
                };

            var mock = new Mock<INotificationsService>();
            mock.Setup(x => x.GetPagedLogEntries(It.IsAny<int>(), It.IsAny<int>())).Returns(data);
            mock.Setup(x => x.GetAllLogEntries()).Returns(data);

            service = mock.Object;

        }

        // Use TestCleanup to run code after each test has run
        [TestCleanup]
        public void Cleanup()
        {
            service = null;
        }


        [TestMethod]
        public void GetAllNotifications_Test()
        {
            var controller = new NotificationsController(service);

            // act
            var results = controller.Get();

            // assess
            Assert.AreEqual(200, results.Status);
            Assert.AreEqual(true, results.Success); 

            // test to make sure correct object type is returned
            Assert.IsInstanceOfType(results.Data, typeof(PageableItemDataModel<LogEntryDataModel>));
            var data = results.Data as PageableItemDataModel<LogEntryDataModel>;

            Assert.AreEqual(3, data.Items.Count());
            Assert.AreEqual(3, data.ItemCount);
        }
    }
}
