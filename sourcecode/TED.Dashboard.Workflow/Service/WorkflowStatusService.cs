using System.Collections.Generic;
using System.Linq;
using TED.Dashboard.Repository;
using TED.Dashboard.Workflow.Repositories;
using TED.Dashboard.Workflow.Models;

namespace TED.Dashboard.Workflow.Services
{
    public class WorkflowStatusService : IWorkflowStatusService
    {
        private readonly IReadOnlyRepository<WorkflowConnector> connectorRepository;
        private readonly IReadOnlyRepository<WorkflowHostInfo> workflowRepository;
        private readonly IReadOnlyRepository<ServiceHostInfo> serviceRepository;
        private readonly IProcessInfoRepository processRepository;

        public WorkflowStatusService(IReadOnlyRepository<WorkflowConnector> connectorRepository,
                                    IReadOnlyRepository<WorkflowHostInfo> workflowRepository,
                                    IReadOnlyRepository<ServiceHostInfo> serviceRepository,
                                    IProcessInfoRepository processRepository)
        {
            this.connectorRepository = connectorRepository;
            this.workflowRepository = workflowRepository;
            this.serviceRepository = serviceRepository;
            this.processRepository = processRepository;
        }

        /// <summary>
        /// List of connectors that are native to HV5 and don't require
        /// service host to run
        /// </summary>
        private readonly IEnumerable<string> blacklist = new List<string> 
        {
            "Finish",
            "Get Container Property", 
            "Set Container Property",
            "Get Submitter",
            "Log",
            "MimeType"
        };

        public IEnumerable<WorkflowConnector> GetConnectors()
        {
            var connectors =  connectorRepository.GetAll();
            
            // filter down connectors based on blacklist
            return connectors.Where(s => !blacklist.Contains(s.Name));
        }

        public IEnumerable<WorkflowHostInfo> GetWorkflowHosts()
        {
            return workflowRepository.GetAll();
        }

        public IEnumerable<ServiceHostInfo> GetServiceHosts()
        {
            return serviceRepository.GetAll();
        }

        public IEnumerable<WorkflowProcessInfo> GetProcessInfo()
        {
            return processRepository.GetAll(includeAdminData: true).GroupBy(x => x.ID)
                .Select(group => group.First());
        }
    }
}
