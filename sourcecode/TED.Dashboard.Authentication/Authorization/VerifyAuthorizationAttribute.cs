using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Common.Logging;
using TED.Dashboard.Users;

namespace TED.Dashboard.Authentication.Authorization
{
    public class VerifyAuthorizationAttribute : AuthorizeAttribute
    {
        private static readonly ILog log = LogManager.GetLogger("TED.Dashboard.Authorization");

        protected virtual IPrincipal CurrentUser
        {
            get { return HttpContext.Current.User; }
        }

        /// <summary>
        /// Send unauthorized users to our custom error page.
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            // if the user isn't authenticated send an unauthorized result response
            if (!CurrentUser.Identity.IsAuthenticated)
            {
                filterContext.Result = new HttpUnauthorizedResult();
                return;
            }

            // user must be in the main access role
            if (!CurrentUser.IsInRole(ApplicationRole.MonitorUser))
            {
                log.ErrorFormat("Current user '{0}' does not have access to Monitor application",
                    CurrentUser.Identity.Name);

                filterContext.Result = RedirectToAccessDenied();
                return;
            }

            // if this is a role based authorization rejection, send the user to a custom page
            if (!CurrentUser.IsInRole(Roles))
            {
                log.ErrorFormat("Current user '{0}' is not in any of the following roles: '{1}'",
                    CurrentUser.Identity.Name, Roles);

                filterContext.Result = RedirectToAccessDenied();
                return;
            }

            // anything else go with default behavior
            base.HandleUnauthorizedRequest(filterContext);
        }

        /// <summary>
        /// Send user to the access denied error page
        /// </summary>
        /// <returns></returns>
        private static RedirectToRouteResult RedirectToAccessDenied()
        {
            return new RedirectToRouteResult(new RouteValueDictionary(
                            new { controller = "Error", action = "AccessDenied" }));
        }
    }
}
