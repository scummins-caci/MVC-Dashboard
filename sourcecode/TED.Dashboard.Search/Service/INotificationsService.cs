using TED.Dashboard.Search.Models;
using TED.Dashboard.Services.Models;

namespace TED.Dashboard.Search.Services
{
    public interface INotificationsService
    {
        // get all of the connector log entries
        PageableItem<LogEntry> GetAllLogEntries();
        PageableItem<LogEntry> GetPagedLogEntries(int page, int count);
    }
}
