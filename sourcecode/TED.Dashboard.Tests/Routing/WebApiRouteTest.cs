using System.Web.Http;
using System.Net.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvcRouteTester;
using TED.Dashboard.Api.Controllers;
using TED.Dashboard.Routes;
using Moq;

namespace TED.Dashboard.Tests.Routing
{
    [TestClass]
    public class WebApiRouteTest
    {
        private HttpConfiguration config;

        // Use TestInitialize to run code before running each test 
        [TestInitialize]
        public void Setup()
        {
            config = WebApiRoutes.RouteConfig;
        }

        #region notifications controller path

        [TestMethod]
        public void NotificationsGetRoute_Test()
        {
            config.ShouldMap("/api/notifications")
                    .To<NotificationsController>(HttpMethod.Get, x => x.Get(It.IsAny<int>(), It.IsAny<int>()));
        }

        [TestMethod]
        public void NotificationsGetRoutePage2_Test()
        {
            config.ShouldMap("/api/notifications?page=2")
                    .To<NotificationsController>(HttpMethod.Get, x => x.Get(2, It.IsAny<int>()));
        }

        [TestMethod]
        public void NotificationsGetRoutePage3_10ItemsPerPage_Test()
        {
            config.ShouldMap("/api/notifications?page=2&pageSize=10")
                    .To<NotificationsController>(HttpMethod.Get, x => x.Get(2, 10));
        }

        #endregion

        #region WorkflowStatus controller path

        [TestMethod]
        public void WorkflowStatusGetRoute_Test()
        {
            config.ShouldMap("/api/workflowstatus")
                    .To<WorkflowStatusController>(HttpMethod.Get, x => x.Get());
        }

        [TestMethod]
        public void WorkflowStatusServicesRoute_Test()
        {
            config.ShouldMap("/api/workflowstatus/services")
                    .To<WorkflowStatusController>(HttpMethod.Get, x => x.Services());
        }

        [TestMethod]
        public void WorkflowStatusServiceHostsRoute_Test()
        {
            config.ShouldMap("/api/workflowstatus/servicehosts")
                    .To<WorkflowStatusController>(HttpMethod.Get, x => x.ServiceHosts());
        }

        [TestMethod]
        public void WorkflowStatusWorkflowHostsRoute_Test()
        {
            config.ShouldMap("/api/workflowstatus/workflowhosts")
                    .To<WorkflowStatusController>(HttpMethod.Get, x => x.WorkflowHosts());
        }

        #endregion

        #region dataflow controller paths

        [TestMethod]
        public void DataflowReceiptsRoute_Test()
        {
            config.ShouldMap("/api/dataflow/receipts")
                    .To<DataflowController>(HttpMethod.Get, x => x.Receipts(It.IsAny<int>(), It.IsAny<int>()));
        }

        [TestMethod]
        public void DataflowReceiptsRoutePage2_Test()
        {
            config.ShouldMap("/api/dataflow/receipts?page=2")
                    .To<DataflowController>(HttpMethod.Get, x => x.Receipts(2, It.IsAny<int>()));
        }

        [TestMethod]
        public void DataflowReceiptsRoutePage2_PageSize10_Test()
        {
            config.ShouldMap("/api/dataflow/receipts?page=2&pageSize=10")
                    .To<DataflowController>(HttpMethod.Get, x => x.Receipts(2, 10));
        }

        [TestMethod]
        public void DataflowErrorsRoute_Test()
        {
            config.ShouldMap("/api/dataflow/errors")
                    .To<DataflowController>(HttpMethod.Get, x => x.Errors(It.IsAny<int>(), It.IsAny<int>()));
        }

        [TestMethod]
        public void DataflowReceiptsErrorsPage2_Test()
        {
            config.ShouldMap("/api/dataflow/errors?page=2")
                    .To<DataflowController>(HttpMethod.Get, x => x.Errors(2, It.IsAny<int>()));
        }

        [TestMethod]
        public void DataflowReceiptsErrorsPage2_PageSize10_Test()
        {
            config.ShouldMap("/api/dataflow/errors?page=2&pageSize=10")
                    .To<DataflowController>(HttpMethod.Get, x => x.Errors(2, 10));
        }

        [TestMethod]
        public void DataflowReceiptsChangeTracking_Test()
        {
            config.ShouldMap("/api/dataflow/changetracking/100")
                    .To<DataflowController>(HttpMethod.Get, x => x.ChangeTracking(100));
        }

        #endregion

        #region workitems controller paths

        [TestMethod]
        public void InbasketGet_Test()
        {
            config.ShouldMap("/api/workflow/queuecounts")
                    .To<WorkflowController>(HttpMethod.Get, x => x.QueueCounts());
        }

        [TestMethod]
        public void InbasketGetManualCounts_Test()
        {
            config.ShouldMap("/api/workflow/manualqueuecounts")
                    .To<WorkflowController>(HttpMethod.Get, x => x.ManualQueueCounts());
        }

        [TestMethod]
        public void InbasketGetAutomatedCounts_Test()
        {
            config.ShouldMap("/api/workflow/automatedqueuecounts")
                    .To<WorkflowController>(HttpMethod.Get, x => x.AutomatedQueueCounts());
        }

        [TestMethod]
        public void InbasketGetStatusCounts_Test()
        {
            config.ShouldMap("/api/workflow/statuscounts")
                    .To<WorkflowController>(HttpMethod.Get, x => x.StatusCounts());
        }

        
        [TestMethod]
        public void InbasketGetProcesses_Test()
        {
            config.ShouldMap("/api/workflow/processes")
                    .To<WorkflowController>(HttpMethod.Get, x => x.Processes(null));
        }

        [TestMethod]
        public void InbasketGetQueues_Test()
        {
            config.ShouldMap("/api/workflow/queues/231")
                    .To<WorkflowController>(HttpMethod.Get, x => x.Queues(231, null));
        }

        [TestMethod]
        public void InbasketGetWorkitems_Test()
        {
            config.ShouldMap("/api/workflow/workitems/231/15")
                    .To<WorkflowController>(HttpMethod.Get, x => x.Workitems(231, 15, null));
        }

        [TestMethod]
        public void InbasketGetWorkitemsQueueOnly_Test()
        {
            config.ShouldMap("/api/workflow/workitems/15")
                    .To<WorkflowController>(HttpMethod.Get, x => x.Workitems(0, 15, null));
        }

        [TestMethod]
        public void InbasketGetQueueUsers_Test()
        {
            config.ShouldMap("/api/workflow/users/1")
                .To<WorkflowController>(HttpMethod.Get, x => x.Users(1));
        }

        #endregion

        #region search audit controller paths

        [TestMethod]
        public void SearchAuditCounts_Test()
        {
            config.ShouldMap("/api/searchaudit/counts")
                    .To<SearchAuditController>(HttpMethod.Get, x => x.Counts());
        }

        [TestMethod]
        public void SearchAuditTopFilters_Test()
        {
            config.ShouldMap("/api/searchaudit/topfilters")
                    .To<SearchAuditController>(HttpMethod.Get, x => x.TopFilters());
        }

        [TestMethod]
        public void SearchAuditTopSearches_Test()
        {
            config.ShouldMap("/api/searchaudit/topsearches")
                    .To<SearchAuditController>(HttpMethod.Get, x => x.TopSearches());
        }

        [TestMethod]
        public void SearchAuditTopUserSearches_Test()
        {
            config.ShouldMap("/api/searchaudit/topusers")
                    .To<SearchAuditController>(HttpMethod.Get, x => x.TopUsers());
        }

        [TestMethod]
        public void SearchAuditGetSearches_Test()
        {
            config.ShouldMap("/api/searchaudit/searches")
                    .To<SearchAuditController>(HttpMethod.Get, x => x.Searches(It.IsAny<int>(), It.IsAny<int>()));
        }

        [TestMethod]
        public void NotificationsGetSearchesPage2_Test()
        {
            config.ShouldMap("/api/searchaudit/searches?page=2")
                    .To<SearchAuditController>(HttpMethod.Get, x => x.Searches(2, It.IsAny<int>()));
        }

        [TestMethod]
        public void NotificationsGetSearchesPage3_10ItemsPerPage_Test()
        {
            config.ShouldMap("/api/searchaudit/searches?page=2&pageSize=10")
                    .To<SearchAuditController>(HttpMethod.Get, x => x.Searches(2, 10));
        }

        #endregion

        #region manage controller paths

        [TestMethod]
        public void ManageGetWidgets_Test()
        {
            config.ShouldMap("/api/manage/widgets")
                    .To<ManageController>(HttpMethod.Get, x => x.Widgets());
        }

        [TestMethod]
        public void ManageGetDashboards_Test()
        {
            config.ShouldMap("/api/manage/dashboards")
                    .To<ManageController>(HttpMethod.Get, x => x.Dashboards());
        }

        [TestMethod]
        public void ManageGetUserSettings_Test()
        {
            config.ShouldMap("/api/manage/usersettings/5")
                    .To<ManageController>(HttpMethod.Get, x => x.UserSettings(5));
        }



        #endregion
    }
}
