using System.Reflection;
using System.Web;

namespace TED.Dashboard.Authentication
{
    public interface IAuthenticate
    {
        AuthType ApplicationAuthType { get; }
        bool AuthenticateUser(HttpContext context, string userName, string password);
        bool AuthenticateUser(string userName, string password);
        bool AuthenticateUser(HttpContext context, HttpClientCertificate certificate, Assembly applicationAssembly);
        bool AuthenticateUser(Assembly applicationAssembly);

        void SignOut();
    }
}
