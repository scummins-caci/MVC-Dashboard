using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Web.Http.OData;
using System.Web.Http.OData.Builder;
using System.Web.Http.OData.Query;
using HighView.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ODataToSql;
using TED.Dashboard.Repository;
using TED.Dashboard.Workflow.Models;
using TED.Dashboard.Workflow.Repositories;

namespace TED.Dashboard.Workflow.Tests.Inbasket
{
    [TestClass]
    public class ProcessInfoRepositoryTest
    {

        private ODataQueryContext context;

        [ClassInitialize]
        public static void MyClassInitialize(TestContext testContext)
        {
            WinModule.Initialize();
            WinModule.CreateNewSession(ConfigurationManager.AppSettings["AuthenticationConnectString"],
                                       ConfigurationManager.AppSettings["UserName"],
                                       ConfigurationManager.AppSettings["Password"]);
        }

        [TestInitialize]
        public void TestInitialize()
        {
            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<WorkflowProcessInfo>("Processes");
            context = new ODataQueryContext(builder.GetEdmModel(), typeof(WorkflowProcessInfo));
        }

        //
        // Use ClassCleanup to run code after all tests in a class have run
        [ClassCleanup]
        public static void MyClassCleanup()
        {
            WinModule.DestroySession();
        }

        [TestMethod]
        public void GetAll_Test()
        {
            var repository = new ProcessInfoRepository();

            var queues = repository.GetAll();

            Assert.IsNotNull(queues);
        }

        [TestMethod]
        public void GetFilteredByName_Test()
        {
            var repository = new ProcessInfoRepository();

            var queues = repository.GetAll(x => x.Name.Contains("Cont"));

            Assert.IsNotNull(queues);
        }

        [TestMethod]
        public void GetFilteredByNameAndVersionOrderBy_Test()
        {
            var repository = new ProcessInfoRepository();
            Expression<Func<IQueryable<WorkflowProcessInfo>,
                IOrderedQueryable<WorkflowProcessInfo>>> orderBy = o => o.OrderBy(x => x.Name);
            var queues = repository.GetAll(x => x.Name.Contains("Cont") && x.Version > 2, orderBy);

            Assert.IsNotNull(queues);
        }

        [TestMethod]
        public void GetOrderByItemsThenByName_Test()
        {
            var repository = new ProcessInfoRepository();

            Expression<Func<IQueryable<WorkflowProcessInfo>,
                IOrderedQueryable<WorkflowProcessInfo>>> orderBy = o => o.OrderBy(x => x.ActiveItems).ThenBy(x => x.Name);
            var queues = repository.GetAll(null, orderBy);

            Assert.IsNotNull(queues);
        }

        [TestMethod]
        public void ODataUri_OrderBy_Multiple_Mixed_Test()
        {
            // setup
            var repository = new ProcessInfoRepository();
            var request = new HttpRequestMessage(HttpMethod.Get, "http://www.example.com/?$filter=substringof('Cont', Name)&$orderby=Description desc, Name");
            var queryOptions = new ODataQueryOptions<WorkflowProcessInfo>(context, request);

            var filter = queryOptions.Filter.ToExpression<WorkflowProcessInfo>();
            var orderBy = queryOptions.OrderBy.ToExpression<WorkflowProcessInfo>();

            // execute
            var queues = repository.GetAll(filter, orderBy);

            // assert
            Assert.IsNotNull(queues);
        }
    }
}
