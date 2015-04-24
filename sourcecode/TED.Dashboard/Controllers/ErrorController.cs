using System.Web.Mvc;
using TED.Dashboard.Models;

namespace TED.Dashboard.Controllers
{
    public class ErrorController : BaseController
    {
        // GET: /Error/
        [AllowAnonymous]
        public ActionResult AccessDenied()
        {
            var viewModel = new ErrorViewModel
                {
                    ErrorMessage = "The current user is not authorized to access this page."
                };

            return View("Error", viewModel);
        }
    }
}
