using TED.Dashboard.Repository;
using TED.Dashboard.UserSettings.Models;

namespace TED.Dashboard.UserSettings.UnitOfWork
{
    public class UserSettingsUOW : IUserSettingsUOW
    {
        private readonly ICRUDRepository<Widget> widgetRepository;
        private readonly ICRUDRepository<CustomDashboard> dashboardRepository;
        private readonly ICRUDRepository<UserParameters> userSettingsRepository;
        private readonly ISettingsContext context;

        public UserSettingsUOW(ISettingsContext context)
        {
            widgetRepository = new EFRepository<Widget>(context.Widgets);
            dashboardRepository = new EFRepository<CustomDashboard>(context.Dashboards);
            userSettingsRepository = new EFRepository<UserParameters>(context.UserSettings);
            this.context = context;
        }

        public ICRUDRepository<Widget> WidgetRepository
        {
            get { return widgetRepository; }
        }

        public ICRUDRepository<CustomDashboard> DashboardRepository
        {
            get { return dashboardRepository; }
        }

        public ICRUDRepository<UserParameters> UserSettingsRepository
        {
            get { return userSettingsRepository; }
        }

        public void UpdateEntityState<TEntity>(TEntity entity, System.Data.Entity.EntityState state)
        {
            context.Entry(entity).State = state;
        }

        public int Commit()
        {
            return context.SaveChanges();
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}