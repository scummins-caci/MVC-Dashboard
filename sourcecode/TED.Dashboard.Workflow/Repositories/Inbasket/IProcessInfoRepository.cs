using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using TED.Dashboard.Workflow.Models;

namespace TED.Dashboard.Workflow.Repositories
{
    public interface IProcessInfoRepository
    {
        IEnumerable<WorkflowProcessInfo> GetAll(Expression<Func<WorkflowProcessInfo, bool>> filter = null,
                                                Expression orderBy = null,
                                                bool includeAdminData = false);
        IEnumerable<WorkflowProcessInfo> GetPage(int page, int count, 
                                                    Expression<Func<WorkflowProcessInfo, bool>> filter = null,
                                                    Expression orderBy = null,
                                                    bool includeAdminData = false);
        int GetCount(Expression<Func<WorkflowProcessInfo, bool>> filter = null, bool includeAdminData = false);
    }
}
