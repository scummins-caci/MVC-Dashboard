
using System.Collections.Generic;

namespace TED.Dashboard.Authentication.Models
{
    public class UserInformation
    {
        public uint UserId { get; set; }
        public string UserName { get; set; }
        public IEnumerable<string> Roles { get; set; } 
        public IEnumerable<string> Dashboards { get; set; } 
    }
}
