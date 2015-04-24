using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using TED.Dashboard.Workflow.Models;

namespace TED.Dashboard.Workflow.Repositories
{
    public interface IQueueRepository
    {
        IEnumerable<WorkflowQueue> GetAll(ulong processId, Expression<Func<WorkflowQueue, bool>> filter = null, Expression orderBy = null);
        IEnumerable<WorkflowQueue> GetPage(ulong processId, int page, int count, Expression<Func<WorkflowQueue, bool>> filter = null, Expression orderBy = null);
        int GetCount(ulong processId, Expression<Func<WorkflowQueue, bool>> filter = null);
    }
}
