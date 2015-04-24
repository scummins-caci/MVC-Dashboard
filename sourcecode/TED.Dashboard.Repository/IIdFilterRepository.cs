using System.Collections.Generic;

namespace TED.Dashboard.Repository
{
    public interface IIDFilterRepository<out TEntity> where TEntity : class
    {
        IEnumerable<TEntity> GetAll(long id);
    }
}
