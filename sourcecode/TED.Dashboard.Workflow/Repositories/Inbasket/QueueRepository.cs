using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using ODataToSql;
using TED.Dashboard.Repository;
using TED.Dashboard.Workflow.Models;

namespace TED.Dashboard.Workflow.Repositories
{
    public class QueueRepository : HV5DataStore<WorkflowQueue>, IQueueRepository
    {
        private const string baseSql = @"select * from (
                                            select rownum as rn, a.*   
                                                from (
                                                    select count(*) over() total, b.* 
                                                    from table(hvwf_q_status_pkg.get_queues(:process)) b {0}
                                                    order by name asc
                                                ) a
                                            )";

        private const string countSql = @"select count(*) from table(hvwf_q_status_pkg.get_queues(:process)) {0}";

        /// <summary>
        /// Add paging to a sql statement
        /// </summary>
        private static string AddPaging(string startSql)
        {
            return string.Format("{0} where rownum <= :count and rn > (:page - 1) * :count", startSql);
        }

        /// <summary>
        /// add a where clause if filter data exists
        /// </summary>
        /// <param name="sql">starting sql</param>
        /// <param name="filter">filter for where clause</param>
        /// <param name="orderBy">orderby for where clause</param>
        /// <returns>full sql statement</returns>
        private string BuildSql(string sql, Expression filter, Expression orderBy)
        {
            var translator = new QueryTranslator(ColumnMapping);
            var whereClause = translator.TranslateFilter(filter);
            var orderByClause = translator.TranslateOrderBy(orderBy);

            return string.Format(sql, whereClause.Length > 0 ? "where " + whereClause : "",
                                        orderByClause.Length > 0 ? orderByClause : "name asc, version asc");
        }

        /// <summary>
        /// add a where clause if filter data exists
        /// </summary>
        /// <param name="sql">starting sql</param>
        /// <param name="filter">filter for where clause</param>
        /// <returns>full sql statement</returns>
        private string BuildSql(string sql, Expression filter)
        {
            var translator = new QueryTranslator(ColumnMapping);
            var whereClause = translator.TranslateFilter(filter);

            return string.Format(sql, whereClause.Length > 0 ? "where " + whereClause : "");
        }

        private readonly Func<IDataReader, WorkflowQueue> DataMapping =
            r => new WorkflowQueue
            {
                ID = r["queue_id"] is DBNull ? 0 : Convert.ToUInt64(r["queue_id"]),
                Name = r["name"] is DBNull ? "" : r["name"].ToString(),
                Description = r["description"] is DBNull ? "" : r["description"].ToString(),
                AssignedItems = r["assigned"] is DBNull ? 0 : Convert.ToInt32(r["assigned"]),
                UnassignedItems = r["unassigned"] is DBNull ? 0 : Convert.ToInt32(r["unassigned"]),
                QueueCount = r["total"] is DBNull ? 0 : Convert.ToInt32(r["total"])
            };

        private readonly IDictionary<string, string> ColumnMapping = new Dictionary<string, string>
            {
                {"ID", "queue_id"},
                {"Name", "name"},
                {"Description", "description"},
                {"AssignedItems", "assigned"},
                {"UnassignedItems", "unassigned"}
            };

        public IEnumerable<WorkflowQueue> GetAll(ulong processId, Expression<Func<WorkflowQueue, bool>> filter = null, Expression orderBy = null)
        {
            var parameters = new Dictionary<string, string> { { "process", processId.ToString() } };
            return RunSelect(BuildSql(baseSql, filter, orderBy), DataMapping, parameters);
        }

        public IEnumerable<WorkflowQueue> GetPage(ulong processId, int page, int count, Expression<Func<WorkflowQueue, bool>> filter = null, Expression orderBy = null)
        {
            var parameters = new Dictionary<string, string> { { "process", processId.ToString() }, { "count", count.ToString() }, { "page", page.ToString() } };
            return RunSelect(AddPaging(BuildSql(baseSql, filter, orderBy)), DataMapping, parameters);
        }

        public int GetCount(ulong processId, Expression<Func<WorkflowQueue, bool>> filter = null)
        {
            var parameters = new Dictionary<string, string> {{"process", processId.ToString()}};
            return GetItemCount(BuildSql(countSql, filter), parameters);
        }
    }
}
