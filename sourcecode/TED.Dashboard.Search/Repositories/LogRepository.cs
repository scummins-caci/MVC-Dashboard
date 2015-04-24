using System;
using System.Collections.Generic;
using System.Data;
using TED.Dashboard.Repository;
using TED.Dashboard.Search.Models;

namespace TED.Dashboard.Search.Repositories
{
    public class LogRepository : HV5DataStore<LogEntry>, IPageableRepository<LogEntry>
    {
        private const string baseSql = @"select * from (
                                            select rownum as rn, a.*
                                                from (
                                                    select count(log_id) over() total, log_id, tstamp, clientmachine, location, 
                                                            severity_level, message 
                                                    from hv_logs 
                                                    where (location like 'HighView.Services%' or location like 'HighView.Workflow%')
                                                        and (severity_level != 'DEBUG' and severity_level != 'INFO')
                                                        --and message not like 'Stack Trace:%'
                                                    order by tstamp desc
                                                ) a
                                            )";

        private const string countSql = @"select count(*) from hv_logs
                                         where (location like 'HighView.Services%' or location like 'HighView.Workflow%')
                                         and (severity_level != 'DEBUG' and severity_level != 'INFO')";

        /// <summary>
        /// Sql to get the full list
        /// </summary>
        private static string GetAllSql
        {
            get { return baseSql; }
        }

        /// <summary>
        /// Sql to get a single page
        /// </summary>
        private static string PageSql
        {
            get { return string.Format("{0} where rownum <= :count and rn > (:page - 1) * :count", baseSql); }
        }

        /// <summary>
        /// Mapping for converting sql results into a model object
        /// </summary>
        private readonly Func<IDataReader, LogEntry> DataMapping =
            r => new LogEntry
            {
                LogId = r["log_id"] is DBNull ? "" : r["log_id"].ToString(),
                ClientMachine = r["clientmachine"] is DBNull ? "" : r["clientmachine"].ToString(),
                TimeStamp = r["tstamp"] is DBNull ? DateTime.MinValue : Convert.ToDateTime(r["tstamp"]),
                Location = r["location"] is DBNull ? "" : r["location"].ToString(),
                Severity = r["severity_level"] is DBNull ? "" : r["severity_level"].ToString(),
                Message = r["message"] is DBNull ? "" : r["message"].ToString(),
                LogCount = r["total"] is DBNull ? 0 : Convert.ToInt32(r["total"])
            };

        public IEnumerable<LogEntry> GetAll()
        {
            // sql to get connector logs from database
            return RunSelect(GetAllSql, DataMapping);
        }

        public IEnumerable<LogEntry> GetPage(int page, int count)
        {
            var parameters = new Dictionary<string, string> { { "count", count.ToString() }, {"page", page.ToString()} };
            return RunSelect(PageSql, DataMapping, parameters);
        }

        public int GetCount()
        {
            return GetItemCount(countSql);
        }
    }
}
