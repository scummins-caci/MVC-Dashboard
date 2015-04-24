using System;
using System.Collections.Generic;
using System.Data;
using TED.Dashboard.Dataflow.Models;
using TED.Dashboard.Repository;

namespace TED.Dashboard.Dataflow.Repositories
{
    public class ReceiptRepository : DataflowDataStore<Receipt>, IPageableRepository<Receipt>
    {
        private const string baseSql = @"select * from (
                                            select rownum as rn, a.*
                                                from (
                                                    select  harmony_number, change_id, rec_type, action, 
                                                            extract_date 
                                                    from rec_change_tracking
                                                    order by extract_date desc
                                                    ) a
                                             )";

        private const string countSql = "select count(*) from rec_change_tracking";

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
        private readonly Func<IDataReader, Receipt> DataMapping =
            r => new Receipt
            {
                ChangeId = r["change_id"] is DBNull ? 0 : Convert.ToInt64(r["change_id"]),
                HarmonyNumber = r["harmony_number"] is DBNull ? "" : r["harmony_number"].ToString(),
                ExtractDate = r["extract_date"] is DBNull ? DateTime.MinValue : Convert.ToDateTime(r["extract_date"]),
                ReceiptType = r["rec_type"] is DBNull ? "" : r["rec_type"].ToString(),
                Action = r["action"] is DBNull ? "" : r["action"].ToString()
            };


        public IEnumerable<Receipt> GetAll()
        {
            // sql to get connector logs from database
            return RunSelect(GetAllSql, DataMapping);
        }

        public IEnumerable<Receipt> GetPage(int page, int count)
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
