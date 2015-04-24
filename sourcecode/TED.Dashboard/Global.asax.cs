using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.SessionState;
using TED.Dashboard.Authentication;
using TED.Dashboard.Routes;

namespace TED.Dashboard
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            
            // configure webapi
            // make sure to configure the web api routes prior to the mvc routes.
            // Reason being is that the web api routes start with ~/api/;  the routing
            // engine will think this is a controller name if it tries to apply the mvc route
            // logic, which will end in a 404.
            RouteConfig.RegisterWebApiRoutes(GlobalConfiguration.Configuration.Routes);
            FilterConfig.RegisterWebApiFilters(GlobalConfiguration.Configuration.Filters);

            // configure mvc
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        // this is needed to make sure session state exists in our web api requests, since
        // we are leveraging the HV5 session to make data requests
        protected void Application_PostAuthorizeRequest()
        {
            if (IsWebApiRequest())
            {
                HttpContext.Current.SetSessionStateBehavior(SessionStateBehavior.ReadOnly);
            }
        }

        /// <summary>
        /// Set our custom principal 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_PostAuthenticateRequest(object sender, EventArgs e)
        {
            // if the user has authenticated grab user info that is stored in the session cookie
            var authenticationHelper = new UserAuthenticationHelper(new HttpContextWrapper(HttpContext.Current));
            var userInfo = authenticationHelper.GetUserInformationFromTicket();

            if (userInfo == null)
            {
                return;
            }
            HttpContext.Current.User = userInfo;
        }

        private static bool IsWebApiRequest()
        {
            return HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath.StartsWith(WebApiRoutes.UrlPrefixRelative);
        }
    }
}