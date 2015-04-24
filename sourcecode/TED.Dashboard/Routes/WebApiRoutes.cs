using System.Web.Http;
using System.Web.Http.Routing;

namespace TED.Dashboard.Routes
{
    public class WebApiRoutes
    {
        public static string UrlPrefix { get { return "api"; } }
        public static string UrlPrefixRelative { get { return "~/api"; } }

        private static HttpConfiguration routeConfig;
        public static HttpConfiguration RouteConfig
        {
            get
            {
                if (routeConfig == null)
                {                    
                    routeConfig = new HttpConfiguration();

                    routeConfig.Routes.MapHttpRoute(
                        name: "WorkitemsNoProcess",
                        routeTemplate: UrlPrefix + "/workflow/workitems/{queueId}",
                        defaults: new
                        {
                            controller = "workflow",
                            action = "workitems",
                            processId = 0
                        }
                    );

                    routeConfig.Routes.MapHttpRoute(
                        name: "QueueUsers",
                        routeTemplate: UrlPrefix + "/workflow/users/{queueId}",
                        defaults: new
                        {
                            controller = "workflow",
                            action = "users"
                        }
                    );

                    routeConfig.Routes.MapHttpRoute(
                        name: "WorkflowApi",
                        routeTemplate: UrlPrefix + "/workflow/{action}/{processId}/{queueId}",
                        defaults: new
                        {
                            controller = "workflow",
                            processId = RouteParameter.Optional,
                            queueId = RouteParameter.Optional
                        }
                    );
                    
                    routeConfig.Routes.MapHttpRoute(
                        name: "WithActionApi",
                        routeTemplate: UrlPrefix + "/{controller}/{action}/{id}",
                        defaults: new { id = RouteParameter.Optional }
                    );

                    routeConfig.Routes.MapHttpRoute(
                        name: "DefaultApi",
                        routeTemplate: UrlPrefix + "/{controller}",
                        defaults: new { action = "get" }
                    );
                }

                return routeConfig;
            }
        }
    }
}