using System;

namespace TED.Dashboard.Dataflow.Models
{
    public abstract class FileInfoBase
    {
        public string Id { get; set; }
        public string DestinationId { get; set; }
        public string PathInfo { get; set; }
        public DateTime FileInfoSentOn { get; set; }
        public DateTime XmlSentOn { get; set; }
        public DateTime ProcessedOn { get; set; }
        public string ProcessedStatus { get; set; }
    }
}
