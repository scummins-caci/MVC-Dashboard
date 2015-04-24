using System.Collections.Generic;

namespace TED.Dashboard.Users.Models
{
    public class CustomDashboard
    {
        public ulong DashboardID { get; set; }
        public string DashboardTitle { get; set; }
        public IEnumerable<string> WidgetControls { get; set; }
    }
}
