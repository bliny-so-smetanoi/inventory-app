using InventoryApp.DataAccess.Providers.Interfaces;
using InventoryApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace InventoryApp.DataAccess.Providers.EntityProviders
{
    public class EntityProvider<TContext, TEntity, TId> : IProvider<TEntity, TId>
        where TEntity: Entity
        where TContext: ApplicationContext
    {
        private readonly TContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        public EntityProvider(TContext context) {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }
        public virtual async Task Add(TEntity added)
        {
            await _dbSet.AddAsync(added);
            await _context.SaveChangesAsync();
        }
        public virtual async Task AddRange(IEnumerable<TEntity> added)
        {
            await _dbSet.AddRangeAsync(added);
            await _context.SaveChangesAsync();
        }
        public virtual async Task Edit(TEntity edited)
        {
            _context.Entry(edited).State = EntityState.Modified;
            await _context.SaveChangesAsync();

        }
        public virtual async Task<TEntity> FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.FirstOrDefaultAsync(predicate);
        }

        public virtual async Task<List<TEntity>> Get(Func<TEntity, bool> predicate, int take = int.MaxValue, int skip = 0)
        {
            return (await GetAll()).Where(predicate).OrderBy(entity => entity.DateTime).Skip(skip).Take(take).ToList();
        }

        public virtual async Task<List<TEntity>> GetAll(int take = int.MaxValue, int skip = 0)
        {
            return await _dbSet.AsNoTracking().OrderBy(entity => entity.DateTime).Skip(skip).Take(take).ToListAsync();
        }

        public virtual async Task<TEntity> GetById(TId id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual async Task Remove(TEntity removed)
        {
            _dbSet.Remove(removed);
            await _context.SaveChangesAsync();
        }
    }
}
