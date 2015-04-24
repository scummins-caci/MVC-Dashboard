using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TED.Dashboard.Repository;
using TED.Dashboard.Workflow.Models;
using TED.Dashboard.Workflow.Repositories;
using TED.Dashboard.Workflow.Services;

namespace TED.Dashboard.Services.Test.Services
{
    [TestClass]
    public class WorkflowStatusServiceTest
    {
        private IReadOnlyRepository<WorkflowConnector> connectorRepository;
        private IReadOnlyRepository<WorkflowHostInfo> workflowRepository;
        private IReadOnlyRepository<ServiceHostInfo> serviceRepository;
        private IProcessInfoRepository processRepository;


        [TestInitialize]
        public void Setup()
        {
            #region data setup

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

            var workflowHosts = new List<WorkflowHostInfo>
                {
                    new WorkflowHostInfo
                        {
                            Name = "TEST-HOST-2",
                            TimeStamp = DateTime.Now.ToString()
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
                            Instances = 1,
                            IsEnabled = true,
                            Name = "Log"
                        },
                    new WorkflowConnector
                        {
                            HostName = "TEST-HOST-1",
                            Instances = 1,
                            IsEnabled = true,
                            Name = "Finish"
                        },
                    new WorkflowConnector
                        {
                            HostName = "TEST-HOST-1",
                            Instances = 1,
                            IsEnabled = true,
                            Name = "Get Container Property"
                        },
                    new WorkflowConnector
                        {
                            HostName = "TEST-HOST-1",
                            Instances = 1,
                            IsEnabled = true,
                            Name = "Set Container Property"
                        },
                    new WorkflowConnector
                        {
                            HostName = "TEST-HOST-1",
                            Instances = 1,
                            IsEnabled = true,
                            Name = "Get Submitter"
                        },
                    new WorkflowConnector
                        {
                            HostName = "TEST-HOST-1",
                            Instances = 1,
                            IsEnabled = true,
                            Name = "MimeType"
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

            #endregion

            // mock all the repositories
            var mockConnectors = new Mock<IReadOnlyRepository<WorkflowConnector>>();
            mockConnectors.Setup(x => x.GetAll()).Returns(connectors);
            connectorRepository = mockConnectors.Object;

            var mockServiceHosts = new Mock<IReadOnlyRepository<ServiceHostInfo>>();
            mockServiceHosts.Setup(x => x.GetAll()).Returns(serviceHosts);
            serviceRepository = mockServiceHosts.Object;

            var mockWorkflowHosts = new Mock<IReadOnlyRepository<WorkflowHostInfo>>();
            mockWorkflowHosts.Setup(x => x.GetAll()).Returns(workflowHosts);
            workflowRepository = mockWorkflowHosts.Object;

            var mockProcess = new Mock<IProcessInfoRepository>();
            mockProcess.Setup(x => x.GetAll(null, null, false)).Returns(processes);
            processRepository = mockProcess.Object;

        }

        [TestCleanup]
        public void Cleanup()
        {
            serviceRepository = null;
            connectorRepository = null;
            workflowRepository = null;
        }

        [TestMethod]
        public void GetConnectors_Test()
        {
            var service = new WorkflowStatusService(connectorRepository, workflowRepository, serviceRepository, processRepository);

            // act
            var results = service.GetConnectors();

            // items should be filtered down based on the black list
            Assert.AreEqual(5, results.Count());
        }

        [TestMethod]
        public void GetWorkflowHost_Test()
        {
            var service = new WorkflowStatusService(connectorRepository, workflowRepository, serviceRepository, processRepository);

            // act
            var results = service.GetWorkflowHosts();

            // assess
            Assert.AreEqual(1, results.Count());
        }

        [TestMethod]
        public void GetServiceHost_Test()
        {
            var service = new WorkflowStatusService(connectorRepository, workflowRepository, serviceRepository, processRepository);

            // act
            var results = service.GetServiceHosts();

            // assess
            Assert.AreEqual(2, results.Count());
        }

        [TestMethod]
        public void GetProcessInfo_Test()
        {
            var service = new WorkflowStatusService(connectorRepository, workflowRepository, serviceRepository, processRepository);

            // act
            var results = service.GetProcessInfo();

            // assess
            Assert.AreEqual(1, results.Count());
        }
    }
}
