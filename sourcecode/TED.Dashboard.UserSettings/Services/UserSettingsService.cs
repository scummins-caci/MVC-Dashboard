using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using TED.Dashboard.UserSettings.Models;
using TED.Dashboard.UserSettings.UnitOfWork;

namespace TED.Dashboard.UserSettings.Services
{
    public class UserSettingsService : IUserSettingsService
    {
        private readonly IUserSettingsUOW settingsUOW;

        public UserSettingsService(IUserSettingsUOW settingsUOW)
        {
            this.settingsUOW = settingsUOW;
        }

        #region retrieval methods

        public IEnumerable<Widget> GetAllWidgets()
        {
            return settingsUOW.WidgetRepository.GetAll();
        }

        public async Task<IEnumerable<Widget>> GetAllWidgetsAsync()
        {
            return await settingsUOW.WidgetRepository.GetAllAsync();
        }

        public IEnumerable<CustomDashboard> GetAllDashboards()
        {
            return settingsUOW.DashboardRepository.GetAll();
        }

        public async Task<IEnumerable<CustomDashboard>> GetAllDashboardsAsync()
        {
            return await settingsUOW.DashboardRepository.GetAllAsync();
        }

        public UserParameters GetUserSettings(int userId)
        {
            return settingsUOW.UserSettingsRepository.GetById(userId);
        }

        public async Task<UserParameters> GetUserSettingsAsync(int userId)
        {
            return await settingsUOW.UserSettingsRepository.GetByIdAsync(userId);
        }

        public CustomDashboard GetDashboard(int dashboardId)
        {
            return settingsUOW.DashboardRepository.GetById(dashboardId);
        }

        public async Task<CustomDashboard> GetDashboardAsync(int dashboardId)
        {
            return await settingsUOW.DashboardRepository.GetByIdAsync(dashboardId);
        }

        #endregion

        #region CRUD methods

        public int CreateUserSettings(UserParameters userSettings)
        {
            settingsUOW.UserSettingsRepository.Add(userSettings);
            return settingsUOW.Commit();
        }

        public int UpdateUserSettings(UserParameters userSettings)
        {
            settingsUOW.UserSettingsRepository.AttachForUpdate(userSettings);
            settingsUOW.UpdateEntityState(userSettings, EntityState.Modified);
            return settingsUOW.Commit();
        }

        public int CreateDashboard(CustomDashboard dashboard)
        {
            settingsUOW.DashboardRepository.Add(dashboard);
            return settingsUOW.Commit();
        }

        public int UpdateDashboard(CustomDashboard dashboard)
        {
            settingsUOW.DashboardRepository.AttachForUpdate(dashboard);
            settingsUOW.UpdateEntityState(dashboard, EntityState.Modified);
            return settingsUOW.Commit();
        }

        public int DeleteDashboard(CustomDashboard dashboard)
        {
            settingsUOW.DashboardRepository.Delete(dashboard);
            return settingsUOW.Commit();
        }

        #endregion
    }
}