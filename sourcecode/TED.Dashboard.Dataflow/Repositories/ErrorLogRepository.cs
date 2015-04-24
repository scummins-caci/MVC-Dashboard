using System;
using System.Collections.Generic;
using System.Data;
using TED.Dashboard.Dataflow.Models;
using TED.Dashboard.Repository;

namespace TED.Dashboard.Dataflow.Repositories
{
    public class ErrorLogRepository : DataflowDataStore<ErrorEntry>, IPageableRepository<ErrorEntry>
    {
        private const string baseSql = @"select * from (
                                            select rownum as rn, a.*
                                                from (
                                                    harmony_number, created_on, processed_on, processed_status, i.*
                                                    from dflow_docmeta_received a, xmltable
                                                    (
                                                       '//error'
                                                       passing processed_response_xml
                                                       columns 
                                                        code      varchar2(150) path '@code',
                                                        message   varchar2(250) path '@message',
                                                        constraint varchar2(250) path '@constraint',
                                                        type  varchar2(250) path '@type'    
                                                    ) i
                                                    where processed_status = 'FAILURE'
                                                    order by updated_on desc
                                                ) a
                                            )";

        private const string countSql = @"select count(*) from dflow_docmeta_received
                                         where processed_status = 'FAILURE'";

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
        private readonly Func<IDataReader, ErrorEntry> DataMapping =
            r => new ErrorEntry
            {
                HarmonyNumber = r["harmony_number"] is DBNull ? "" : r["harmony_number"].ToString(),
                Status = r["processed_status"] is DBNull ? "" : r["processed_status"].ToString(),
                ErrorCode = r["code"] is DBNull ? "" : r["code"].ToString(),
                ErrorMessage = r["message"] is DBNull ? "" : r["message"].ToString(),
                ErrorConstraint = r["constraint"] is DBNull ? "" : r["constraint"].ToString(),
                ErrorType = r["type"] is DBNull ? "" : r["type"].ToString()
            };

        public IEnumerable<ErrorEntry> GetAll()
        {
            return RunSelect(GetAllSql, DataMapping);
        }

        public IEnumerable<ErrorEntry> GetPage(int page, int count)
        {
            var parameters = new Dictionary<string, string> { { "count", count.ToString() }, { "page", page.ToString() } };
            return RunSelect(PageSql, DataMapping, parameters);
        }

        public int GetCount()
        {
            return GetItemCount(countSql);
        }
    }
}
