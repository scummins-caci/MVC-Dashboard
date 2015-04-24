using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TED.Dashboard.Api.Controllers;
using TED.Dashboard.Api.Models;

using TED.Dashboard.Workflow.Services;
using TED.Dashboard.Workflow.Models;
using Moq;

namespace TED.Dashboard.Tests.Api.Controllers
{
    [TestClass]
    public class WorkflowStatusControllerTest
    {
        private IWorkflowStatusService service;

        // Use TestInitialize to run code before running each test 
        [TestInitialize]
        public void Setup()
        {
            // sample data to return
            var serviceHosts = new List<ServiceHostInfo>
                {
                    new ServiceHostInfo
                        {
                            Name = "TEST-HOST-1",
                            TimeStamp = DateTime.Now.ToString()
                        },
                    new ServiceHostInfo
                        {
                            Name = "TEST-HOST-2",
                            TimeStamp = DateTime.Now.ToString()
                        },
                };

            var workflowHosts = new List<WorkflowHostInfo>();

            var connectors = new List<WorkflowConnector>
                {
                    new WorkflowConnector
                        {
                            HostName = "TEST-HOST-1",
                            Instances = 3,
                            IsEnabled = true,
                            Name = "Mock NEE"
                        },
                    new WorkflowConnector
                        {
                            HostName = "TEST-HOST-1",
                            Instances = 1,
                            IsEnabled = true,
                            Name = "Mock Translation"
                        },
                    new WorkflowConnector
                        {
                            HostName = "TEST-HOST-1",
                            Instances = 0,
                            IsEnabled = true,
                            Name = "Mock Export"
                        },
                    new WorkflowConnector
                        {
                            HostName = "TEST-HOST-2",
                            Instances = 5,
                            IsEnabled = true,
                            Name = "Mock Text Extraction"
                        },
                    new WorkflowConnector
                        {
                            HostName = "TEST-HOST-1",
                            Instances = 0,
                            IsEnabled = false,
                            Name = "Mock Disabled"
                        }
                };

            var processes = new List<WorkflowProcessInfo>
                {
                    new WorkflowProcessInfo
                    {
                        ID = 1,
                        Name = "Content Processing",
                        IsEnabled = true
                    },
                    new WorkflowProcessInfo
                    {
                        ID = 1,
                        Name = "Validation Process",
                        IsEnabled = true
                    },
                    new WorkflowProcessInfo
                    {
                        ID = 1,
                        Name = "Human Translation",
                        IsEnabled = true
                    },
                    new WorkflowProcessInfo
                    {
                        ID = 1,
                        Name = "Document Screning",
                        IsEnabled = true
                    },
                    new WorkflowProcessInfo
                    {
                        ID = 1,
                        Name = "Media Conversion Process",
                        IsEnabled = true
                    }
                };

            var mock = new Mock<IWorkflowStatusService>();
            mock.Setup(x => x.GetConnectors()).Returns(connectors);
            mock.Setup(x => x.GetServiceHosts()).Returns(serviceHosts);
            mock.Setup(x => x.GetWorkflowHosts()).Returns(workflowHosts);
            mock.Setup(x => x.GetProcessInfo()).Returns(processes);

            service = mock.Object;
        }

        // Use TestCleanup to run code after each test has run
        [TestCleanup]
        public void Cleanup()
        {
            service = null;
        }

        [TestMethod]
        public void GetWorkflowStatusInfo_Test()
        {
            var controller = new WorkflowStatusController(service);
            var results = controller.Get();

            // assess
            Assert.AreEqual(200, results.Status);
            Assert.AreEqual(true, results.Success);

            // test to make sure correct object type is returned
            Assert.IsInstanceOfType(results.Data, typeof(WorkflowServicesDataModel));
            var data = results.Data as WorkflowServicesDataModel;


            Assert.AreEqual(5, data.ConnectorCount);
            Assert.AreEqual(3, data.RunningConnectorCount);
            Assert.AreEqual(2, data.ServiceHostCount);
            Assert.AreEqual(0, data.WorkflowHostCount);
            Assert.AreEqual(5, data.ProcessCount);
        }

        [TestMethod]
        public void GetWorkflowServiceStatusInfo_Test()
        {
            var controller = new WorkflowStatusController(service);
            var results = controller.Services();

            // assess
            Assert.AreEqual(200, results.Status);
            Assert.AreEqual(true, results.Success);

            // test to make sure correct object type is returned
            Assert.IsInstanceOfType(results.Data, typeof(IEnumerable<WorkflowConnector>));
            var data = results.Data as IEnumerable<WorkflowConnector>;

            Assert.AreEqual(5, data.Count());
        }

        [TestMethod]
        public void GetWorkflowHostStatusInfo_Test()
        {
            var controller = new WorkflowStatusController(service);
            var results = controller.WorkflowHosts();

            // assess
            Assert.AreEqual(200, results.Status);
            Assert.AreEqual(true, results.Success);

            // test to make sure correct object type is returned
            Assert.IsInstanceOfType(results.Data, typeof(IEnumerable<HostInfo>));
            var data = results.Data as IEnumerable<HostInfo>;

            Assert.AreEqual(0, data.Count());
        }

        [TestMethod]
        public void GetServiceHostStatusInfo_Test()
        {
            var controller = new WorkflowStatusController(service);
            var results = controller.ServiceHosts();

            // assess
            Assert.AreEqual(200, results.Status);
            Assert.AreEqual(true, results.Success);

            // test to make sure correct object type is returned
            Assert.IsInstanceOfType(results.Data, typeof(IEnumerable<HostInfo>));
            var data = results.Data as IEnumerable<HostInfo>;

            Assert.AreEqual(2, data.Count());
        }

        [TestMethod]
        public void GetProcessInfo_Test()
        {
            var controller = new WorkflowStatusController(service);
            var results = controller.ProcessInfo();

            // assess
            Assert.AreEqual(200, results.Status);
            Assert.AreEqual(true, results.Success);

            // test to make sure correct object type is returned
            Assert.IsInstanceOfType(results.Data, typeof(IEnumerable<WorkflowProcessInfo>));
            var data = results.Data as IEnumerable<WorkflowProcessInfo>;

            Assert.AreEqual(5, data.Count());
        }
    }
}
