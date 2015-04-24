using System;
using System.Collections.Generic;
using System.Data;
using TED.Dashboard.Dataflow.Models;
using TED.Dashboard.Repository;

namespace TED.Dashboard.Dataflow.Repositories
{
    public class BinaryRouteRepository : DataflowDataStore<BinaryRouteInfo>, IIDFilterRepository<BinaryRouteInfo>
    {
        private const string baseSql = @"select  RAWTOHEX(dbr.docbin_id) as id, 
                                                RAWTOHEX(dbr.dest_id) as dest_id,
                                                dbr.file_info_sent_on,
                                                dbr.xml_sent_on,
                                                dbr.target_processed_on,
                                                dbr.target_processed_status,
                                                dbc.pathinfo
                                             from dflow_docbin_route dbr
                                             join dflow_dm_ch_bin dbc
                                             on (dbc.docbin_id = dbr.docbin_id)
                                             where dbr.docmeta_change_id in (
                                                  select dbc.docmeta_change_id
                                                  from dflow_dm_ch_bin dbc
                                                  join dflow_docmeta_change dmc
                                                       on (dbc.docmeta_change_id = dmc.id)
                                                  where dmc.rec_change_id = :change_id
                                                  )
                                              order by dbr.dest_id";

        /// <summary>
        /// Mapping for converting sql results into a model object
        /// </summary>
        private readonly Func<IDataReader, BinaryRouteInfo> DataMapping =
            r => new BinaryRouteInfo
            {
                Id = r["id"] is DBNull ? "" : r["id"].ToString(),
                DestinationId = r["dest_id"] is DBNull ? "" : r["dest_id"].ToString(),
                FileInfoSentOn = r["file_info_sent_on"] is DBNull ? DateTime.MinValue : Convert.ToDateTime(r["file_info_sent_on"]),
                XmlSentOn = r["xml_sent_on"] is DBNull ? DateTime.MinValue : Convert.ToDateTime(r["xml_sent_on"]),
                ProcessedOn = r["target_processed_on"] is DBNull ? DateTime.MinValue : Convert.ToDateTime(r["target_processed_on"]),
                ProcessedStatus = r["target_processed_status"] is DBNull ? "" : r["target_processed_status"].ToString(),
                PathInfo = r["pathinfo"] is DBNull ? "" : r["pathinfo"].ToString()
            };


        public IEnumerable<BinaryRouteInfo> GetAll(long id)
        {
            var parameters = new Dictionary<string, string> { { "change_id", id.ToString() } };
            return RunSelect(baseSql, DataMapping, parameters);
        }
    }
}
