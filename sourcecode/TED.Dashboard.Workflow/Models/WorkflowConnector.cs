using System;
using System.Data;

namespace TED.Dashboard.Workflow.Models
{
    /// <summary>
    /// Model that represents workflow connector (used to be 'service') information
    /// </summary>
    public class WorkflowConnector
    {
        // connector name
        public string Name { get; set; }
        
        // whether connector is currently enabled
        public bool IsEnabled { get; set; }

        // host where connector is running
        public string HostName { get; set; }

        // number of instances on that host
        public int Instances { get; set; }
    }
}
