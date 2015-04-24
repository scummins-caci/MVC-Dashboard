using System;
using System.Collections.Generic;
using Omu.ValueInjecter;
using TED.Dashboard.UserSettings.Models;

namespace TED.Dashboard.Api.Models
{
    public class CustomDashboardDataModel
    {
        public int DashboardID { get; set; }
        public string Name { get; set; }
        public DateTime DateCreated { get; set; }
        public int CreatedBy { get; set; }
        public bool IsPublic { get; set; }

        // model links
        public virtual ICollection<WidgetDataModel> Widgets { get; set; }

        public static CustomDashboardDataModel BuildFromCustomDashboard(CustomDashboard dashboard)
        {
            var dashboardVM = new CustomDashboardDataModel();
            dashboardVM.InjectFrom(dashboard);

            return dashboardVM;
        }
    }
}