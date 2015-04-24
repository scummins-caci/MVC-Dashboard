using System.Data.Common;
using System.Data.Entity;
using TED.Dashboard.Repository.Context;
using TED.Dashboard.UserSettings.Models;

namespace TED.Dashboard.UserSettings
{
    public class SettingsContext : BaseEFContext<SettingsContext>, ISettingsContext
    {
        public SettingsContext() { }
        public SettingsContext(DbConnection existingConnection, bool contextOwnsConnection)
            : base(existingConnection, contextOwnsConnection) { }

        public DbSet<UserParameters> UserSettings { get; set; }
        public DbSet<CustomDashboard> Dashboards { get; set; }
        public DbSet<Widget> Widgets { get; set; }
    }
}
