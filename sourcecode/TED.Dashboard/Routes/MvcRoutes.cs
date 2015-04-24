using System.Web.Mvc;
using System.Web.Routing;

namespace TED.Dashboard.Routes
{
    public class MvcRoutes
    {
        /// <summary>
        /// Declare routes here so that we can use them for unit testing
        /// </summary>
        private static RouteCollection routes;
        public static RouteCollection Routes
        {
            get
            {
                if (routes == null)
                {
                    routes = new RouteCollection();
                    routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

                    routes.MapRoute(
                        name: "Fallthrough",
                        url: "{controller}/{action}/{id}",
                        defaults: new { controller = "Dashboard", action = "Index", id = UrlParameter.Optional }
                        );

                    routes.MapRoute(
                        name: "Default",
                        url: "{action}",
                        defaults: new { controller = "Dashboard", action = "Index" }
                    );
                }

                return routes;
            }
        }

    }
}