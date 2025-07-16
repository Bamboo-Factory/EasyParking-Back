using Microsoft.EntityFrameworkCore;
using EasyParking.Core.Interfaces;
using EasyParking.Core.Entities;
using EasyParking.Infrastructure.Data;
using System.Linq.Expressions;

namespace EasyParking.Infrastructure.Repositories
{
    public class BaseRepository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly EasyParkingDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public BaseRepository(EasyParkingDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public virtual async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.Where(e => e.IsActive).ToListAsync();
        }

        public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).Where(e => e.IsActive).ToListAsync();
        }

        public virtual async Task<T> AddAsync(T entity)
        {
            entity.CreatedAt = DateTime.UtcNow;
            entity.IsActive = true;
            
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            
            return entity;
        }

        public virtual async Task UpdateAsync(T entity)
        {
            entity.UpdatedAt = DateTime.UtcNow;
            
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                entity.IsActive = false;
                entity.UpdatedAt = DateTime.UtcNow;
                
                _dbSet.Update(entity);
                await _context.SaveChangesAsync();
            }
        }

        public virtual async Task<bool> ExistsAsync(int id)
        {
            return await _dbSet.AnyAsync(e => e.Id == id && e.IsActive);
        }

        public virtual async Task<int> CountAsync()
        {
            return await _dbSet.CountAsync(e => e.IsActive);
        }
    }
} 