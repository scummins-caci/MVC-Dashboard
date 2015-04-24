using System;

namespace TED.Dashboard.Dataflow.Models
{
    public class MetaRouteInfo : MetaInfoBase
    {
        public DateTime FilesSentOn { get; set; }
        public DateTime XmlSentOn { get; set; }
        public DateTime ProcessedOn { get; set; }
        public string ProcessedStatus { get; set; }
    }
}
