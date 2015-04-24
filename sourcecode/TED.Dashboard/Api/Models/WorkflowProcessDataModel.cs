using Omu.ValueInjecter;
using TED.Dashboard.Workflow.Models;

namespace TED.Dashboard.Api.Models
{
    public class WorkflowProcessDataModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int AssignedItems { get; set; }
        public int UnassignedItems { get; set; }

        public static WorkflowProcessDataModel BuildFromWorkflowProcessInfo(WorkflowProcessInfo model)
        {
            var processVM = new WorkflowProcessDataModel();
            processVM.InjectFrom(model);

            return processVM;
        }
    }
}