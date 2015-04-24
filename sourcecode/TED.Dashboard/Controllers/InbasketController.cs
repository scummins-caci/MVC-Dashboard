using System.Web.Mvc;
using TED.Dashboard.Authentication.Authorization;
using TED.Dashboard.Users;

namespace TED.Dashboard.Controllers
{
    public class InbasketController : BaseController
    {
        // GET: Inbasket
        [VerifyAuthorization(Roles = ApplicationRole.WorkflowAdmin)]
        public ActionResult Basic()
        {
            return View();
        }

        // GET: Inbasket
        [VerifyAuthorization(Roles = ApplicationRole.WorkflowAdmin)]
        public ActionResult Component()
        {
            return View();
        }
    }
}