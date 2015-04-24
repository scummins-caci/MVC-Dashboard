
namespace TED.Dashboard.Workflow.Models
{
    public class WorkflowQueue
    {
        public ulong ID { get; set; }
        public string Name { get; set;}
        public string Description { get; set; }
        public int AssignedItems { get; set; }
        public int UnassignedItems { get; set; }

        // count property
        public int QueueCount { get; set; }
    }
}
