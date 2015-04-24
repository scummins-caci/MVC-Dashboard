using System.Collections.Generic;
using Omu.ValueInjecter;
using TED.Dashboard.UserSettings.Models;

namespace TED.Dashboard.Api.Models
{
    public class UserParametersDataModel
    {
        public int ID { get; set; }
        public int UserID { get; set; }

        // model links
        public virtual ICollection<CustomDashboardDataModel> Dashboards { get; set; }

        public static UserParametersDataModel BuildFromUserParameters(UserParameters parameters)
        {
            var parametersVM = new UserParametersDataModel();
            parametersVM.InjectFrom(parameters);

            return parametersVM;
        }
    }
}