using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TED.Dashboard.Repository;
using TED.Dashboard.Workflow.Models;
using TED.Dashboard.Workflow.Repositories;
using TED.Dashboard.Workflow.Services;

namespace TED.Dashboard.Services.Tests.Services
{

    [TestClass]
    public class WorkitemServiceTest
    {
        private IReadOnlyRepository<WorkItemCount> workItemCountRepository;
        private IReadOnlyRepository<WorkItemStatusCount> statusCountRepository;
        private readonly IProcessInfoRepository processRepository;
        private readonly IQueueRepository queueRepository;
        private readonly IWorkItemRepository workitemRepository;
        private readonly IQueueUserRepository userRepository;

        [TestInitialize]
        public void Setup()
        {
            #region create test data

            var statusCounts = new List<WorkItemStatusCount>
                {
                    new WorkItemStatusCount
                        {
                            Count = 1200,
                            Status = "ready"
                        },
                    new WorkItemStatusCount
                        {
                            Count = 25,
                            Status = "failed"
                        }
                };

            // sample data to return
            var workitemCounts = new List<WorkItemCount>
                {
                    new WorkItemCount
                        {
                            Activity = "Sample Manual Activity-1",
                            Type = "Manual",
                            Count = 102
                        },
                    new WorkItemCount
                        {
                           Activity = "Sample Manual Activity-2",
                            Type = "Manual",
                            Count = 44
                        },
                    new WorkItemCount
                        {
                            Activity = "Sample Automated Activity-1",
                            Type = "Automated",
                            Count = 102
                        },
                    new WorkItemCount
                        {
                           Activity = "Sample Automated Activity-2",
                            Type = "Automated",
                            Count = 44
                        }
                };

            #endregion

            var wkItemCountMock = new Mock<IReadOnlyRepository<WorkItemCount>>();
            wkItemCountMock.Setup(x => x.GetAll()).Returns(workitemCounts);
            workItemCountRepository = wkItemCountMock.Object;

            var statusCountMock = new Mock<IReadOnlyRepository<WorkItemStatusCount>>();
            statusCountMock.Setup(x => x.GetAll()).Returns(statusCounts);
            statusCountRepository = statusCountMock.Object;
        }

        [TestCleanup]
        public void Cleanup()
        {
            workItemCountRepository = null;
            statusCountRepository = null;
        }

        [TestMethod]
        public void GetStatusCounts_Test()
        {
            var service = new WorkitemService(workItemCountRepository, statusCountRepository,
                                                processRepository, queueRepository, workitemRepository, userRepository);

            // act
            var results = service.GetStatusCounts();

            Assert.AreEqual(2, results.Count());
        }

        [TestMethod]
        public void GetQueueCounts_Test()
        {
            var service = new WorkitemService(workItemCountRepository, statusCountRepository,
                                                processRepository, queueRepository, workitemRepository, userRepository);

            // act
            var results = service.GetQueueCounts();

            Assert.AreEqual(4, results.Count());
        }

        [TestMethod]
        public void GetManualQueueCounts_Test()
        {
            var service = new WorkitemService(workItemCountRepository, statusCountRepository,
                                                processRepository, queueRepository, workitemRepository, userRepository);

            // act
            var results = service.GetManualQueueCounts();

            Assert.AreEqual(2, results.Count());
        }

        [TestMethod]
        public void GetAutomatedQueueCounts_Test()
        {
            var service = new WorkitemService(workItemCountRepository, statusCountRepository,
                                                processRepository, queueRepository, workitemRepository, userRepository);

            // act
            var results = service.GetAutomatedQueueCounts();

            Assert.AreEqual(2, results.Count());
        }
    }
}
