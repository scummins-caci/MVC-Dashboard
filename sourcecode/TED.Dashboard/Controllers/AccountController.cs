using System.Reflection;
using System.Web.Mvc;
using TED.Dashboard.Models;
using TED.Dashboard.Authentication;

namespace TED.Dashboard.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IAuthenticate authenticator;
        public AccountController(IAuthenticate authenticator)
        {
            this.authenticator = authenticator;
        }

        //
        // GET: /Account/
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            // choose authentication type
            var authType = authenticator.ApplicationAuthType;
            if (authType == AuthType.PKIAuthentication)
            {
                return PKIAuthenticationLogin(returnUrl);
            }

            if (authType == AuthType.PasswordAuthentication)
            {
                return PasswordAuthenticationLogin(returnUrl);
            }

            throw new AuthenticationException(string.Format("Authentication type '{0}' currently not supported.",
                                                        authType));
        }

        /// <summary>
        /// Display the login control for users to enter username and password
        /// </summary>
        /// <param name="returnUrl">return url</param>
        /// <returns>mvc action result</returns>
        private ActionResult PasswordAuthenticationLogin(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View("Login");
        }

        /// <summary>
        /// Attempts to authenticate user using PKI authentication
        /// </summary>
        /// <param name="returnUrl">return url</param>
        /// <returns>mvc action result</returns>
        private ActionResult PKIAuthenticationLogin(string returnUrl)
        {
            // the authenticator requires the calling assembly info to match things up
            var assembly = Assembly.GetCallingAssembly();
            var result = authenticator.AuthenticateUser(assembly);

            // if certificate authentication failed
            if (!result)
            {
                return RedirectToAction("AccessDenied", "Error");
            }

            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Dashboard");
        }

        /// <summary>
        /// Login POST action.  This is for username/password authentication
        /// </summary>
        /// <param name="model">viewModel containing user entered values</param>
        /// <param name="returnUrl">url to redirect user to</param>
        /// <returns>mvc action result</returns>
        [HttpPost, AllowAnonymous, ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            var result = authenticator.AuthenticateUser(model.Username, model.Password);

            if (!result)
            {
                ModelState.AddModelError("LoginError", "The username or password provided is incorrect.");
                return View();
            }

            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Dashboard");
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            authenticator.SignOut();

            // if this is a pki system, redirect to the logout page.  
            // If not, just go back to the username/password login.
            return RedirectToAction(
                        authenticator.ApplicationAuthType == AuthType.PKIAuthentication ? "LoggedOut" : "Login", 
                        "Account");
        }

        /// <summary>
        /// Landing point for users who have Logged out of the system
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult LoggedOut()
        {
            return View();
        }
    }
}
