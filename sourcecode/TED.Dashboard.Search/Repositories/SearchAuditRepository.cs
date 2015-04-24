using System;
using System.Collections.Generic;
using System.Data;
using Newtonsoft.Json.Linq;
using TED.Dashboard.Repository;
using TED.Dashboard.Search.Models;

namespace TED.Dashboard.Search.Repositories
{
    public class SearchAuditRepository : HV5DataStore<SearchAudit>, IPageableRepository<SearchAudit>
    {
        private const string baseSql = @"select * from (
                                            select rownum as rn, a.*
                                                from (
                                                    select count(user_name) over() total, user_name, tstamp, message
			                                        from hv_audit b
			                                        join hv_audit_configs c
			                                        on c.action_id = b.action_id
			                                        where c.action_name = 'WB_SEARCH'
			                                        order by tstamp desc
                                                ) a
                                            )";

        private const string countSql = @"select count(*) from hv_audit b join hv_audit_configs c
                                         on c.action_id = b.action_id where c.action_name = 'WB_SEARCH'";

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

        private readonly Func<IDataReader, SearchAudit> DataMapping =
            r =>
                {
                    var audit = new SearchAudit
                        {
                            UserName = r["user_name"] is DBNull ? "" : r["user_name"].ToString(),
                            DateExecuted = r["tstamp"] is DBNull ? DateTime.MinValue : Convert.ToDateTime(r["tstamp"]),
                            SearchCount = r["total"] is DBNull ? 0 : Convert.ToInt32(r["total"]),
                            Criteria = new List<SearchCriteria>()
                        };

                    // if message is null return item
                    if (r["message"] is DBNull)
                    {
                        return audit;
                    }

                    // audit message is a json object;  parse it into our search object
                    var search = JObject.Parse(r["message"].ToString());
                    var filters = search["filters"] as JArray;
                    if (filters == null)
                    {
                        return audit;
                    }

                    // search may have been multiple search filters
                    foreach (var filter in filters)
                    {
                        // we aren't interested in themes searches
                        if (filter["filter"].Value<string>() == "Themes")
                        {
                            continue;
                        }

                        var criteria = new SearchCriteria
                            {
                                Operator = filter["op"].Value<string>(),
                                FilterName = filter["filter"].Value<string>(),
                                Operands = new List<string>()
                            };

                        foreach (var operand in filter["operand"].Value<JArray>())
                        {
                            criteria.Operands.Add(operand.Value<string>());
                        }

                        audit.Criteria.Add(criteria);
                    }

                    return audit;
                };


        public IEnumerable<SearchAudit> GetAll()
        {
            return RunSelect(GetAllSql, DataMapping);
        }

        public IEnumerable<SearchAudit> GetPage(int page, int count)
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
