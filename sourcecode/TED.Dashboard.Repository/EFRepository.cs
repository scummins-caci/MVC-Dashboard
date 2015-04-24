using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace TED.Dashboard.Repository
{
    public class EFRepository<TEntity> : ICRUDRepository<TEntity> where TEntity : class
    {
        private readonly DbSet<TEntity> dbSet;

        public EFRepository(DbSet<TEntity> dbSet)
        {
            this.dbSet = dbSet;
        }

        public void Add(TEntity entity)
        {
            dbSet.Add(entity);
        }

        public void Delete(TEntity entity)
        {
            dbSet.Remove(entity);
        }

        public void AttachForUpdate(TEntity entity)
        {
            dbSet.Attach(entity);
        }

        public IEnumerable<TEntity> GetAll()
        {
            return dbSet.ToList();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await dbSet.ToListAsync();
        }

        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return BuildQuery(predicate);
        }

        public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await BuildQuery(predicate).ToListAsync();
        }

        public TEntity FindSingleOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return BuildQuery(predicate).SingleOrDefault();
        }

        public async Task<TEntity> FindSingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await BuildQuery(predicate).SingleOrDefaultAsync();
        }

        public TEntity FindFirst(Expression<Func<TEntity, bool>> predicate)
        {
            return BuildQuery(predicate).First();
        }

        public async Task<TEntity> FindFirstAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await BuildQuery(predicate).FirstAsync();
        }

        public TEntity GetById(int id)
        {
            return dbSet.Find(id);
        }

        public async Task<TEntity> GetByIdAsync(int id)
        {
            return await dbSet.FindAsync(id);
        }


        private IQueryable<TEntity> BuildQuery(Expression<Func<TEntity, bool>> predicate)
        {
            return dbSet.Where(predicate);
            // TODO:  add any includes?
        }
    }
}