using System.Web.Mvc;
using System.Web.Routing;
using TED.Dashboard.Authentication;
using TED.Dashboard.Models;

namespace TED.Dashboard.Controllers
{
    public class BaseController : Controller
    {
        protected IUserPrincipal UserInfo
        {
            get { return HttpContext.User as IUserPrincipal; }
        }


        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);

            // get current user settings
            var model = new BaseViewModel
                {
                    CurrentAction = RouteData.Values["action"].ToString()
                };

            if (UserInfo != null)
            {
                model.UserDashboards = UserInfo.Dashboards;
                model.UserID = UserInfo.UserId;
                model.UserName = UserInfo.UserName;
                model.IsAuthenticated = UserInfo.IsAuthenticated;
            }

            ViewBag.UserData = model;

            // if the highview session isn't there, let's go back to the login page
            if (!model.IsAuthenticated)
            {
                RedirectToAction("Login", "Account");
            }
        }
    }
}
