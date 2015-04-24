using System.Collections.Generic;
using System.Threading.Tasks;
using TED.Dashboard.UserSettings.Models;

namespace TED.Dashboard.UserSettings.Services
{
    public interface IUserSettingsService
    {
        IEnumerable<Widget> GetAllWidgets();
        Task<IEnumerable<Widget>> GetAllWidgetsAsync();
        IEnumerable<CustomDashboard> GetAllDashboards();
        Task<IEnumerable<CustomDashboard>> GetAllDashboardsAsync();

        UserParameters GetUserSettings(int userId);
        Task<UserParameters> GetUserSettingsAsync(int userId);
        CustomDashboard GetDashboard(int dashboardId);
        Task<CustomDashboard> GetDashboardAsync(int dashboardId);

        int CreateUserSettings(UserParameters userSettings);
        int UpdateUserSettings(UserParameters userSettings);

        int CreateDashboard(CustomDashboard dashboard);
        int UpdateDashboard(CustomDashboard dashboard);
        int DeleteDashboard(CustomDashboard dashboard); 
    }
}