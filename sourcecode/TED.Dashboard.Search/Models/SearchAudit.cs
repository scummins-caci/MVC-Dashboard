using System;
using System.Collections.Generic;

namespace TED.Dashboard.Search.Models
{
    public class SearchAudit
    {
        public string UserName { get; set; }
        public DateTime DateExecuted { get; set; }
        public IList<SearchCriteria> Criteria { get; set; }

        public int SearchCount { get; set; }
    }
}
