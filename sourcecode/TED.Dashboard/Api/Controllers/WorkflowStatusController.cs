using System.Net;
using System.Web.Http;
using TED.Dashboard.Api.Models;
using TED.Dashboard.Authentication.Authorization;
using TED.Dashboard.Users;
using TED.Dashboard.Workflow.Services;

namespace TED.Dashboard.Api.Controllers
{
    [VerifyDataAuthorization(Roles = ApplicationRole.WorkflowAdmin)]
    public class WorkflowStatusController : ApiController
    {
        private readonly IWorkflowStatusService dataService;
        public WorkflowStatusController(IWorkflowStatusService dataService)
        {
            this.dataService = dataService;
        }

        /// <summary>
        /// Gets the status information for workflow host, service host, and
        /// automated services
        /// </summary>
        /// <returns></returns>
        public ApiReturnDataModel Get()
        {         
            // retrieve data
            var model = new WorkflowServicesDataModel
            {
                WorkflowConnectors = dataService.GetConnectors(),
                ServiceHosts = dataService.GetServiceHosts(),
                WorkflowHosts = dataService.GetWorkflowHosts(),
                ProcessInfo = dataService.GetProcessInfo()
            };
            return new ApiReturnDataModel(model, (int)HttpStatusCode.OK, true);
        }

        /// <summary>
        /// Gets the service information only
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ApiReturnDataModel Services()
        {
            var items = dataService.GetConnectors();
            return new ApiReturnDataModel(items, (int)HttpStatusCode.OK, true);
        }

        /// <summary>
        /// Get the service host information only
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ApiReturnDataModel ServiceHosts()
        {
            var items = dataService.GetServiceHosts();
            return new ApiReturnDataModel(items, (int)HttpStatusCode.OK, true);
        }

        /// <summary>
        /// Gets the workflow host information only
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ApiReturnDataModel WorkflowHosts()
        {
            var items = dataService.GetWorkflowHosts();
            return new ApiReturnDataModel(items, (int)HttpStatusCode.OK, true);
        }

        /// <summary>
        /// Gets the process info only
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ApiReturnDataModel ProcessInfo()
        {
            var items = dataService.GetProcessInfo();
            return new ApiReturnDataModel(items, (int)HttpStatusCode.OK, true);
        }
    }
}
