using System.Collections.Generic;

namespace TED.Dashboard.Models
{
    public class BaseViewModel
    {
        public string UserName { get; set; }
        public uint UserID { get; set; }
        public bool IsAuthenticated { get; set; }
        public string CurrentAction { get; set; }

        public IEnumerable<string> UserDashboards { get; set; } 
    }
}