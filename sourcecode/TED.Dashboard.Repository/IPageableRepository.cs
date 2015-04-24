using System.Collections.Generic;

namespace TED.Dashboard.Repository
{
    public interface IPageableRepository<out TEntity> where TEntity : class
    {
        IEnumerable<TEntity> GetAll();
        IEnumerable<TEntity> GetPage(int page, int count);
        int GetCount();
    }
}
