using Common.Logging;
using TED.Dashboard.Models;
using System.Web.Mvc;


namespace TED.Dashboard
{
    public class HandleException : HandleErrorAttribute
    {
        private static readonly ILog log = LogManager.GetLogger("TED.Dashboard");

        public override void OnException(ExceptionContext filterContext)
        {
            if (filterContext.ExceptionHandled)
            {
                return;
            }

            var errorMessage = string.Format("Error occurred in dashboard application.  Controller: {0}, Action: {1}",
                                             filterContext.RouteData.Values["controller"],
                                             filterContext.RouteData.Values["action"]);
            log.Error(errorMessage, filterContext.Exception);

            var viewModel = new ErrorViewModel();

            #if DEBUG
                viewModel.ErrorMessage = filterContext.Exception.Message;
                viewModel.ErrorStack = filterContext.Exception.StackTrace;
                viewModel.ErrorOrigin = errorMessage;
            #else
                viewModel.ErrorMessage = "Please contact an administrator for assistance.";
            #endif

            filterContext.Result = new ViewResult { ViewName = "Error",
                                                    ViewData = new ViewDataDictionary<ErrorViewModel>(viewModel)
                                                  };
            filterContext.ExceptionHandled = true;
        }
    }
}