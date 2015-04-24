using System;
using System.Data;

namespace TED.Dashboard.Workflow.Models
{
    public abstract class HostInfo
    {
        // host name
        public string Name { get; set; }
        
        // last time host was accessed
        public string TimeStamp { get; set; }
    }
}
