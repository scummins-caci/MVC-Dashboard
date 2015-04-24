using System;
using System.Data.Entity;
using TED.Dashboard.Repository;
using TED.Dashboard.UserSettings.Models;

namespace TED.Dashboard.UserSettings.UnitOfWork
{
    public interface IUserSettingsUOW : IDisposable
    {
        ICRUDRepository<Widget> WidgetRepository { get; }
        ICRUDRepository<CustomDashboard> DashboardRepository { get; }
        ICRUDRepository<UserParameters> UserSettingsRepository { get; }

        void UpdateEntityState<TEntity>(TEntity entity, EntityState state);
        int Commit();
    }
}