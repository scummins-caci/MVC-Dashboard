using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TED.Dashboard.Repository;
using TED.Dashboard.Search.Models;
using TED.Dashboard.Search.Services;

namespace TED.Dashboard.Services.Test.Services
{
    [TestClass]
    public class NotificationsServiceTest
    {
        private IPageableRepository<LogEntry> repository;
        
        [TestInitialize]
        public void Setup()
        {
            var logEntries = new List<LogEntry>
                {
                    new LogEntry
                        {
                            ClientMachine = "TEST-MACHINE-1",
                            LogId = "d852ec6d-954e-4683-84f3-52183f0d46f8",
                            Location = "HighView.Services.TEST-LOCATION1",
                            Message = "This is a test log entry",
                            Severity = "WARN",
                            TimeStamp = DateTime.Now.AddDays(-4)
                        },
                    new LogEntry
                        {
                            ClientMachine = "TEST-MACHINE-1",
                            LogId = "d852ec6d-954e-4683-84f3-52erwf0d46f8",
                            Location = "HighView.Services.TEST-LOCATION2",
                            Message = "This is a test log entry, it is an error",
                            Severity = "ERROR",
                            TimeStamp = DateTime.Now.AddMinutes(-45)
                        },
                    new LogEntry
                        {
                            ClientMachine = "TEST-MACHINE-1",
                            LogId = "d852ec6d-954e-4683-84f3-5dfd83f0d46f8",
                            Location = "HighView.WorkflowHost.TEST-LOCATION3",
                            Message = "This is a test log entry, it's severity isn't valid",
                            Severity = "NOT A TYPE",
                            TimeStamp = DateTime.Now.AddDays(-365)
                        },
                    new LogEntry
                        {
                            ClientMachine = "TEST-MACHINE-1",
                            LogId = "d852ec6d-954e-4683-84f3-52183f0d46f8",
                            Location = "HighView.ServiceHost.TEST-LOCATION4",
                            Message = "This is a test log entry",
                            Severity = "WARN",
                            TimeStamp = DateTime.Now.AddDays(-4)
                        },
                    new LogEntry
                        {
                            ClientMachine = "TEST-MACHINE-1",
                            LogId = "d852ec6d-954e-4683-84f3-52erwf0d46f8",
                            Location = "HighView.ServiceHost.TEST-LOCATION5",
                            Message = "This is a test log entry, it is an error",
                            Severity = "ERROR",
                            TimeStamp = DateTime.Now.AddMinutes(-45)
                        },
                    new LogEntry
                        {
                            ClientMachine = "TEST-MACHINE-1",
                            LogId = "d852ec6d-954e-4683-84f3-5dfd83f0d46f8",
                            Location = "HighView.ServiceHost.TEST-LOCATION6",
                            Message = "This is a test log entry, it's severity isn't valid",
                            Severity = "NOT A TYPE",
                            TimeStamp = DateTime.Now.AddDays(-365)
                        }
                };

            var mock = new Mock<IPageableRepository<LogEntry>>();
            mock.Setup(x => x.GetAll()).Returns(logEntries);
            mock.Setup(x => x.GetPage(It.IsAny<int>(), It.IsAny<int>())).Returns(logEntries);

            repository = mock.Object;
        }
        
        [TestCleanup]
        public void Cleanup()
        {
            repository = null;
        }

        
        [TestMethod]
        public void GetAllLogEntries_Test()
        {
            var service = new NotificationsService(repository);

            // act
            var result = service.GetAllLogEntries();

            Assert.AreEqual(6, result.Items.Count());
        }

        [TestMethod]
        public void GetPagedLogEntries_Test()
        {
            var service = new NotificationsService(repository);

            // act
            var result = service.GetPagedLogEntries(1, 10);
            Assert.AreEqual(6, result.Items.Count());
        }
    }
}
