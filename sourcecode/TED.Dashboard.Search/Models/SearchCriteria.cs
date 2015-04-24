using System.Collections.Generic;

namespace TED.Dashboard.Search.Models
{
    public class SearchCriteria
    {
        public string FilterName { get; set; }
        public string Operator { get; set; }
        public IList<string> Operands { get; set; }
    }
}
