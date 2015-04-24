using System.Web.Http;
using TED.Dashboard.Routes;

namespace TED.Dashboard
{
    public static class WebApiConfig
    {   
        public static void Register(HttpConfiguration config)
        {
            var routeNumber = 1;
            foreach (var route in WebApiRoutes.RouteConfig.Routes)
            {
                config.Routes.Add(string.Format("Route{0}", routeNumber), route);
                routeNumber++;
            }
        }
    }
}
