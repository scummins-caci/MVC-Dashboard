using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using ODataToSql;
using Oracle.DataAccess.Client;
using TED.Dashboard.Repository;
using TED.Dashboard.Workflow.Models;

namespace TED.Dashboard.Workflow.Repositories
{
    public class WorkItemRepository : HV5DataStore<WorkItem>, IWorkItemRepository
    {
        private const string sql = "hvwf_q_status_pkg.get_queue_detail_cursor";

        private readonly Func<IDataReader, WorkItem> DataMapping =
            r => new WorkItem
            {
                ID = r["work_item_id"] is DBNull ? 0 : Convert.ToUInt64(r["work_item_id"]),
                ContainerInstId = r["inst_id"] is DBNull ? 0 : Convert.ToUInt64(r["inst_id"]),
                ExecutionId = r["execution_id"] is DBNull ? 0 : Convert.ToUInt64(r["execution_id"]),
                ActivityInstId = r["activity_inst_id"] is DBNull ? 0 : Convert.ToUInt64(r["activity_inst_id"]),
                QueueId = r["queue_id"] is DBNull ? 0 : Convert.ToInt32(r["queue_id"]),
                ContainerType = r["type_name"] is DBNull ? "" : r["type_name"].ToString(),
                UserId = r["user_id"] is DBNull ? 0 : Convert.ToInt32(r["user_id"]),
                LoginName = r["login_name"] is DBNull ? "" : r["login_name"].ToString(),
                GroupName = r["group_name"] is DBNull ? "" : r["group_name"].ToString(),
                DateSubmitted = r["submitted"] is DBNull ? DateTime.MinValue : Convert.ToDateTime(r["submitted"]),
                Status = r["status"] is DBNull ? "" : r["status"].ToString(),
                StatusChanged = r["status_changed"] is DBNull ? DateTime.MinValue : Convert.ToDateTime(r["status_changed"]),
                ShortName = r["short_name"] is DBNull ? "" : r["short_name"].ToString(),
                ProcessName = r["process_name"] is DBNull ? "" : r["process_name"].ToString(),
                QueueName = r["queue_name"] is DBNull ? "" : r["queue_name"].ToString(),
                CheckedOutUserId = r["checked_out_user_id"] is DBNull ? 0 : Convert.ToInt32(r["checked_out_user_id"]),
                CheckedOutUserName = r["checked_out_user_name"] is DBNull ? "" : r["checked_out_user_name"].ToString(),
                WorkitemCount = r["ctotal"] is DBNull ? 0 : Convert.ToInt32(r["ctotal"]),
                
                // fields that may not be returned
                ParentHarmonyNumber = !r.HasColumn("harmony_number") || r["harmony_number"] is DBNull ? "" : r["harmony_number"].ToString(),
                BatchName = !r.HasColumn("batch_name") || r["batch_name"] is DBNull ? "" : r["batch_name"].ToString(),
                Priority = !r.HasColumn("priority") || r["priority"] is DBNull ? "" : r["priority"].ToString(),
                LoadId = !r.HasColumn("qa_load_id") || r["qa_load_id"] is DBNull ? "" : r["qa_load_id"].ToString(),
                TotalPages = !r.HasColumn("total_pages") || r["total_pages"] is DBNull ? 0 : Convert.ToInt32(r["total_pages"]),
                MediaLength = !r.HasColumn("video_length") || r["video_length"] is DBNull ? "" : r["video_length"].ToString()
            };

        private readonly IDictionary<string, string> ColumnMapping = new Dictionary<string, string>
            {
                {"ID", "work_item_id"},
                {"ContainerInstId", "inst_id"},
                {"ExecutionId", "execution_id"},
                {"ActivityInstId", "activity_inst_id"},
                {"QueueId", "queue_id"},
                {"ContainerType", "type_name"},
                {"UserId", "user_id"},
                {"LoginName", "login_name"},
                {"GroupName", "group_name"},
                {"DateSubmitted", "submitted"},
                {"StatusChanged", "status_changed"},
                {"Status", "status"},
                {"ShortName", "short_name"},
                {"ProcessName", "process_name"},
                {"QueueName", "queue_name"},
                {"CheckedOutUserId", "checked_out_user_id"},
                {"CheckedOutUserName", "checked_out_user_name"},
                {"WorkitemCount", "ctotal"},
                {"ParentHarmonyNumber", "harmony_number"},
                {"BatchName", "batch_name"},
                {"Priority", "priority"},
                {"LoadId", "qa_load_id"},
                {"TotalPages", "total_pages"},
                {"MediaLength", "video_length"}
            };

        public IEnumerable<WorkItem> GetPage(ulong queueId, int page, int count, Expression<Func<WorkItem, bool>> filter = null, Expression orderBy = null)
        {
            return GetPage(queueId, 0, page, count, filter, orderBy);
        }

        public IEnumerable<WorkItem> GetPage(ulong queueId, ulong processId, int page, int count, Expression<Func<WorkItem, bool>> filter = null, Expression orderBy = null)
        {
            var parameters = GetParameters(queueId, processId, filter, orderBy, count, page);
            return RunStoredProcedure(sql, DataMapping, parameters);
        }

        public int GetCount(ulong queueId, Expression<Func<WorkItem, bool>> filter = null)
        {
            return GetCount(queueId, 0, filter);
        }

        public int GetCount(ulong queueId, ulong processId, Expression<Func<WorkItem, bool>> filter = null)
        {
            // leverage the count that comes back with the full collection
            var parameters = GetParameters(queueId, processId, filter, null, 1, 1);
            var results = RunStoredProcedure(sql, DataMapping, parameters);

            var firstItem = results.FirstOrDefault();
            return firstItem != null ? firstItem.WorkitemCount : 0;
        }

        private  IEnumerable<OracleParameter> GetParameters(ulong queueID, ulong processID, Expression filter, Expression orderBy, int count, int page)
        {
            var recordSet = new OracleParameter("p_recordset", OracleDbType.RefCursor, ParameterDirection.Output);
            var queueId = new OracleParameter("p_queue_id", OracleDbType.Int16, ParameterDirection.Input);
            var proId = new OracleParameter("p_process_id", OracleDbType.Int16, ParameterDirection.Input);
            var whereStatement = new OracleParameter("p_where", OracleDbType.Varchar2, ParameterDirection.Input);
            var sortStatement = new OracleParameter("p_sort_order", OracleDbType.Varchar2, ParameterDirection.Input);
            var pSize = new OracleParameter("p_page_size", OracleDbType.Int16, ParameterDirection.Input);
            var pCount = new OracleParameter("p_page_num", OracleDbType.Int16, ParameterDirection.Input);

            var args = new [] { recordSet, queueId, proId, whereStatement, sortStatement, pSize, pCount };
            
            var translator = new QueryTranslator(ColumnMapping);
            var whereClause = translator.TranslateFilter(filter);
            var orderByClause = translator.TranslateOrderBy(orderBy);

            queueId.Value = queueID;
            proId.Value = processID == 0 ? null : (object)processID;

            whereStatement.Value = whereClause.Length > 0 ? whereClause : null;
            sortStatement.Value = orderByClause.Length > 0 ? orderByClause : null;
            pSize.Value = count;
            pCount.Value = page;

            return args;
        }
    }
}
