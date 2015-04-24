using System;
using System.Security.Principal;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using Common.Logging;
using TED.Dashboard.Users;

namespace TED.Dashboard.Authentication.Authorization
{
    public class VerifyDataAuthorizationAttribute : AuthorizeAttribute
    {
        private static readonly ILog log = LogManager.GetLogger("TED.Dashboard.Data.Authorization");
        
        protected virtual IPrincipal CurrentUser
        {
            get { return HttpContext.Current.User; }
        }

        /// <summary>
        /// Check to see if user is authorized to access web api controller action
        /// </summary>
        /// <param name="actionContext"></param>
        /// <returns></returns>
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            if (actionContext == null)
            {
                throw new NullReferenceException("'actionContext' is null.");
            }

            // check to see if user is authenticated
            if (!CurrentUser.Identity.IsAuthenticated)
            {
                return false;
            }

            // make sure user can access application with the base 'MonitorUser' role
            if (!CurrentUser.IsInRole(ApplicationRole.MonitorUser))
            {
                log.ErrorFormat("Current user '{0}' does not have access to Monitor application",
                    CurrentUser.Identity.Name);
                return false;
            }

            // if there are no roles specified for this action, we are ok to authorize
            if (String.IsNullOrEmpty(Roles))
            {
                return true;
            }

            // if the current user isn't in the roles for this action, return false
            if (!CurrentUser.IsInRole(Roles))
            {
                log.ErrorFormat("Current user '{0}' is not in any of the following roles: '{1}'",
                    CurrentUser.Identity.Name, Roles);
                return false;
            }

            // passed all tests, allow through
            return true;
        }
    }
}
