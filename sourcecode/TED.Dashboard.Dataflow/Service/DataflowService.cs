using System.Linq;
using TED.Dashboard.Repository;
using TED.Dashboard.Dataflow.Models;
using TED.Dashboard.Services.Models;

namespace TED.Dashboard.Dataflow.Services
{
    public class DataflowService : IDataflowService
    {
        private readonly IPageableRepository<Receipt> repository;
        private readonly IPageableRepository<ErrorEntry> errorRepository;
        private readonly IIDFilterRepository<MetaRouteInfo> dmrRepository;
        private readonly IIDFilterRepository<BinaryRouteInfo> binRepository;
        private readonly IIDFilterRepository<ScanRouteInfo> scnRepository;

        public DataflowService(IPageableRepository<Receipt> repository,
                                IPageableRepository<ErrorEntry> errorRepository,
                                IIDFilterRepository<MetaRouteInfo> dmrRepository,
                                IIDFilterRepository<BinaryRouteInfo> binRepository,
                                IIDFilterRepository<ScanRouteInfo> scnRepository)
        {
            this.repository = repository;
            this.errorRepository = errorRepository;
            this.dmrRepository = dmrRepository;
            this.binRepository = binRepository;
            this.scnRepository = scnRepository;
        }

        /// <summary>
        /// Get a collection of dataflow records
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public PageableItem<Receipt> GetReceipts(int page = 1, int pageSize = 10)
        {
            var allItems = repository.GetPage(page, pageSize).ToList();
            return new PageableItem<Receipt>
            {
                Items = allItems,
                ItemCount = repository.GetCount()
            };
        }

        /// <summary>
        /// Gets a list of dataflow errors logged
        /// </summary>
        /// <param name="page">page to retrieve</param>
        /// <param name="pageSize">size of pages</param>
        /// <returns></returns>
        public PageableItem<ErrorEntry> GetDataflowErrors(int page = 1, int pageSize = 10)
        {
            var errors = errorRepository.GetPage(page, pageSize).ToList();
            return new PageableItem<ErrorEntry>
            {
                Items = errors,
                ItemCount = errorRepository.GetCount()
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="changeid"></param>
        /// <returns></returns>
        public DataflowChangeInfo GetChangeInformation(int changeid)
        {
            return new DataflowChangeInfo
                {
                    DocumentMetaRoutes = dmrRepository.GetAll(changeid),
                    BinaryRoutes = binRepository.GetAll(changeid),
                    ScanRoutes = scnRepository.GetAll(changeid)
                };
        }
    }
}
