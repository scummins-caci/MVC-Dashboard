using System;
using System.Collections.Generic;
using System.Data;
using TED.Dashboard.Repository;
using TED.Dashboard.Search.Models;

namespace TED.Dashboard.Search.Repositories
{
    public class UserSearchCountRepository : HV5DataStore<UserSearchCount>, IReadOnlyRepository<UserSearchCount>
    {
        private const string sql = @"select * from ( 
                                    select user_name, count(*) as usersearchcount
                                    from search_info
	                                    group by user_name
	                                    order by usersearchcount desc
                                    )
                                    where rownum < 15";

        /// <summary>
        /// Mapping for converting sql results into a model object
        /// </summary>
        private readonly Func<IDataReader, UserSearchCount> DataMapping =
            r => new UserSearchCount
            {
                Count = r["usersearchcount"] is DBNull ? 0 : Convert.ToInt32(r["usersearchcount"]),
                UserName = r["user_name"] is DBNull ? "(null)" : r["user_name"].ToString()
            };

        public IEnumerable<UserSearchCount> GetAll()
        {
            return RunSelect(sql, DataMapping);
        }
    }
}
