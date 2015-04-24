using System.Web.Http;
using System.Web.Routing;
using TED.Dashboard.Routes;

namespace TED.Dashboard
{
    public class RouteConfig
    {
        /// <summary>
        /// Register routes declared above
        /// </summary>
        /// <param name="routes"></param>
        public static void RegisterRoutes(RouteCollection routes)
        {
            foreach (var route in MvcRoutes.Routes)
            {
                routes.Add(route);
            }
        }

        /// <summary>
        /// Register web api routes
        /// </summary>
        /// <param name="routes"></param>
        public static void RegisterWebApiRoutes(HttpRouteCollection routes)
        {
            var routeNumber = 1;
            foreach (var route in WebApiRoutes.RouteConfig.Routes)
            {
                routes.Add(string.Format("Route{0}", routeNumber), route);
                routeNumber++;
            }
        }
    }
}