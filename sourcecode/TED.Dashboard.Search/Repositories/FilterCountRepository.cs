using System;
using System.Collections.Generic;
using System.Data;
using TED.Dashboard.Repository;
using TED.Dashboard.Search.Models;

namespace TED.Dashboard.Search.Repositories
{
    public class FilterCountRepository : HV5DataStore<SearchFilterCount>, IReadOnlyRepository<SearchFilterCount>
    {
        private const string sql = @"select * from ( 
                                    select search_filter, count(*) as filtercount
                                    from search_info
	                                    group by search_filter
	                                    order by filtercount desc)
                                      where rownum < 15";

        /// <summary>
        /// Mapping for converting sql results into a model object
        /// </summary>
        private readonly Func<IDataReader, SearchFilterCount> DataMapping =
            r => new SearchFilterCount
            {
                Count = r["filtercount"] is DBNull ? 0 : Convert.ToInt32(r["filtercount"]),
                Filter = r["search_filter"] is DBNull ? "(null)" : r["search_filter"].ToString()
            };

        public IEnumerable<SearchFilterCount> GetAll()
        {
            return RunSelect(sql, DataMapping);
        }
    }
}
