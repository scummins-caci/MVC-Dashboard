using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TED.Dashboard.Api.Controllers;
using TED.Dashboard.Workflow.Services;
using TED.Dashboard.Workflow.Models;
using Moq;

namespace TED.Dashboard.Tests.Api.Controllers
{
    [TestClass]
    public class WorkflowControllerTest
    {
        private IWorkitemService inbasketService;

        // Use TestInitialize to run code before running each test 
        [TestInitialize]
        public void Setup()
        {
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
            var manualWorkItemCounts = new List<WorkItemCount>
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
                };

            var automatedWorkItemCounts = new List<WorkItemCount>
                {
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
                        },
                };

            var allItems = new List<WorkItemCount>();
            allItems.AddRange(automatedWorkItemCounts);
            allItems.AddRange(manualWorkItemCounts);


            var mock = new Mock<IWorkitemService>();
            mock.Setup(x => x.GetQueueCounts()).Returns(allItems);
            mock.Setup(x => x.GetManualQueueCounts()).Returns(manualWorkItemCounts);
            mock.Setup(x => x.GetAutomatedQueueCounts()).Returns(automatedWorkItemCounts);
            mock.Setup(x => x.GetStatusCounts()).Returns(statusCounts);

            inbasketService = mock.Object;
        }

        // Use TestCleanup to run code after each test has run
        [TestCleanup]
        public void Cleanup()
        {
            inbasketService = null;
        }

        [TestMethod]
        public void GetCounts()
        {
            var controller = new WorkflowController(inbasketService);
            var results = controller.QueueCounts();

            // assess
            Assert.AreEqual(200, results.Status);
            Assert.AreEqual(true, results.Success);

            // test to make sure correct object type is returned
            Assert.IsInstanceOfType(results.Data, typeof(IEnumerable<WorkItemCount>));
            var data = results.Data as IEnumerable<WorkItemCount>;

            Assert.AreEqual(4, data.Count());
        }

        [TestMethod]
        public void GetManualCounts_Test()
        {
            var controller = new WorkflowController(inbasketService);
            var results = controller.ManualQueueCounts();

            // assess
            Assert.AreEqual(200, results.Status);
            Assert.AreEqual(true, results.Success);

            // test to make sure correct object type is returned
            Assert.IsInstanceOfType(results.Data, typeof(IEnumerable<WorkItemCount>));
            var data = results.Data as IEnumerable<WorkItemCount>;

            Assert.AreEqual(2, data.Count());
        }

        [TestMethod]
        public void GetAutomatedCounts_Test()
        {
            var controller = new WorkflowController(inbasketService);
            var results = controller.AutomatedQueueCounts();

            // assess
            Assert.AreEqual(200, results.Status);
            Assert.AreEqual(true, results.Success);

            // test to make sure correct object type is returned
            Assert.IsInstanceOfType(results.Data, typeof(IEnumerable<WorkItemCount>));
            var data = results.Data as IEnumerable<WorkItemCount>;

            Assert.AreEqual(2, data.Count());
        }

        [TestMethod]
        public void GetStatusCounts_Test()
        {
            var controller = new WorkflowController(inbasketService);
            var results = controller.StatusCounts();

            // assess
            Assert.AreEqual(200, results.Status);
            Assert.AreEqual(true, results.Success);

            // test to make sure correct object type is returned
            Assert.IsInstanceOfType(results.Data, typeof(IEnumerable<WorkItemStatusCount>));
            var data = results.Data as IEnumerable<WorkItemStatusCount>;

            Assert.AreEqual(2, data.Count());
        }
    }
}
