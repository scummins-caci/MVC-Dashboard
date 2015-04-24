using TED.Dashboard.Repository.Connections;

namespace TED.Dashboard.Repository
{
    public abstract class DataflowDataStore<TEntity> : BaseDataStore<TEntity> where TEntity : class
    {
        protected DataflowDataStore()
            : base(new DataflowConnection())
        {
        }
    }
}
