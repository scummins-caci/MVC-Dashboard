using System;
using System.Collections.Generic;
using System.Data;
using TED.Dashboard.Repository;
using TED.Dashboard.Workflow.Models;

namespace TED.Dashboard.Workflow.Repositories
{
    public class WorkflowStatusRepository : HV5DataStore<WorkflowHostInfo>, IReadOnlyRepository<WorkflowHostInfo>
    {
        private const string baseSql = @"select hostname, tstamp 
                        from hvwf_hosts";

        /// <summary>
        /// Mapping for converting sql results into a model object
        /// </summary>
        private readonly Func<IDataReader, WorkflowHostInfo> DataMapping =
            r => new WorkflowHostInfo
            {
                Name = r["hostname"] is DBNull ? "" : r["hostname"].ToString(),
                TimeStamp = r["tstamp"] is DBNull ? "" : Convert.ToDateTime(r["tstamp"]).ToDisplayString()
            };
        
        public IEnumerable<WorkflowHostInfo> GetAll()
        {
            // sql to get connector logs from database
            return RunSelect(baseSql, DataMapping);
        }
    }
}
