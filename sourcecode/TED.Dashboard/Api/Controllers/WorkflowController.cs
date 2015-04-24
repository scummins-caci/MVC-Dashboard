using ODataToSql;
using System.Net;
using System.Linq;
using System.Web.Http;
using System.Web.Http.OData.Query;
using TED.Dashboard.Api.Models;
using TED.Dashboard.Authentication.Authorization;
using TED.Dashboard.Users;
using TED.Dashboard.Workflow.Services;
using TED.Dashboard.Workflow.Models;

namespace TED.Dashboard.Api.Controllers
{
    [VerifyDataAuthorization(Roles = ApplicationRole.WorkflowQueueAdmin)]
    public class WorkflowController : ApiController
    {
        private readonly IWorkitemService dataService;
        public WorkflowController(IWorkitemService dataService)
        {
            this.dataService = dataService;
        }

        #region workitem statistics

        /// <summary>
        /// Gets the counts of workitems per queue
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ApiReturnDataModel QueueCounts()
        {
            var counts = dataService.GetQueueCounts();
            return new ApiReturnDataModel(counts, (int)HttpStatusCode.OK, true);
        }

        /// <summary>
        /// Gets the counts of workitems in manual queues
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ApiReturnDataModel ManualQueueCounts()
        {
            var counts = dataService.GetManualQueueCounts();
            return new ApiReturnDataModel(counts, (int)HttpStatusCode.OK, true);
        }

        /// <summary>
        /// Get the counts of workitems in automated activities
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ApiReturnDataModel AutomatedQueueCounts()
        {
            var counts = dataService.GetAutomatedQueueCounts();
            return new ApiReturnDataModel(counts, (int)HttpStatusCode.OK, true);
        }

        /// <summary>
        /// Gets the counts for workitems in the different statuses (failed, ready, etc.)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ApiReturnDataModel StatusCounts()
        {
            var counts = dataService.GetStatusCounts();
            return new ApiReturnDataModel(counts, (int)HttpStatusCode.OK, true);
        }

        #endregion

        #region process/queue/workitem 'inbasket' data

        /// <summary>
        /// Gets workflow process information;  exposes OData query options
        /// </summary>
        /// <param name="queryOptions">OData options passed in query string</param>
        /// <returns>collection of processes</returns>
        [HttpGet]
        public ApiReturnDataModel Processes(ODataQueryOptions<WorkflowProcessInfo> queryOptions)
        {
            var filter = queryOptions.Filter.ToExpression<WorkflowProcessInfo>();
            var orderBy = queryOptions.OrderBy.ToExpression<WorkflowProcessInfo>();

            var processes = dataService.GetProcesses(queryOptions.PageNumber(), queryOptions.PageSize(), filter, orderBy);

             var dataModel = new PageableItemDataModel<WorkflowProcessDataModel>
                {
                    ItemCount = processes.ItemCount,
                    Items = processes.Items.Select(WorkflowProcessDataModel.BuildFromWorkflowProcessInfo)
                };

             return new ApiReturnDataModel(dataModel, (int)HttpStatusCode.OK, true);
        }
        
        /// <summary>
        /// returns workflow process information and includes administrative info
        /// </summary>
        /// <param name="queryOptions"></param>
        /// <returns></returns>
        [HttpGet]
        public ApiReturnDataModel AdminProcesses(ODataQueryOptions<WorkflowProcessInfo> queryOptions)
        {
            var filter = queryOptions.Filter.ToExpression<WorkflowProcessInfo>();
            var orderBy = queryOptions.OrderBy.ToExpression<WorkflowProcessInfo>();

            var processes = dataService.GetProcesses(queryOptions.PageNumber(), queryOptions.PageSize(), filter, orderBy, true);

            var dataModel = new PageableItemDataModel<WorkflowProcessInfo>
            {
                ItemCount = processes.ItemCount,
                Items = processes.Items
            };

            return new ApiReturnDataModel(dataModel, (int)HttpStatusCode.OK, true);
        }

        /// <summary>
        /// Get a list of queues in a given process
        /// </summary>
        /// <param name="processId">process to get queues from</param>
        /// <param name="queryOptions">odata filter and order by options</param>
        /// <returns></returns>
        [HttpGet]
        public ApiReturnDataModel Queues(ulong processId, ODataQueryOptions<WorkflowQueue> queryOptions)
        {
            var filter = queryOptions.Filter.ToExpression<WorkflowQueue>();
            var orderBy = queryOptions.OrderBy.ToExpression<WorkflowQueue>();

            var queues = dataService.GetQueues(processId, queryOptions.PageNumber(), queryOptions.PageSize(), filter, orderBy);

            var dataModel = new PageableItemDataModel<WorkflowQueue>
            {
                ItemCount = queues.ItemCount,
                Items = queues.Items
            };

            return new ApiReturnDataModel(dataModel, (int)HttpStatusCode.OK, true);
        }

        /// <summary>
        /// Gets a list of workitems for a given queue
        /// </summary>
        /// <param name="processId">process that the queue is in</param>
        /// <param name="queueId">queues to get workitems for</param>
        /// <param name="queryOptions">odata filter and order by options</param>
        /// <returns></returns>
        [HttpGet]
        public ApiReturnDataModel Workitems(ulong processId, ulong queueId, ODataQueryOptions<WorkItem> queryOptions)
        {
            var filter = queryOptions.Filter.ToExpression<WorkItem>();
            var orderBy = queryOptions.OrderBy.ToExpression<WorkItem>();

            var queues = dataService.GetWorkItems(queueId, processId, queryOptions.PageNumber(), queryOptions.PageSize(), filter, orderBy);

            var dataModel = new PageableItemDataModel<WorkItem>
            {
                ItemCount = queues.ItemCount,
                Items = queues.Items
            };

            return new ApiReturnDataModel(dataModel, (int)HttpStatusCode.OK, true);
        }

        /// <summary>
        /// Gets a list of users that have access to a given queue
        /// </summary>
        /// <param name="queueId">queue to get users for</param>
        /// <returns></returns>
        [HttpGet]
        public ApiReturnDataModel Users(ulong queueId)
        {
            var users = dataService.GetQueueUsers(queueId);
            return new ApiReturnDataModel(users, (int)HttpStatusCode.OK, true);
        }

        #endregion
    }
}
