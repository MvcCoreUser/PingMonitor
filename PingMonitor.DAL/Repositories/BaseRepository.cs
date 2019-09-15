using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PingMonitor.DAL.EF;
using PingMonitor.DAL.Interfaces;

namespace PingMonitor.DAL.Repositories
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        private AppDbContext context;
        private DbSet<TEntity> dbSet;
        public BaseRepository(AppDbContext dbContext)
        {
            this.context = dbContext;
            dbSet = context.Set<TEntity>();
        }
        public Task<int> CreateAsync(TEntity item)
        {
            dbSet.Add(item);
            return context.SaveChangesAsync();
        }

        public void Dispose()
        {
            context.Dispose();
        }

        public TEntity FindById(int id)
        => dbSet.Find(id);

        public IQueryable<TEntity> Get(Func<TEntity, bool> predicate)
        => dbSet.Where(predicate).AsQueryable();

        public IQueryable<TEntity> GetAll()
        => dbSet.AsQueryable();

        public Task<int> RemoveAsync(TEntity item)
        {
            dbSet.Remove(item);
            return context.SaveChangesAsync();
        }

        public Task<int> UpdateAsync(TEntity item)
        {
            context.Entry(item).State = EntityState.Modified;
            return context.SaveChangesAsync();
        }

        public IQueryable<TEntity> GetAllWithInclude(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return Include(includeProperties);
        }

        public IQueryable<TEntity> GetWithInclude(Func<TEntity, bool> predicate,
            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var query = Include(includeProperties);
            return query.Where(predicate).AsQueryable();
        }

        private IQueryable<TEntity> Include(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = dbSet.AsNoTracking();
            return includeProperties
                .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        }
    }
}