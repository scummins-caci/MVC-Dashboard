using System.Collections.Generic;
using System.Security.Principal;

namespace TED.Dashboard.Authentication
{
    public interface IUserPrincipal : IPrincipal
    {
        string UserName { get; }
        uint UserId { get; }
        bool IsAuthenticated { get; }

        IEnumerable<string> Dashboards { get; set; }
        IEnumerable<string> Roles { get; }
    }
}
