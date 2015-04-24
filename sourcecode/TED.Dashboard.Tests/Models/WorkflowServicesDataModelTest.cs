using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TED.Dashboard.Api.Models;
using TED.Dashboard.Workflow.Models;

namespace TED.Dashboard.Tests.Models
{
    [TestClass]
    public class WorkflowServicesDataModelTest
    {

        #region test data

        private readonly List<ServiceHostInfo> serviceHosts = new List<ServiceHostInfo>()
                {
                    new ServiceHostInfo()
                        {
                            Name = "TEST-HOST-1",
                            TimeStamp = DateTime.Now.ToString()
                        },
                    new ServiceHostInfo()
                        {
                            Name = "TEST-HOST-2",
                            TimeStamp = DateTime.Now.ToString()
                        },
                };

        // sample data to return
        private readonly List<WorkflowHostInfo> workflowHosts = new List<WorkflowHostInfo>()
                {
                    new WorkflowHostInfo()
                        {
                            Name = "TEST-HOST-1",
                            TimeStamp = DateTime.Now.ToString()
                        },
                    new WorkflowHostInfo()
                        {
                            Name = "TEST-HOST-2",
                            TimeStamp = DateTime.Now.ToString()
                        },
                        new WorkflowHostInfo()
                    {
                        Name = "TEST-HOST-2",
                        TimeStamp = DateTime.Now.ToString()
                    },
                };

        private readonly List<WorkflowConnector> connectors = new List<WorkflowConnector>()
                {
                    new WorkflowConnector()
                        {
                            HostName = "TEST-HOST-1",
                            Instances = 3,
                            IsEnabled = true,
                            Name = "Mock NEE"
                        },
                    new WorkflowConnector()
                        {
                            HostName = "TEST-HOST-1",
                            Instances = 1,
                            IsEnabled = true,
                            Name = "Mock Translation"
                        },
                    new WorkflowConnector()
                        {
                            HostName = "TEST-HOST-1",
                            Instances = 0,
                            IsEnabled = true,
                            Name = "Mock Export"
                        },
                    new WorkflowConnector()
                        {
                            HostName = "TEST-HOST-2",
                            Instances = 5,
                            IsEnabled = true,
                            Name = "Mock Text Extraction"
                        },
                    new WorkflowConnector()
                        {
                            HostName = "TEST-HOST-1",
                            Instances = 2,
                            IsEnabled = true,
                            Name = "Mock Text Extraction"
                        },
                    new WorkflowConnector()
                        {
                            HostName = "TEST-HOST-1",
                            Instances = 0,
                            IsEnabled = false,
                            Name = "Mock Disabled"
                        }
                };

        private readonly List<WorkflowProcessInfo> processes = new List<WorkflowProcessInfo>
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
        #endregion

        [TestMethod]
        public void ServiceHostCount_Test()
        {
            // sample data to return
            var testModel = new WorkflowServicesDataModel()
                {
                    ServiceHosts = serviceHosts
                };

            // Assess
            Assert.AreEqual(2, testModel.ServiceHostCount);
        }

        [TestMethod]
        public void WorkflowHostCount_Test()
        {
            var testModel = new WorkflowServicesDataModel()
            {
                WorkflowHosts = workflowHosts
            };

            // Assess
            Assert.AreEqual(3, testModel.WorkflowHostCount);
        }

        [TestMethod]
        public void ProcessInfoCount_Test()
        {
            var testModel = new WorkflowServicesDataModel()
            {
                ProcessInfo = processes
            };

            // Assess
            Assert.AreEqual(5, testModel.ProcessCount);
        }

        [TestMethod]
        public void CheckConnectors_Test()
        {
            var testModel = new WorkflowServicesDataModel()
            {
                WorkflowConnectors = connectors
            };

            // Assess
            Assert.AreEqual(5, testModel.ConnectorCount);
        }

        [TestMethod]
        public void CheckRunningConnectors_Test()
        {
            var testModel = new WorkflowServicesDataModel()
            {
                WorkflowConnectors = connectors
            };

            // Assess
            Assert.AreEqual(3, testModel.RunningConnectorCount);
        }
    }
}
