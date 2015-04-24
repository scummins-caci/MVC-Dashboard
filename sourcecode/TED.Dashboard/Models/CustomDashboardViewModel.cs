using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TED.Dashboard.Models
{
    public class CustomDashboardViewModel
    {
        public string DashboardTitle { get; set; }
        public IEnumerable<string> WidgetControls { get; set; } 
    }
}