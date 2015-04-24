using TED.Dashboard.Repository;
using System.Linq;
using TED.Dashboard.Search.Models;
using TED.Dashboard.Services.Models;

namespace TED.Dashboard.Search.Services
{
    public class NotificationsService : INotificationsService
    {
        private readonly IPageableRepository<LogEntry> logRepository;

        public NotificationsService(IPageableRepository<LogEntry> logRepository)
        {
            this.logRepository = logRepository;
        }

        public PageableItem<LogEntry> GetAllLogEntries()
        {
            var allItems = logRepository.GetAll().ToList();
            return new PageableItem<LogEntry>
                {
                    Items = allItems,
                    ItemCount = allItems.Count()
                };
        }

        public PageableItem<LogEntry> GetPagedLogEntries(int page, int count)
        {
            var logs = logRepository.GetPage(page, count).ToArray();

            return new PageableItem<LogEntry>
                {
                    Items = logs,
                    ItemCount = logs.Any() ? logs.First().LogCount : 0
                };
        }
    }
}
