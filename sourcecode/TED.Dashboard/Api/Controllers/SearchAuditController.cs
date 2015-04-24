using System.Net;
using System.Web.Http;
using TED.Dashboard.Api.Models;
using TED.Dashboard.Authentication.Authorization;
using System.Linq;
using TED.Dashboard.Search.Services;
using TED.Dashboard.Users;
using WebApi.OutputCache.V2;

namespace TED.Dashboard.Api.Controllers
{
    [VerifyDataAuthorization(Roles = ApplicationRole.SearchAdmin)]
    public class SearchAuditController : ApiController
    {
        private readonly ISearchAuditService auditService;
        public SearchAuditController(ISearchAuditService auditService)
        {
            this.auditService = auditService;
        }

        /// <summary>
        /// Gets the counts of searches executed per hour
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ApiReturnDataModel Counts()
        {
            var counts = auditService.GetSearchCounts(15);
            return new ApiReturnDataModel(counts.Select(SearchAuditCountDataModel.BuildFromLogEntry), 
                                            (int)HttpStatusCode.OK, true);
        }

        [HttpGet, CacheOutput(ClientTimeSpan = 3600, ServerTimeSpan = 3600)] //cache 1 hour
        public ApiReturnDataModel Statistics()
        {
            var dataModel = new SearchStatisticsDataModel
            {
                FilterStats = auditService.GetFilterCounts(),
                OperandStats = auditService.GetOperandCounts(),
                UserStats = auditService.GetUserSearchCounts()
            };
            return new ApiReturnDataModel(dataModel, (int)HttpStatusCode.OK, true);
        }

        /// <summary>
        /// Gets the top filters searched
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ApiReturnDataModel TopFilters()
        {
            var filters = auditService.GetFilterCounts();
            return new ApiReturnDataModel(filters, (int)HttpStatusCode.OK, true);
        }

        /// <summary>
        /// Gets the top search operands searched on
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ApiReturnDataModel TopSearches()
        {
            var searches = auditService.GetOperandCounts();
            return new ApiReturnDataModel(searches, (int)HttpStatusCode.OK, true);
        }

        /// <summary>
        /// Gets the top users that have executed searches
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ApiReturnDataModel TopUsers()
        {
            var users = auditService.GetUserSearchCounts();
            return new ApiReturnDataModel(users, (int)HttpStatusCode.OK, true);
        }

        /// <summary>
        /// Gets a collection of executed searches to page through
        /// </summary>
        /// <returns>search information</returns>
        [HttpGet]
        public ApiReturnDataModel Searches(int page = 1, int pageSize = 20)
        {
            var data = auditService.GetPagedSearches(page, pageSize);

            var dataModel = new PageableItemDataModel<SearchAuditDataModel>
                {
                    ItemCount = data.ItemCount,
                    Items = data.Items.Select(SearchAuditDataModel.BuildFromSearchAudit)
                };

            return new ApiReturnDataModel(dataModel, (int)HttpStatusCode.OK, true);
        }
    }
}
