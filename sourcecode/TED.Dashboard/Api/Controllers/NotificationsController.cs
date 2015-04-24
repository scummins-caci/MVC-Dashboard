using System.Linq;
using System.Net;
using System.Web.Http;
using TED.Dashboard.Api.Models;
using TED.Dashboard.Authentication.Authorization;
using TED.Dashboard.Search.Services;
using TED.Dashboard.Users;

namespace TED.Dashboard.Api.Controllers
{
    [VerifyDataAuthorization(Roles = ApplicationRole.WorkflowAdmin)]
    public class NotificationsController : ApiController
    {
        private readonly INotificationsService dataService;
        public NotificationsController(INotificationsService dataService)
        {
            this.dataService = dataService;
        }
        
        /// <summary>
        /// Get the full collection of log items of all types
        /// </summary>
        /// <returns></returns>
        public ApiReturnDataModel Get(int page = 1, int pageSize = 10)
        {
            var data = dataService.GetPagedLogEntries(page, pageSize);

            var dataModel = new PageableItemDataModel<LogEntryDataModel>
                {
                    ItemCount = data.ItemCount,
                    Items = data.Items.Select(LogEntryDataModel.BuildFromLogEntry)
                };

            return new ApiReturnDataModel(dataModel, (int)HttpStatusCode.OK, true);
        }
    }
}
