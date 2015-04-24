using System.Collections.Generic;
using System.Linq;
using TED.Dashboard.Utilities;
using TED.Dashboard.Workflow.Models;

namespace TED.Dashboard.Api.Models
{
    public class WorkflowServicesDataModel
    {
        public IEnumerable<WorkflowConnector> WorkflowConnectors { get; set; }
        public IEnumerable<HostInfo> WorkflowHosts { get; set; }
        public IEnumerable<HostInfo> ServiceHosts { get; set; }
        public IEnumerable<WorkflowProcessInfo> ProcessInfo { get; set; } 

        public int WorkflowHostCount 
        {
            get { return WorkflowHosts.Count(); }
        }

        public int ServiceHostCount
        {
            get { return ServiceHosts.Count(); }
        }

        public int ProcessCount
        {
            get { return ProcessInfo.Count(); }
        }

        public int EnabledProcessCount
        {
            get { return ProcessInfo.Count(c => c.IsEnabled = true); }
        }

        public int ConnectorCount
        {
            get
            {
                return WorkflowConnectors.DistinctBy(c => c.Name).Count();
            }
        }

        public int RunningConnectorCount
        {
            get
            {
                return WorkflowConnectors.Where(c => c.Instances > 0)
                                        .DistinctBy(c => c.Name).Count();
            }
        }
    }
}