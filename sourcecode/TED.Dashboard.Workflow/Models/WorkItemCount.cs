
namespace TED.Dashboard.Workflow.Models
{
    public class WorkItemCount
    {
        public int Count { get; set; }
                
        public string Activity { get; set; }

        // Automated or Manual
        public string Type { get; set; }
    }
}
