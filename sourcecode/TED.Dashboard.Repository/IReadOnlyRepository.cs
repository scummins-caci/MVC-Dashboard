using System.Collections.Generic;

namespace TED.Dashboard.Repository
{
    public interface IReadOnlyRepository<out TEntity> where TEntity: class
    {
        IEnumerable<TEntity> GetAll();
    }
}
