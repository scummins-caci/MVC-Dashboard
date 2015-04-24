using System.Collections.Generic;
using TED.Dashboard.Repository;
using System.Linq;
using TED.Dashboard.Search.Models;
using TED.Dashboard.Services.Models;

namespace TED.Dashboard.Search.Services
{
    public class SearchAuditService : ISearchAuditService
    {
        private readonly IReadOnlyRepository<SearchAuditCount> statsRepository;
        private readonly IPageableRepository<SearchAudit> auditRepository;
        private readonly IReadOnlyRepository<SearchFilterCount> filterRepository;
        private readonly IReadOnlyRepository<SearchOperandCount> operandRepository;
        private readonly IReadOnlyRepository<UserSearchCount> userSearchRepository;

        public SearchAuditService(IReadOnlyRepository<SearchAuditCount> statsRepository,
                                    IReadOnlyRepository<SearchFilterCount> filterRepository,
                                    IReadOnlyRepository<SearchOperandCount> operandRepository,
                                    IReadOnlyRepository<UserSearchCount> userSearchRepository, 
                                    IPageableRepository<SearchAudit> auditRepository)
        {
            this.statsRepository = statsRepository;
            this.auditRepository = auditRepository;
            this.filterRepository = filterRepository;
            this.operandRepository = operandRepository;
            this.userSearchRepository = userSearchRepository;
        }

        /// <summary>
        /// retrieves a list of searches executed per hour
        /// </summary>
        /// <returns>collection of counts for each hour</returns>
        public IEnumerable<SearchAuditCount> GetSearchCounts(int daysCovered)
        {
            // retrieve search counts from audit 
            var counts = statsRepository.GetAll();

            // return full collection with all hours filled in
            return FillInHourGaps(counts, daysCovered).OrderBy(x => x.DaySearchesExecuted);
        }

        /// <summary>
        /// Retrieves a list of top 16 filters searched on and their counts
        /// </summary>
        /// <returns>collection of search filters and times used</returns>
        public IEnumerable<SearchFilterCount> GetFilterCounts()
        {
            return filterRepository.GetAll();
        }

        /// <summary>
        /// Retrieves a list of top 16 operands searched on and their counts
        /// </summary>
        /// <returns>collection of search operands and times used</returns>
        public IEnumerable<SearchOperandCount> GetOperandCounts()
        {
            return operandRepository.GetAll();
        }

        /// <summary>
        /// Retrieves a list of top 16 users executing searches
        /// </summary>
        /// <returns>collection of users executing searches</returns>
        public IEnumerable<UserSearchCount> GetUserSearchCounts()
        {
            return userSearchRepository.GetAll();
        }

        /// <summary>
        /// Gets a list of searches that were executed and info about that search
        /// </summary>
        /// <param name="page">page of items to get</param>
        /// <param name="count"># of items per pgae</param>
        /// <returns>collection of searches executed</returns>
        public PageableItem<SearchAudit> GetPagedSearches(int page, int count)
        {
            var searches = auditRepository.GetPage(page, count).ToArray();
            return new PageableItem<SearchAudit>
                {
                    ItemCount = searches.Any() ? searches.First().SearchCount : 0,
                    Items = searches
                };
        }


        #region private methods

        /// <summary>
        /// loops through the counts;  if any hour is missing, add a count of 0 for that hour.
        /// For graphing purposes
        /// </summary>
        /// <param name="retrievedCounts">counts retrieved from audit</param>
        /// <param name="daysCovered">How many days of results that should be returned</param>
        /// <returns></returns>
        private static IEnumerable<SearchAuditCount> FillInHourGaps(IEnumerable<SearchAuditCount> retrievedCounts, int daysCovered)
        {
            var searchCounts = new List<SearchAuditCount>();

            var searchAuditCounts = retrievedCounts.ToArray();
            var previousCount = searchAuditCounts.OrderByDescending(x => x.DaySearchesExecuted).FirstOrDefault();
            
            // check if any items were retrieved;  if not, return nothing
            if (previousCount == null)
            {
                return searchCounts;
            }

            // scrub through results;  if any hours aren't represented, fill in an item for the missing hour
            foreach (var currentCount in searchAuditCounts.OrderByDescending(x => x.DaySearchesExecuted))
            {
                // make sure the previous item is only one hour ahead the current item
                while (previousCount.DaySearchesExecuted.AddDays(-1)
                            .CompareTo(currentCount.DaySearchesExecuted) == 1)
                {
                    var newCount = new SearchAuditCount
                    {
                        SearchesExecuted = 0,
                        DaySearchesExecuted = previousCount.DaySearchesExecuted.AddDays(-1)
                    };

                    searchCounts.Add(newCount);
                    previousCount = newCount;
                }


                // filled in the gaps;  add current item and continue
                searchCounts.Add(currentCount);
                previousCount = currentCount;
            }

            // fill in any gaps past the amount of hours we want to cover
            while (searchCounts.Count < daysCovered)
            {
                var newCount = new SearchAuditCount
                {
                    SearchesExecuted = 0,
                    DaySearchesExecuted = previousCount.DaySearchesExecuted.AddDays(-1)
                };

                searchCounts.Add(newCount);
                previousCount = newCount;
            }
            return searchCounts;
        }

        #endregion

    }
}
