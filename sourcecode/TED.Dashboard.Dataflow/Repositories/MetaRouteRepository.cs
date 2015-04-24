using System;
using System.Collections.Generic;
using System.Data;
using TED.Dashboard.Dataflow.Models;
using TED.Dashboard.Repository;

namespace TED.Dashboard.Dataflow.Repositories
{
    public class MetaRouteRepository : DataflowDataStore<MetaRouteInfo>, IIDFilterRepository<MetaRouteInfo>
    {
        private const string baseSql = @"select id, docmeta_id, harmony_number, change_id, routed_on, network, files_sent_on, xml_sent_on, target_processed_on, target_processed_status
	                                    from (	
                                            select RAWTOHEX(c.id) as id, RAWTOHEX(c.docmeta_id) as docmeta_id, c.trackingnumber as harmony_number, c.rec_change_id as change_id,
                                                c.routed_on, c.network, r.file_info_sent_on as files_sent_on,
                                                r.xml_sent_on, r.target_processed_on, r.target_processed_status
                                            from dflow_docmeta_change c 
                                                join dflow_docmeta_route r
                                                    on (c.id = r.docmeta_change_id)
                                            where rec_change_id = :change_id
	                                    order by c.routed_on desc 
	                                    )";

        /// <summary>
        /// Mapping for converting sql results into a model object
        /// </summary>
        private readonly Func<IDataReader, MetaRouteInfo> DataMapping =
            r => new MetaRouteInfo
            {
                Id = r["id"] is DBNull ? "" : r["id"].ToString(),
                ChangeId = r["change_id"] is DBNull ? 0 : Convert.ToInt64(r["change_id"]),
                HarmonyNumber = r["harmony_number"] is DBNull ? "" : r["harmony_number"].ToString(),
                MetaId = r["docmeta_id"] is DBNull ? "" : r["docmeta_id"].ToString(),
                Network = r["network"] is DBNull ? "" : r["network"].ToString(),
                FilesSentOn = r["files_sent_on"] is DBNull ? DateTime.MinValue : Convert.ToDateTime(r["files_sent_on"]),
                XmlSentOn = r["xml_sent_on"] is DBNull ? DateTime.MinValue : Convert.ToDateTime(r["xml_sent_on"]),
                ProcessedOn = r["target_processed_on"] is DBNull ? DateTime.MinValue : Convert.ToDateTime(r["target_processed_on"]),
                RoutedOn = r["routed_on"] is DBNull ? DateTime.MinValue : Convert.ToDateTime(r["routed_on"]),
                ProcessedStatus = r["target_processed_status"] is DBNull ? "" : r["target_processed_status"].ToString()
            };


        public IEnumerable<MetaRouteInfo> GetAll(long id)
        {
            var parameters = new Dictionary<string, string> { { "change_id", id.ToString() } };
            return RunSelect(baseSql, DataMapping, parameters);
        }
    }
}
