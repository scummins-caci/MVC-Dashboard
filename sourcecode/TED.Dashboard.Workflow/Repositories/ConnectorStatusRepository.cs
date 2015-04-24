using System;
using System.Collections.Generic;
using System.Data;
using TED.Dashboard.Repository;
using TED.Dashboard.Workflow.Models;

namespace TED.Dashboard.Workflow.Repositories
{
    public class ConnectorStatusRepository : HV5DataStore<WorkflowConnector>, IReadOnlyRepository<WorkflowConnector>
    {
        private const string baseSql = @"select s.name, s.is_enabled, h.hostname,  count(i.service_id) as ""instances"" 
                        from hvsvc_services s 
                        left outer join hvsvc_instances i on s.id = i.service_id
                        left outer join hvsvc_hosts h on h.id = i.host_id 
                        group by s.name, s.is_enabled, h.hostname
                        order by s.name";

        /// <summary>
        /// Mapping for converting sql results into a model object
        /// </summary>
        private readonly Func<IDataReader, WorkflowConnector> DataMapping =
            r => new WorkflowConnector
            {
                Instances = r["instances"] is DBNull ? 0 : Convert.ToInt32(r["instances"]),
                Name = r["name"] is DBNull ? "" : r["name"].ToString(),
                HostName = r["hostname"] is DBNull ? "" : r["hostname"].ToString(),
                IsEnabled = !(r["is_enabled"] is DBNull) && Convert.ToBoolean(r["is_enabled"])
            };

        public IEnumerable<WorkflowConnector> GetAll()
        {
            // sql to get connector logs from database
            return RunSelect(baseSql, DataMapping);
        }
    }
}
