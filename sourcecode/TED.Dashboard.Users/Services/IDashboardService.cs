using TED.Dashboard.Users.Models;

namespace TED.Dashboard.User.Services
{
    public interface IDashboardService
    {
        CustomDashboard GetDashboardByName(string name);
        CustomDashboard GetDashboardById(ulong id);
    }
}
