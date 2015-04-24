using System.Collections.Generic;
using TED.Dashboard.Workflow.Models;

namespace TED.Dashboard.Workflow.Repositories
{
    public interface IQueueUserRepository
    {
        IEnumerable<QueueUser> GetQueueUsers(ulong queueID);
    }
}
