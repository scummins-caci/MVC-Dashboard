using TED.Dashboard.Repository.Connections;

namespace TED.Dashboard.Repository
{
    public abstract class HV5DataStore<TEntity> : BaseDataStore<TEntity> where TEntity : class 
    {
        protected HV5DataStore() : base(new HV5Connection()) { }
    }
}
