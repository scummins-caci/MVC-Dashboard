using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace TED.Dashboard.Repository
{
    public interface ICRUDRepository<TEntity> where TEntity : class
    {
        void Add(TEntity entity);
        void Delete(TEntity entity);
        void AttachForUpdate(TEntity entity);

        IEnumerable<TEntity> GetAll();
        Task<IEnumerable<TEntity>> GetAllAsync();

        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);
        Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);

        TEntity FindSingleOrDefault(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> FindSingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);

        TEntity FindFirst(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> FindFirstAsync(Expression<Func<TEntity, bool>> predicate);

        TEntity GetById(int id);
        Task<TEntity> GetByIdAsync(int id);
    }
}
