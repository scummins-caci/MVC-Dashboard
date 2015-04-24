using System.Web.Mvc;
using Omu.ValueInjecter;
using TED.Dashboard.Authentication.Authorization;
using TED.Dashboard.Models;
using TED.Dashboard.User.Services;
using TED.Dashboard.Users;


namespace TED.Dashboard.Controllers
{
    public class DashboardController : BaseController
    {
        private readonly IDashboardService dataService;
        public DashboardController(IDashboardService service)
        {
            dataService = service;
        }

        #region Dashboard display views

        // GET: /Dashboard/
        [VerifyAuthorization(Roles = ApplicationRole.WorkflowAdmin)]
        public ActionResult Index()
        {
            // TODO:  this is where we can configure what is displayed in the dash and what isn't
            var model = new WorkflowDashboardViewModel
                {
                    ShowWorkflowServicesInfo = true,
                    ShowWorkflowLogsInfo = true,
                    ShowWorkflowChartInfo = true
                };

            return View(model);
        }

        [VerifyAuthorization(Roles = ApplicationRole.WorkflowQueueAdmin)]
        public ActionResult Inbasket()
        {
            // TODO:  this is where we can configure what is displayed in the dash and what isn't
            var model = new InbasketViewModel
            {
                ShowInbasketWorkItemCounts = true
            };
            return View(model);
        }

        [VerifyAuthorization(Roles = ApplicationRole.DataflowAdmin)]
        public ActionResult Dataflow()
        {
            return View();
        }

        [VerifyAuthorization(Roles = ApplicationRole.SearchAdmin)]
        public ActionResult Search()
        {
            return View();
        }

        /// <summary>
        /// A custom dashboard with user selected widgets
        /// </summary>
        /// <param name="id">name of the custom dashboard</param>
        /// <returns></returns>
        public ActionResult Custom(string id)
        {
            var model = dataService.GetDashboardByName(id);

            // convert to view model
            var viewModel = new CustomDashboardViewModel();
            viewModel.InjectFrom(model);

            return View(viewModel);
        }

        #endregion

        #region Dashboard Management views

        [VerifyAuthorization(Roles = ApplicationRole.ManageDashboards)]
        public ActionResult Manage()
        {
            return View();
        }

        #endregion

    }
}
