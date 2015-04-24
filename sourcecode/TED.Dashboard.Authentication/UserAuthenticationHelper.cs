using System;
using System.Web;
using System.Web.Security;
using Newtonsoft.Json;
using TED.Dashboard.Authentication.Models;

namespace TED.Dashboard.Authentication
{
    /// <summary>
    /// Class to handle storing an retrieving user information from the 
    /// forms authentication ticket
    /// </summary>
    public class UserAuthenticationHelper
    {
        private readonly HttpContextBase CurrentContext;
        public UserAuthenticationHelper(HttpContextBase currentContext)
        {
            CurrentContext = currentContext;
        }
        

        /// <summary>
        /// Creates a forms authentication ticket and stores user information
        /// </summary>
        /// <param name="userInfo">user information to store in ticket</param>
        public void CreateUserSessionTicket(UserInformation userInfo)
        {
            var ticket = new FormsAuthenticationTicket(1, userInfo.UserName,
                       DateTime.Now, DateTime.Now.AddMinutes(15),
                       false, JsonConvert.SerializeObject(userInfo));
            var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket));

            CurrentContext.Response.Cookies.Add(authCookie);
        }

        /// <summary>
        /// Expires the current session authentication ticket
        /// </summary>
        public void DestroyUserSessionTicket()
        {
            if (CurrentContext.Response.Cookies[FormsAuthentication.FormsCookieName] == null)
            {
                return;
            }

            // remove authentication cookie
            var expiredCookie = new HttpCookie(FormsAuthentication.FormsCookieName)
                {
                    Expires = DateTime.Now.AddDays(-1)
                };
            CurrentContext.Response.Cookies.Remove(FormsAuthentication.FormsCookieName);
            CurrentContext.Response.Cookies.Add(expiredCookie);
        }

        /// <summary>
        /// Gets user information from the current session ticket
        /// </summary>
        /// <returns>User principal information</returns>
        public IUserPrincipal GetUserInformationFromTicket()
        {            
            var authCookie = CurrentContext.Request.Cookies[FormsAuthentication.FormsCookieName];

            if (authCookie == null || authCookie.Value == null)
            {
                return null;
            }

            var authTicket = FormsAuthentication.Decrypt(authCookie.Value);
            if (authTicket == null)
            {
                return null;
            }

            var userModel = JsonConvert.DeserializeObject<UserPrincipal>(authTicket.UserData);

            // set our custom principal
            return new UserPrincipal(userModel.UserId, userModel.UserName, userModel.Roles)
            {
                Dashboards = userModel.Dashboards
            };
        }
    }
}
