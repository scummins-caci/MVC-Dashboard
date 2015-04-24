using System;
using System.Collections.Generic;
using System.Data;
using TED.Dashboard.Repository;
using TED.Dashboard.Workflow.Models;

namespace TED.Dashboard.Workflow.Repositories
{
    public class ServiceStatusRepository : HV5DataStore<ServiceHostInfo>, IReadOnlyRepository<ServiceHostInfo>
    {
        private const string baseSql = @"select hostname, tstamp 
                        from hvsvc_hosts";

        /// <summary>
        /// Mapping for converting sql results into a model object
        /// </summary>
        private readonly Func<IDataReader, ServiceHostInfo> DataMapping =
            r => new ServiceHostInfo
            {
                Name = r["hostname"] is DBNull ? "" : r["hostname"].ToString(),
                TimeStamp = r["tstamp"] is DBNull ? "" : Convert.ToDateTime(r["tstamp"]).ToDisplayString()
            };

        public IEnumerable<ServiceHostInfo> GetAll()
        {
            return RunSelect(baseSql, DataMapping);
        }
    }
}
