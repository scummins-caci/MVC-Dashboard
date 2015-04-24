using System;
using System.Collections.Generic;
using System.Data;
using TED.Dashboard.Dataflow.Models;
using TED.Dashboard.Repository;

namespace TED.Dashboard.Dataflow.Repositories
{
    public class ScanRouteRepository : DataflowDataStore<ScanRouteInfo>, IIDFilterRepository<ScanRouteInfo>
    {
        private const string baseSql = @"select  RAWTOHEX(dsr.docscan_id) as id, 
                                                RAWTOHEX(dsr.dest_id) as dest_id,
                                                dsr.file_info_sent_on,
                                                dsr.xml_sent_on,
                                                dsr.target_processed_on,
                                                dsr.target_processed_status,
                                                dbc.pathinfo
                                        from dflow_docscan_route dsr
                                         join dflow_dm_ch_bin dbc
                                          on (dbc.docmeta_change_id = dsr.docmeta_change_id)
                                        where dsr.docmeta_change_id in (
                                            select dsc.docmeta_change_id
                                            from dflow_dm_ch_scan dsc
                                            join dflow_docmeta_change dmc
                                                 on (dsc.docmeta_change_id = dmc.id)
                                            where dmc.rec_change_id = :change_id
                                            )
                                        order by dsr.dest_id";

        /// <summary>
        /// Mapping for converting sql results into a model object
        /// </summary>
        private readonly Func<IDataReader, ScanRouteInfo> DataMapping =
            r => new ScanRouteInfo
            {
                Id = r["id"] is DBNull ? "" : r["id"].ToString(),
                DestinationId = r["dest_id"] is DBNull ? "" : r["dest_id"].ToString(),
                FileInfoSentOn = r["file_info_sent_on"] is DBNull ? DateTime.MinValue : Convert.ToDateTime(r["file_info_sent_on"]),
                XmlSentOn = r["xml_sent_on"] is DBNull ? DateTime.MinValue : Convert.ToDateTime(r["xml_sent_on"]),
                ProcessedOn = r["target_processed_on"] is DBNull ? DateTime.MinValue : Convert.ToDateTime(r["target_processed_on"]),
                ProcessedStatus = r["target_processed_status"] is DBNull ? "" : r["target_processed_status"].ToString(),
                PathInfo = r["pathinfo"] is DBNull ? "" : r["pathinfo"].ToString()
            };


        public IEnumerable<ScanRouteInfo> GetAll(long id)
        {
            var parameters = new Dictionary<string, string> { { "change_id", id.ToString() } };
            return RunSelect(baseSql, DataMapping, parameters);
        }
    }
}
