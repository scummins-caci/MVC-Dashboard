using Omu.ValueInjecter;
using TED.Dashboard.UserSettings.Models;

namespace TED.Dashboard.Api.Models
{
    public class WidgetDataModel
    {
        public int WidgetID { get; set; }
        public string DisplayName { get; set; }
        public string ControlName { get; set; }

        public static WidgetDataModel BuildFromWidget(Widget widget)
        {
            var widgetVM = new WidgetDataModel();
            widgetVM.InjectFrom(widget);

            return widgetVM;
        }
    }
}