
using System;

namespace TED.Dashboard.Workflow.Models
{
    public class WorkflowProcessInfo
    {        
        public int ID { get; set; }
        public int ProcessDefinitionID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Version { get; set; }
        public bool IsEnabled { get; set; }
        public string LoginName { get; set; }
        public DateTime CreatedDate { get; set; }
        public ulong ActivityInstID { get; set; }
        public ulong ParentDefinitionID { get; set; }
        public int PendingItems { get; set; }
        public int PausedItems { get; set; }
        public int IdleItems { get; set; }
        public int SuspendedItems { get; set; }
        public int CancelledItems { get; set; }
        public int FailedItems { get; set; }
        public int CompletedItems { get; set; }
        public int ActiveItems { get; set; }
        public int AssignedItems { get; set; }
        public int UnassignedItems { get; set; }

        // count property
        public int ProcessCount { get; set; }
    }
}
