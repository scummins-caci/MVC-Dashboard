using System.Configuration;
using System.Data.Common;
using System.Data.Entity;

namespace TED.Dashboard.Repository.Context
{
    public class BaseEFContext<T> : DbContext where T : DbContext
    {
        static BaseEFContext()
        {
            Database.SetInitializer<T>(null);
        }

        protected BaseEFContext(DbConnection existingConnection, bool contextOwnsConnection)
            : base(existingConnection, contextOwnsConnection) { }

        protected BaseEFContext()
            : base("name=MainContext") { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var defaultSchema = ConfigurationManager.AppSettings["DefaultSchema"];
            if (!string.IsNullOrEmpty(defaultSchema))
            {
                modelBuilder.HasDefaultSchema(defaultSchema);
            }
            
            base.OnModelCreating(modelBuilder);
        }
    }
}
