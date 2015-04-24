using System.Collections.Generic;
using TED.Dashboard.Workflow.Models;

namespace TED.Dashboard.Workflow.Services
{
    public interface IWorkflowStatusService
    {
        // gets a list of all of the workflow connectors for a given TED system
        IEnumerable<WorkflowConnector> GetConnectors();

        // gets a list of currently running workflow hosts
        IEnumerable<WorkflowHostInfo> GetWorkflowHosts();

        // gets a list of currently running service hosts
        IEnumerable<ServiceHostInfo> GetServiceHosts();

                // gets a list of currently running service hosts
        IEnumerable<WorkflowProcessInfo> GetProcessInfo();

        
    }
}
