using System;
using System.Reflection;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using Common.Logging;
using HighView.Framework;
using HighView.Security.Exception;
using TED.Dashboard.Authentication.Models;
using TED.Dashboard.Users.Services;


namespace TED.Dashboard.Authentication.HV5
{
    public class Authenticate : IAuthenticate
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Authenticate));
        private readonly IUserInfoService userInfoService;
        private readonly UserAuthenticationHelper authenticationHelper;
        
        public Authenticate(IUserInfoService userInfo, HttpContextBase httpContext)
        {
            userInfoService = userInfo;
            authenticationHelper = new UserAuthenticationHelper(httpContext);
        }

        /// <summary>
        /// gets the current authentication type set for the application
        /// </summary>
        public AuthType ApplicationAuthType
        {
            get
            {
                var authTypeString = SecureConfiguration.AppSettings["AuthenticationType"];

                // if key is missing go with password authentication
                if (authTypeString == null)
                {
                    return AuthType.PasswordAuthentication;
                }

                if (authTypeString.ToLower() == "passwordauthentication")
                {
                    return AuthType.PasswordAuthentication;
                }

                if (authTypeString.ToLower() == "pkicertificate")
                {
                    return AuthType.PKIAuthentication;
                }

                // if we got here the key doesn't match a supported authentication type
                throw new AuthenticationException(string.Format("Authentication type '{0}' is not supported.", authTypeString));
            }
        }

        /// <summary>
        /// Creates a hv5 session and generates session cookie
        /// </summary>
        /// <param name="userName">username</param>
        /// <param name="password">password</param>
        /// <returns>success</returns>
        public bool AuthenticateUser(string userName, string password)
        {
            var context = HttpContext.Current;
            return AuthenticateUser(context, userName, password);
        }

        /// <summary>
        /// Creates a hv5 session and generates session cookie
        /// </summary>
        /// <param name="context">current context</param>
        /// <param name="userName">username</param>
        /// <param name="password">password</param>
        /// <returns>success</returns>
        public bool AuthenticateUser(HttpContext context, string userName, string password)
        {
            // make sure current authentication type is username/password
            if (ApplicationAuthType != AuthType.PasswordAuthentication)
            {
                throw new AuthenticationException("System currently not configured to use password authentication.");
            }
            
            var connectString = SecureConfiguration.AppSettings["AuthenticationConnectString"];
            try
            {
                WebModule.CreateNewSession(context, connectString, userName, password);
            }
            catch (HighViewLogonFailed lex)
            {
                log.Error("Logon error during HV username/password authentication.", lex);
                return false;
            }
            catch (Exception ex)
            {
                log.Error("Exception occurred during HV5Authenticate.AuthenticateUser", ex);
                throw;
            }

            // store the authentication ticket
            SetSessionTicket();

            return true;
        }

        /// <summary>
        /// Creates a hv5 session and generates session cookie
        /// </summary>
        /// <returns>success</returns>
        public bool AuthenticateUser(Assembly applicationAssembly)
        {
            var context = HttpContext.Current;
            return AuthenticateUser(context, context.Request.ClientCertificate, applicationAssembly);
        }

        /// <summary>
        /// Creates a hv5 session and generates session cookie
        /// </summary>
        /// <param name="context">current context</param>
        /// <param name="certificate">client certificate</param>
        /// <param name="applicationAssembly">application assembly</param>
        /// <returns></returns>
        public bool AuthenticateUser(HttpContext context, HttpClientCertificate certificate, Assembly applicationAssembly)
        {
            // make sure current authentication type is PKI certificate auth
            if (ApplicationAuthType != AuthType.PKIAuthentication)
            {
                throw new AuthenticationException("System currently not configured to use PKI authentication.");   
            }
            
            try
            {
                // HV5 api requires the certificate to be X509Certificate2 type
                WebModule.CreateNewSession(context, null, null, applicationAssembly, 
                                            new X509Certificate2(certificate.Certificate));
            }
            catch (HighViewLogonFailed lex)
            {
                log.Error("Logon error during HV certificate authentication.", lex);
                return false;
            }
            catch (Exception ex)
            {
                log.Error("Exception occurred during HV5Authenticate.AuthenticateUser", ex);
                throw;
            }

            // store the authentication ticket
            SetSessionTicket();

            return true;
        }

        /// <summary>
        /// Sets the user information in the authentication ticket/cookie
        /// </summary>
        private void SetSessionTicket()
        {
            var userInfo = GetUserInformation();
            authenticationHelper.CreateUserSessionTicket(userInfo);
        }

        /// <summary>
        /// Gets user information to store in auth ticket
        /// </summary>
        /// <returns></returns>
        private UserInformation GetUserInformation()
        {
            var session = Session.GetSession();
            
            var userInfo = new UserInformation
                {
                    UserId = session.UserID,
                    UserName = session.User,
                    Roles = userInfoService.GetUserRoles(session.UserID),
                    Dashboards = userInfoService.GetUserDashboards(session.UserID)
                };
            
            return userInfo;
        }

        public void SignOut()
        {
            try
            {
                // close hv5 session
                WebModule.DestroySession();

                // remove authentication cookie
                authenticationHelper.DestroyUserSessionTicket();
            }
            catch (Exception ex)
            {
                log.Error("Exception occurred during HV5Authenticate.SignOut", ex);
                throw;
            }
        }
    }
}
