using System;
using System.Collections.Generic;
using System.Data;
using TED.Dashboard.Repository;
using TED.Dashboard.Workflow.Models;

namespace TED.Dashboard.Workflow.Repositories
{
    public class WorkItemCountRepository : HV5DataStore<WorkItemCount>, IReadOnlyRepository<WorkItemCount>
    {
        private const string allSql = @"select * from (
                                        select name activity, 'Manual' type, count(*) count 
                                        from hvwf_work_items wi
                                        join hvwf_execution_current ex on (ex.work_item_id=wi.id)
                                        join hvwf_queues q on (q.id=ex.queue_id)
                                        group by name
                                        union all
                                        select name, 'Automated' type, count(*) count 
                                            from hvwf_work_items wi
                                        join hvwf_execution_current ex on (ex.work_item_id=wi.id)
                                        join hvsvc_services s on (s.id=ex.activity_id)
                                        group by name)
                                        order by type, count desc";

        /// <summary>
        /// Mapping for converting sql results into a model object
        /// </summary>
        private readonly Func<IDataReader, WorkItemCount> DataMapping =
            r => new WorkItemCount
            {
                Activity = r["activity"].ToString(),
                Type = r["type"].ToString(),
                Count = Convert.ToInt32(r["count"])
            };

        public IEnumerable<WorkItemCount> GetAll()
        {
            // sql to get connector logs from database
            return RunSelect(allSql, DataMapping);
        }
    }
}
