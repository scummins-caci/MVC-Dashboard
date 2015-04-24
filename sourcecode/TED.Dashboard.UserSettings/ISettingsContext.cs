using System.Data.Entity;
using System.Data.Entity.Infrastructure;

using TED.Dashboard.UserSettings.Models;

namespace TED.Dashboard.UserSettings
{
    public interface ISettingsContext
    {
        DbSet<UserParameters> UserSettings { get; set; }
        DbSet<CustomDashboard> Dashboards { get; set; }
        DbSet<Widget> Widgets { get; set; }
        int SaveChanges();
        

        DbEntityEntry Entry(object entity);
        void Dispose();
    }
}
