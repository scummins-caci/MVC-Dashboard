using TED.Dashboard.Dataflow.Models;
using TED.Dashboard.Services.Models;

namespace TED.Dashboard.Dataflow.Services
{
    public interface IDataflowService
    {
        PageableItem<Receipt> GetReceipts(int page = 1, int pageSize = 10);
        PageableItem<ErrorEntry> GetDataflowErrors(int page = 1, int pageSize = 10);
        DataflowChangeInfo GetChangeInformation(int changeId);
    }
}
