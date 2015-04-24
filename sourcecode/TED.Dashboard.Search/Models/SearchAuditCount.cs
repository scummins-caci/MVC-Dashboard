using System;

namespace TED.Dashboard.Search.Models
{
    public class SearchAuditCount
    {
        public int SearchesExecuted { get; set; }
        public DateTime DaySearchesExecuted { get; set; }
    }
}
