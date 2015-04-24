using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Filters;
using Common.Logging;
using TED.Dashboard.Exceptions;

namespace TED.Dashboard
{
    public class HandleApiException : ExceptionFilterAttribute
    {
        private static readonly ILog log = LogManager.GetLogger("TED.Dashboard.Api");
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            // log error and return error information
            log.Error(string.Format("Error occurred in api request.  Controller: {0}, Action: {1}",
                    actionExecutedContext.ActionContext.ControllerContext.RouteData.Values["controller"],
                    actionExecutedContext.ActionContext.ControllerContext.RouteData.Values["action"]),
                    actionExecutedContext.Exception);
            
            // if we are running in debug just send the exception to the caller
            #if DEBUG
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    ReasonPhrase = "Exception occurred",
                    Content = new StringContent(string.Format("Error: {0}, Stack: {1}", 
                                                                actionExecutedContext.Exception.Message,
                                                                actionExecutedContext.Exception.StackTrace))
                });
            #endif
            
            if (actionExecutedContext.Exception is BusinessException)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError)
                    {
                        ReasonPhrase = "Exception occurred",
                        Content = new StringContent(actionExecutedContext.Exception.Message)
                    });

            }

            throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                ReasonPhrase = "Critical exception occurred",
                Content = new StringContent("A critical error occurred, contact an administrator.")
            });
        }
    }
}