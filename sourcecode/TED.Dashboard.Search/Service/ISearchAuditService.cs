using System.Collections.Generic;
using TED.Dashboard.Search.Models;
using TED.Dashboard.Services.Models;

namespace TED.Dashboard.Search.Services
{
    public interface ISearchAuditService
    {
        /// <summary>
        /// retrieves a list of searches executed per hour
        /// </summary>
        /// <returns>collection of counts for each hour</returns>
        IEnumerable<SearchAuditCount> GetSearchCounts(int hoursCovered);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="page">page of items to get</param>
        /// <param name="count">number of items to get per page</param>
        /// <returns>list of searches executed</returns>
        PageableItem<SearchAudit> GetPagedSearches(int page, int count);

        /// <summary>
        /// Retrieves a list of top 16 filters searched on and their counts
        /// </summary>
        /// <returns>collection of search filters and times used</returns>
        IEnumerable<SearchFilterCount> GetFilterCounts();

        /// <summary>
        /// Retrieves a list of top 16 operands searched on and their counts
        /// </summary>
        /// <returns>collection of search operands and times used</returns>
        IEnumerable<SearchOperandCount> GetOperandCounts();

        /// <summary>
        /// Retrieves a list of top 16 users executing searches
        /// </summary>
        /// <returns>collection of users executing searches</returns>
        IEnumerable<UserSearchCount> GetUserSearchCounts();
    }
}
