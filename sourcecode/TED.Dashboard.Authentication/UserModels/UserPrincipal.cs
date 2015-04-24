using System.Collections.Generic;
using System.Security.Principal;
using System.Linq;

namespace TED.Dashboard.Authentication.Models
{
    public class UserPrincipal : IUserPrincipal
    {
        public UserPrincipal(uint id, string userName, IEnumerable<string> roles)
        {
            Identity = new GenericIdentity(userName);
            UserId = id;
            UserName = userName;
            Roles = roles;
        }
        /// <summary>
        /// Current user name
        /// </summary>
        public string UserName { get; private set; }

        /// <summary>
        /// current user id
        /// </summary>
        public uint UserId { get; private set; }
        
        /// <summary>
        /// List of dashboards user displays
        /// </summary>
        public IEnumerable<string> Dashboards { get; set; }

        /// <summary>
        /// List of roles user is in
        /// </summary>
        public IEnumerable<string> Roles { get; internal set; }
        

        public bool IsAuthenticated
        {
            get { return Identity.IsAuthenticated; }
        }

        public bool IsInRole(string role)
        {
            return Roles.Any(role.Contains);
        }

        public IIdentity Identity { get; private set; }
    }
}
