using System.Collections.Generic;
using TED.Dashboard.Search.Models;

namespace TED.Dashboard.Api.Models
{
    public class SearchStatisticsDataModel
    {
        public IEnumerable<SearchFilterCount> FilterStats { get; set; }
        public IEnumerable<SearchOperandCount> OperandStats { get; set; }
        public IEnumerable<UserSearchCount> UserStats { get; set; }
    }
}