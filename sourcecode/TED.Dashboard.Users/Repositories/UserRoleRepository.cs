using System;
using System.Collections.Generic;
using System.Data;
using TED.Dashboard.Repository;

namespace TED.Dashboard.Users.Repositories
{
    public class UserRoleRepository : HV5DataStore<Role>, IIDFilterRepository<Role>
    {
        private const string baseSql = @"select rolename, rolevalue
                                        from (
                                              select 'IsSysAdmin' rolename, issysadmin rolevalue, user_id
                                              from v_rpt_sec_user_permissions
                                              union all
                                              select 'IsWfQueueAdmin' rolename, iswfqueueadmin rolevalue, user_id
                                              from v_rpt_sec_user_permissions
                                              union all
                                              select 'CanSearchExternal' rolename, cansearchexternal rolevalue, user_id
                                              from v_rpt_sec_user_permissions
                                        ) 
                                        where user_id = :user_id";

        /// <summary>
        /// Mapping for converting sql results into a model object
        /// </summary>
        private readonly Func<IDataReader, Role> DataMapping =
            r => new Role
            {
                RoleName = r["rolename"] is DBNull ? "" : r["rolename"].ToString(),
                Value = !(r["rolevalue"] is DBNull) && (r["rolevalue"].ToString() == "1")
            };

        public IEnumerable<Role> GetAll(long id)
        {
            var parameters = new Dictionary<string, string> { { "user_id", id.ToString() } };
            return RunSelect(baseSql, DataMapping, parameters);
        }
    }
}
