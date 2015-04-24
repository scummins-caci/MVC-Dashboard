using System.Collections.Generic;
using TED.Dashboard.User.Services;
using TED.Dashboard.Users.Models;

namespace TED.Dashboard.Users.Services
{
    public class DashboardService : IDashboardService
    {
        private CustomDashboard testDashboard = new CustomDashboard()
        {
            DashboardTitle = "Custom Dashboard 1",
            WidgetControls = new List<string>()
                        {
                            "_WorkflowLogsPartial",
                            "_WorkitemCountsPartial",
                            "_WorkflowLogsPartial",
                            "_WorkitemCountsPartial"
                        }
        };


        public CustomDashboard GetDashboardByName(string name)
        {
            // TODO:  replace stub code with data retrieval
            return testDashboard;
        }

        public CustomDashboard GetDashboardById(ulong id)
        {
            // TODO:  replace stub code with data retrieval
            return testDashboard;
        }
    }
}
