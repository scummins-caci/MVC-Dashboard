using System;
using System.Collections.Generic;
using System.Data;
using TED.Dashboard.Repository;
using TED.Dashboard.Search.Models;

namespace TED.Dashboard.Search.Repositories
{
    /// <summary>
    /// Retrieves a count of searches executed by the day for the past 15 days
    /// </summary>
    public class SearchCountRepository : HV5DataStore<SearchAuditCount>, IReadOnlyRepository<SearchAuditCount> 
    {
        private const string sql = @"select trunc(tstamp, 'DD') day, count(*) as searchcount
                                      from hv_audit a
                                      join hv_audit_configs c
                                      on c.action_id = a.action_id
                                      where c.action_name = 'WB_SEARCH'
                                        and tstamp > trunc(SYSDATE -15)
                                      group by trunc(tstamp, 'DD')
                                      order by trunc(tstamp, 'DD') desc";

        /// <summary>
        /// Mapping for converting sql results into a model object
        /// </summary>
        private readonly Func<IDataReader, SearchAuditCount> DataMapping =
            r => new SearchAuditCount
            {
                SearchesExecuted = r["searchcount"] is DBNull ? 0 : Convert.ToInt32(r["searchcount"]),
                DaySearchesExecuted = r["day"] is DBNull ? DateTime.MinValue : Convert.ToDateTime(r["day"])
            };

        public IEnumerable<SearchAuditCount> GetAll()
        {
            return RunSelect(sql, DataMapping);
        }
    }
}
