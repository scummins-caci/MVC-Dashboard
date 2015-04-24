using System;

namespace TED.Dashboard.Search.Models
{
    public class LogEntry
    {
        public string LogId { get; set; }
        public string ClientMachine { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Location { get; set; }
        public string Severity { get; set; }
        public string Message { get; set; }

        public int LogCount { get; set; }
    }
}
