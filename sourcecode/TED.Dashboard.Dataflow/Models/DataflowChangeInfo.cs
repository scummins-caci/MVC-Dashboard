using System.Collections.Generic;

namespace TED.Dashboard.Dataflow.Models
{
    public class DataflowChangeInfo
    {
        public IEnumerable<MetaRouteInfo> DocumentMetaRoutes { get; set; }
        public IEnumerable<BinaryRouteInfo> BinaryRoutes { get; set; }
        public IEnumerable<ScanRouteInfo> ScanRoutes { get; set; }
    }
}
