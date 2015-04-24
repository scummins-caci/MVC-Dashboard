using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using TED.Dashboard.Api.Models;
using TED.Dashboard.UserSettings.Services;

namespace TED.Dashboard.Api.Controllers
{
    public class ManageController : ApiController
    {
        private readonly IUserSettingsService settingsService;
        public ManageController(IUserSettingsService settingsService)
        {
            this.settingsService = settingsService;
        }

        [HttpGet]
        public async Task<ApiReturnDataModel> Widgets()
        {
            var widgets = await settingsService.GetAllWidgetsAsync();
            return new ApiReturnDataModel(widgets.Select(WidgetDataModel.BuildFromWidget), 
                                            (int)HttpStatusCode.OK, true);
        }

        [HttpGet]
        public async Task<ApiReturnDataModel> Dashboards()
        {
            var dashboards = await settingsService.GetAllDashboardsAsync();
            return new ApiReturnDataModel(dashboards.Select(CustomDashboardDataModel.BuildFromCustomDashboard),
                                (int)HttpStatusCode.OK, true);
        }

        [HttpGet]
        public async Task<ApiReturnDataModel> UserSettings(int id)
        {
            var settings = await settingsService.GetUserSettingsAsync(id);
            return new ApiReturnDataModel(UserParametersDataModel.BuildFromUserParameters(settings), 
                                (int) HttpStatusCode.OK, true);
        }
    }
}
