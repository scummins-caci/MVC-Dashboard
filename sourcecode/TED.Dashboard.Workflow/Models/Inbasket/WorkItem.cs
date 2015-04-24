using System;

namespace TED.Dashboard.Workflow.Models
{
    public class WorkItem
    {
        public ulong ID { get; set; }
        public ulong ContainerInstId { get; set; }
        public ulong ExecutionId { get; set; }
        public ulong ActivityInstId { get; set; }
        public int QueueId { get; set; }
        public string ContainerType { get; set; }
        public int UserId { get; set; }
        public string LoginName { get; set; }
        public string GroupName { get; set; }
        public DateTime DateSubmitted { get; set; }
        public DateTime StatusChanged { get; set; }
        public string Status { get; set; }
        public string ShortName { get; set; }
        public string ProcessName { get; set; }
        public string QueueName { get; set; }
        public int CheckedOutUserId { get; set; }
        public string CheckedOutUserName { get; set; }
        public int WorkitemCount { get; set; }

        // container properties, these may not be available based on container type and queue
        public string ParentHarmonyNumber { get; set; }     // section containers

        public string BatchName { get; set; }               // record containers
        public string Priority { get; set; }                // record containers
        public string MediaLength { get; set; }             // record containers
        public int TotalPages { get; set; }                 // record containers

        public string LoadId { get; set; }                  // record or batch containers
    }
}
