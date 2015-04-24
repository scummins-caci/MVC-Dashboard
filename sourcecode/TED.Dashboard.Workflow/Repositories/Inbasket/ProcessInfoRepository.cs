using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using ODataToSql;
using TED.Dashboard.Repository;
using TED.Dashboard.Workflow.Models;

namespace TED.Dashboard.Workflow.Repositories
{
    public class ProcessInfoRepository : HV5DataStore<WorkflowProcessInfo>, IProcessInfoRepository
    {
        private const string versionProcedure = "hvwf_status_pkg.get_processes()";
        private const string noVersionProcedure = "hvwf_q_status_pkg.get_queue_process()";

        private const string baseSql = @"select * from (
                                            select rownum as rn, a.*   
                                                from (
                                                    select count(*) over() total, b.* 
                                                    from table({0}) b {1}
                                                    order by {2}
                                                ) a
                                            )";

        private const string countSql = @"select count(*) from table({0}) {1}";

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
        private string BuildSql(string sql, Expression filter, Expression orderBy, bool includeAdminData = false)
        {
            var translator = new QueryTranslator(includeAdminData ? AdminColumnMapping : ColumnMapping);
            var whereClause = translator.TranslateFilter(filter);
            var orderByClause = translator.TranslateOrderBy(orderBy);

            return string.Format(sql, includeAdminData ? versionProcedure : noVersionProcedure,
                                        whereClause.Length > 0 ? "where " + whereClause : "", 
                                        orderByClause.Length > 0 ? orderByClause : "name asc");
        }

        /// <summary>
        /// add a where clause if filter data exists
        /// </summary>
        /// <param name="sql">starting sql</param>
        /// <param name="filter">filter for where clause</param>
        /// <returns>full sql statement</returns>
        private string BuildSql(string sql, Expression filter, bool includeAdminData = false)
        {
            var translator = new QueryTranslator(includeAdminData ? AdminColumnMapping : ColumnMapping);
            var whereClause = translator.TranslateFilter(filter);

            return string.Format(sql, includeAdminData ? versionProcedure : noVersionProcedure, 
                                        whereClause.Length > 0 ? "where " + whereClause : "");
        }

        /// <summary>
        /// Mapping for converting sql results into a model object
        /// </summary>
        private readonly Func<IDataReader, WorkflowProcessInfo> DataMapping =
           r => new WorkflowProcessInfo
           {
               ID = r["process_id"] is DBNull ? 0 : Convert.ToInt32(r["process_id"]),
               Name = r["name"] is DBNull ? "" : r["name"].ToString(),
               Description = r["description"] is DBNull ? "" : r["description"].ToString(),
               UnassignedItems = r["unassigned"] is DBNull ? 0 : Convert.ToInt32(r["unassigned"]),
               AssignedItems = r["assigned"] is DBNull ? 0 : Convert.ToInt32(r["assigned"]),
               ProcessCount = r["total"] is DBNull ? 0 : Convert.ToInt32(r["total"])
           }; 

        private readonly Func<IDataReader, WorkflowProcessInfo> AdminDataMapping =
            r => new WorkflowProcessInfo
            {
                ID = r["process_id"] is DBNull ? 0 : Convert.ToInt32(r["process_id"]),
                ProcessDefinitionID = r["process_defn_id"] is DBNull ? 0 : Convert.ToInt32(r["process_defn_id"]),
                Name = r["name"] is DBNull ? "" : r["name"].ToString(),
                Description = r["description"] is DBNull ? "" : r["description"].ToString(),                
                IsEnabled = !(r["is_enabled"] is DBNull) && Convert.ToBoolean(r["is_enabled"]),
                LoginName = r["login_name"] is DBNull ? "" : r["login_name"].ToString(),
                CreatedDate = r["created_date"] is DBNull ? DateTime.MinValue : Convert.ToDateTime(r["created_date"]),
                ActivityInstID = r["activity_inst_id"] is DBNull ? 0 : Convert.ToUInt64(r["activity_inst_id"]),
                ParentDefinitionID = r["parent_defn_id"] is DBNull ? 0 : Convert.ToUInt64(r["parent_defn_id"]),
                PendingItems = r["pending"] is DBNull ? 0 : Convert.ToInt32(r["pending"]),
                ActiveItems = r["active_items"] is DBNull ? 0 : Convert.ToInt32(r["active_items"]),
                PausedItems = r["paused"] is DBNull ? 0 : Convert.ToInt32(r["paused"]),
                IdleItems = r["idle"] is DBNull ? 0 : Convert.ToInt32(r["idle"]),
                SuspendedItems = r["suspended"] is DBNull ? 0 : Convert.ToInt32(r["suspended"]),
                CancelledItems = r["cancelled"] is DBNull ? 0 : Convert.ToInt32(r["cancelled"]),
                FailedItems = r["failed"] is DBNull ? 0 : Convert.ToInt32(r["failed"]),
                CompletedItems = r["completed"] is DBNull ? 0 : Convert.ToInt32(r["completed"]),
                ProcessCount = r["total"] is DBNull ? 0 : Convert.ToInt32(r["total"]),
                // columns that may not show up
                Version = r.HasColumn("version") && r["version"] is DBNull ? 0 : Convert.ToInt32(r["version"])
            };

        private readonly IDictionary<string, string> ColumnMapping = new Dictionary<string, string>
            {
                {"ID", "process_id"},
                {"Name", "name"},
                {"Description", "description"},
                {"UnassignedItems", "unassigned"},
                {"AssignedItems", "assigned"}
            };


        private readonly IDictionary<string, string> AdminColumnMapping = new Dictionary<string, string>
            {
                {"ID", "process_id"},
                {"ProcessDefinitionID", "process_defn_id"},
                {"Name", "name"},
                {"Description", "description"},
                {"Version", "version"},
                {"IsEnabled", "is_enabled"},
                {"LoginName", "login_name"},
                {"CreatedDate", "created_date"},
                {"ActivityInstID", "activity_inst_id"},
                {"ParentDefinitionID", "parent_defn_id"},
                {"ActiveItems", "active_items"},
                {"PausedItems", "paused"},
                {"IdleItems", "idle"},
                {"SuspendedItems", "suspended"},
                {"CancelledItems", "cancelled"},
                {"FailedItems", "failed"},
                {"CompletedItems", "completed"}
            };

        public IEnumerable<WorkflowProcessInfo> GetAll(Expression<Func<WorkflowProcessInfo, bool>> filter = null,
                                                        Expression orderBy = null, bool includeAdminData = false)
        {
            var mapping = includeAdminData ? AdminDataMapping : DataMapping;
            return RunSelect(BuildSql(baseSql, filter, orderBy, includeAdminData), mapping);
        }

        public IEnumerable<WorkflowProcessInfo> GetPage(int page, int count, 
                                                        Expression<Func<WorkflowProcessInfo, bool>> filter = null,
                                                        Expression orderBy = null,
                                                        bool includeAdminData = false)
        {
            var parameters = new Dictionary<string, string> { { "count", count.ToString() }, { "page", page.ToString() } };
            var mapping = includeAdminData ? AdminDataMapping : DataMapping;
            
            return RunSelect(
                        AddPaging(
                            BuildSql(baseSql, filter, orderBy, includeAdminData)), mapping, parameters);
        }

        public int GetCount(Expression<Func<WorkflowProcessInfo, bool>> filter = null, bool includeAdminData = false)
        {
            return GetItemCount(BuildSql(countSql, filter, includeAdminData));
        }
    }
}
