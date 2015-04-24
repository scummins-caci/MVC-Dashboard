using System.Linq;
using System.Net;
using System.Web.Http;
using TED.Dashboard.Api.Models;
using TED.Dashboard.Authentication.Authorization;
using TED.Dashboard.Dataflow.Services;
using TED.Dashboard.Dataflow.Models;
using TED.Dashboard.Users;

namespace TED.Dashboard.Api.Controllers
{
    [VerifyDataAuthorization(Roles = ApplicationRole.DataflowAdmin)]
    public class DataflowController : ApiController
    {
        private readonly IDataflowService dataService;
        public DataflowController(IDataflowService dataService)
        {
            this.dataService = dataService;
        }

        /// <summary>
        /// Gets a list of dataflow receipts from the system
        /// </summary>
        /// <returns>collection of receipts</returns>
        [HttpGet]
        public ApiReturnDataModel Receipts(int page = 1, int pageSize = 10)
        {
            // get receipt data
            var data = dataService.GetReceipts(page, pageSize);

            // convert data to data model object
            var dataModel = new PageableItemDataModel<ReceiptDataModel>
                {
                    ItemCount = data.ItemCount,
                    Items = data.Items.Select(ReceiptDataModel.BuildFromLogEntry)
                };

            return new ApiReturnDataModel(dataModel, (int)HttpStatusCode.OK, true);
        }

        [HttpGet]
        public ApiReturnDataModel Errors(int page = 1, int pageSize = 10)
        {
            var data = dataService.GetDataflowErrors(page, pageSize);
            
            // convert data to data model object
            var dataModel = new PageableItemDataModel<ErrorEntry>
            {
                ItemCount = data.ItemCount,
                Items = data.Items
            };

            return new ApiReturnDataModel(dataModel, (int)HttpStatusCode.OK, true);
        }

        /// <summary>
        /// gets the change info from one of the receipt change ids
        /// </summary>
        /// <param name="id">receipt id to get info for</param>
        /// <returns></returns>
        [HttpGet]
        public ApiReturnDataModel ChangeTracking(int id)
        {
            var changeInfo = dataService.GetChangeInformation(id);
            return new ApiReturnDataModel(changeInfo, (int)HttpStatusCode.OK, true);
        }
    }
}
