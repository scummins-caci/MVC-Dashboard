using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using TED.Dashboard.Workflow.Models;

namespace TED.Dashboard.Workflow.Repositories
{
    public interface IWorkItemRepository
    {
        IEnumerable<WorkItem> GetPage(ulong queueId, int page, int count, Expression<Func<WorkItem, bool>> filter = null, Expression orderBy = null);
        IEnumerable<WorkItem> GetPage(ulong queueId, ulong processId, int page, int count, Expression<Func<WorkItem, bool>> filter = null, Expression orderBy = null);
        int GetCount(ulong queueId, Expression<Func<WorkItem, bool>> filter = null);
        int GetCount(ulong queueId, ulong processId, Expression<Func<WorkItem, bool>> filter = null);
    }
}
