using System;
using System.Collections.Generic;
using System.Data;
using TED.Dashboard.Repository;
using TED.Dashboard.Search.Models;

namespace TED.Dashboard.Search.Repositories
{
    public class OperandCountRepository : HV5DataStore<SearchOperandCount>, IReadOnlyRepository<SearchOperandCount>
    {
        private const string sql = @"select * from ( 
                                    select search_operand, count(*) as searchcount
                                    from search_info
	                                    group by search_operand
	                                    order by searchcount desc)
                                      where rownum < 15";

        /// <summary>
        /// Mapping for converting sql results into a model object
        /// </summary>
        private readonly Func<IDataReader, SearchOperandCount> DataMapping =
            r => new SearchOperandCount
            {
                Count = r["searchcount"] is DBNull ? 0 : Convert.ToInt32(r["searchcount"]),
                Operand = r["search_operand"] is DBNull ? "(null)" : r["search_operand"].ToString()
            };

        public IEnumerable<SearchOperandCount> GetAll()
        {
            return RunSelect(sql, DataMapping);
        }
    }
}
