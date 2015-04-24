using System;
using System.Collections.Generic;
using System.Data;
using TED.Dashboard.Repository;
using TED.Dashboard.Workflow.Models;

namespace TED.Dashboard.Workflow.Repositories
{
    public class StatusCountRepository : HV5DataStore<WorkItemStatusCount>, IReadOnlyRepository<WorkItemStatusCount>
    {
        private const string allSql = @"select * from (
                                            select status, count(*) count 
                                            from highview_owner.hvwf_work_items wi
                                            group by status
                                        )
                                        order by count desc";

        /// <summary>
        /// Mapping for converting sql results into a model object
        /// </summary>
        private readonly Func<IDataReader, WorkItemStatusCount> DataMapping =
            r => new WorkItemStatusCount
            {
                Status = r["status"].ToString(),
                Count = Convert.ToInt32(r["count"])
            };

        public IEnumerable<WorkItemStatusCount> GetAll()
        {
            // sql to get connector logs from database
            return RunSelect(allSql, DataMapping);
        }
    }
}
