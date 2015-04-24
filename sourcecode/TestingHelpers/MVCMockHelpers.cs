using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Moq;
    
namespace TestingHelpers
{
    public static class MVCMockHelpers
    {
        #region Mock HTTP Context methods

        public static HttpContextBase FakeHttpContext()
        {
            var context = new Mock<HttpContextBase>();
            var request = new Mock<HttpRequestBase>();
            var response = new Mock<HttpResponseBase>();
            var session = new Mock<HttpSessionStateBase>();
            var server = new Mock<HttpServerUtilityBase>();
            var application = new Mock<HttpApplication>();

            // share cookie collection between requests and responses
            var cookieCollection = new HttpCookieCollection();

            context.Setup(ctx => ctx.Request).Returns(request.Object);
            context.Setup(ctx => ctx.Request.Cookies).Returns(cookieCollection);

            // mock browser capabilities
            var browser = new Mock<HttpBrowserCapabilitiesBase>(MockBehavior.Strict);
            browser.Setup(ctx => ctx.Capabilities).Returns(new Dictionary<string, string> { { "supportsEmptyStringInCookieValue", "false" } });
            context.Setup(ctx => ctx.Request.Browser).Returns(browser.Object);


            context.Setup(ctx => ctx.Response).Returns(response.Object);
            context.Setup(ctx => ctx.Response.Cookies).Returns(cookieCollection);
            context.Setup(ctx => ctx.Session).Returns(session.Object);
            context.Setup(ctx => ctx.Server).Returns(server.Object);
            context.Setup(ctx => ctx.ApplicationInstance).Returns(application.Object);
            context.SetupProperty(ctx => ctx.User);
            

            return context.Object;
        }

        public static HttpContext FakeCurrentContext()
        {
            var httpRequest = new HttpRequest("fakefile", "http://example.com", "user=noone");
            var httpResponse = new HttpResponse(new Mock<TextWriter>().Object);
            var httpcontext = new HttpContext(httpRequest, httpResponse);

            return httpcontext;
        }

        public static HttpContextBase FakeHttpContext(string url)
        {
            var context = FakeHttpContext();
            context.Request.SetupRequestUrl(url);
            return context;
        }

        public static void SetFakeControllerContext(this Controller controller, string requestUrl = "~/Nowhere/")
        {
            var httpContext = FakeHttpContext(requestUrl);
            var requestContext = new RequestContext(httpContext, new RouteData());
            var context = new ControllerContext(requestContext, controller);
            controller.ControllerContext = context;
            controller.Url = new UrlHelper(requestContext);
        }

        private static string GetUrlFileName(string url)
        {
            return url.Contains("?") ? url.Substring(0, url.IndexOf("?")) : url;
        }

        private static NameValueCollection GetQueryStringParameters(string url)
        {
            if (url.Contains("?"))
            {
                var parameters = new NameValueCollection();

                var parts = url.Split("?".ToCharArray());
                var keys = parts[1].Split("&".ToCharArray());

                foreach (var key in keys)
                {
                    var part = key.Split("=".ToCharArray());
                    parameters.Add(part[0], part[1]);
                }

                return parameters;
            }
            return null;
        }

        public static void SetHttpMethodResult(this HttpRequestBase request, string httpMethod)
        {
            Mock.Get(request)
                .Setup(req => req.HttpMethod)
                .Returns(httpMethod);
        }

        public static void SetupRequestUrl(this HttpRequestBase request, string url)
        {
            if (url == null)
                throw new ArgumentNullException("url");

            if (!url.StartsWith("~/"))
                throw new ArgumentException("Sorry, we expect a virtual url starting with \"~/\".");

            var mock = Mock.Get(request);

            mock.Setup(req => req.QueryString)
                .Returns(GetQueryStringParameters(url));
            mock.Setup(req => req.AppRelativeCurrentExecutionFilePath)
                .Returns(GetUrlFileName(url));
            mock.Setup(req => req.PathInfo)
                .Returns(string.Empty);
            mock.SetupGet(x => x.Url).Returns(new Uri(url.Replace("~/", "http://www.nowhere.com/")));
        }

        #endregion
    }
}
