using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using TED.Dashboard.Services.Models;
using TED.Dashboard.Workflow.Models;

namespace TED.Dashboard.Workflow.Services
{
    public interface IWorkitemService
    {
        IEnumerable<WorkItemStatusCount> GetStatusCounts();

        IEnumerable<WorkItemCount> GetQueueCounts();

        // get counts of workites in manual activities
        IEnumerable<WorkItemCount> GetManualQueueCounts();

        // get counts of workites in automated activities
        IEnumerable<WorkItemCount> GetAutomatedQueueCounts();

        IEnumerable<QueueUser> GetQueueUsers(ulong queueId);

        PageableItem<WorkflowProcessInfo> GetProcesses(int page = 1, int count = 25,
                                                        Expression<Func<WorkflowProcessInfo, bool>> filter = null, 
                                                        Expression orderBy = null,
                                                        bool includeAdminInfo = false);

        PageableItem<WorkflowQueue> GetQueues(ulong processId, int page = 1, int count = 25,
                                                        Expression<Func<WorkflowQueue, bool>> filter = null,
                                                        Expression orderBy = null);

        PageableItem<WorkItem> GetWorkItems(ulong queueId, ulong processId = 0, int page = 1, int count = 25,
                                                        Expression<Func<WorkItem, bool>> filter = null,
                                                        Expression orderBy = null);
    }
}
