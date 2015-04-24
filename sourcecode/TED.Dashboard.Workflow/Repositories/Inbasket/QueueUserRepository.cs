using System;
using System.Collections.Generic;
using System.Data;
using TED.Dashboard.Repository;
using TED.Dashboard.Workflow.Models;

namespace TED.Dashboard.Workflow.Repositories
{
    public class QueueUserRepository : HV5DataStore<QueueUser>, IQueueUserRepository
    {
        private const string sql = @"SELECT DISTINCT hau.user_id, hau.login_name 
                                        FROM hvsec_authorized_users  hau
                                        INNER JOIN hvsec_group_users hgu ON hau.user_id = hgu.user_id
                                        INNER JOIN hvwf_queues hvwq ON hgu.group_id = hvwq.queue_admin OR hgu.group_id = hvwq.queue_user
                                        WHERE hvwq.id = :queue
                                        ORDER BY UPPER(hau.login_name) ASC";

        private readonly Func<IDataReader, QueueUser> DataMapping =
            r => new QueueUser
            {
                ID = r["user_id"] is DBNull ? 0 : Convert.ToUInt64(r["user_id"]),
                UserName = r["login_name"] is DBNull ? "" : r["login_name"].ToString()
            };

        public IEnumerable<QueueUser> GetQueueUsers(ulong queueID)
        {
            var parameters = new Dictionary<string, string> { { "queue", queueID.ToString() } };
            return RunSelect(sql, DataMapping, parameters);
        }
    }
}
