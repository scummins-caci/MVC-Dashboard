using System.Web.Http.Filters;
using System.Web.Mvc;
using TED.Dashboard.Authentication.Authorization;

namespace TED.Dashboard
{
    public class FilterConfig
    {
        /// <summary>
        /// Add filters that work with MVC controllers
        /// </summary>
        /// <param name="filters"></param>
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new VerifyAuthorizationAttribute());   
            filters.Add(new HandleException());
        }

        /// <summary>
        /// Add filters for Web Api controllers
        /// </summary>
        /// <param name="filters"></param>
        public static void RegisterWebApiFilters(HttpFilterCollection filters)
        {
            filters.Add(new VerifyDataAuthorizationAttribute());
            filters.Add(new HandleApiException());
        }
    }
}