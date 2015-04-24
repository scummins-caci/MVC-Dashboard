using System.Web.Http;
using Ninject.Syntax;
using TED.Dashboard.Dataflow.Models;
using TED.Dashboard.Dataflow.Repositories;
using TED.Dashboard.Dataflow.Services;

using TED.Dashboard.Workflow.Models;
using TED.Dashboard.Workflow.Repositories;
using TED.Dashboard.Workflow.Services;

using TED.Dashboard.Users;
using TED.Dashboard.Users.Repositories;
using TED.Dashboard.Users.Services;

using TED.Dashboard.Search.Models;
using TED.Dashboard.Search.Repositories;
using TED.Dashboard.Search.Services;
using TED.Dashboard.User.Services;
using TED.Dashboard.UserSettings.Services;
using TED.Dashboard.UserSettings.UnitOfWork;


[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(TED.Dashboard.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(TED.Dashboard.App_Start.NinjectWebCommon), "Stop")]

namespace TED.Dashboard.App_Start
{
    using System;
    using System.Web;
    using Authentication;
    using Authentication.HV5;
    using Repository;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);

                GlobalConfiguration.Configuration.DependencyResolver = new NinjectResolver(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IBindingRoot kernel)
        {
            // context mapping
            // this is already handled in the source of ninject
            //kernel.Bind<HttpContextBase>().ToMethod(ctx => new HttpContextWrapper(HttpContext.Current)).InTransientScope();
            
            // service mappings
            kernel.Bind<IAuthenticate>().To<Authenticate>();
            kernel.Bind<IDashboardService>().To<DashboardService>();
            kernel.Bind<INotificationsService>().To<NotificationsService>();
            kernel.Bind<IWorkflowStatusService>().To<WorkflowStatusService>();
            kernel.Bind<IWorkitemService>().To<WorkitemService>();
            kernel.Bind<IDataflowService>().To<DataflowService>();
            kernel.Bind<IUserInfoService>().To<UserInfoService>();
            kernel.Bind<ISearchAuditService>().To<SearchAuditService>();
            kernel.Bind<IUserSettingsService>().To<UserSettingsService>();

            // unit of work
            kernel.Bind<IUserSettingsUOW>().To<UserSettingsUOW>();

            // repository mappings
            kernel.Bind<IReadOnlyRepository<WorkflowConnector>>().To<ConnectorStatusRepository>();
            kernel.Bind<IReadOnlyRepository<ServiceHostInfo>>().To<ServiceStatusRepository>();
            kernel.Bind<IReadOnlyRepository<WorkflowHostInfo>>().To<WorkflowStatusRepository>();
            kernel.Bind<IReadOnlyRepository<WorkItemCount>>().To<WorkItemCountRepository>();
            kernel.Bind<IReadOnlyRepository<WorkItemStatusCount>>().To<StatusCountRepository>();
            kernel.Bind<IReadOnlyRepository<SearchAuditCount>>().To<SearchCountRepository>();
            kernel.Bind<IReadOnlyRepository<SearchFilterCount>>().To<FilterCountRepository>();
            kernel.Bind<IReadOnlyRepository<SearchOperandCount>>().To<OperandCountRepository>();
            kernel.Bind<IReadOnlyRepository<UserSearchCount>>().To<UserSearchCountRepository>();

            kernel.Bind<IPageableRepository<Receipt>>().To<ReceiptRepository>();
            kernel.Bind<IPageableRepository<ErrorEntry>>().To<ErrorLogRepository>();
            kernel.Bind<IPageableRepository<SearchAudit>>().To<SearchAuditRepository>();
            kernel.Bind<IPageableRepository<LogEntry>>().To<LogRepository>();

            kernel.Bind<IIDFilterRepository<MetaRouteInfo>>().To<MetaRouteRepository>();
            kernel.Bind<IIDFilterRepository<BinaryRouteInfo>>().To<BinaryRouteRepository>();
            kernel.Bind<IIDFilterRepository<ScanRouteInfo>>().To<ScanRouteRepository>();
            kernel.Bind<IIDFilterRepository<Role>>().To<UserRoleRepository>();

            kernel.Bind<IProcessInfoRepository>().To<ProcessInfoRepository>();
            kernel.Bind<IWorkItemRepository>().To<WorkItemRepository>();
            kernel.Bind<IQueueRepository>().To<QueueRepository>();
            kernel.Bind<IQueueUserRepository>().To<QueueUserRepository>();
        }        
    }
}
