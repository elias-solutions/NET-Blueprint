using BIT.NET.Backend.Blueprint.DataAccess;
using BIT.NET.Backend.Blueprint.Entities.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using BIT.NET.Backend.Blueprint.Extensions;

namespace BIT.NET.Backend.Blueprint.Repository.Base
{
    public class Repository<TEntity> where TEntity : EntityBase
    {
        private readonly BlueprintDbContext _context;

        public Repository(BlueprintDbContext context) => _context = context;

        public async Task<TEntity> GetAsync(Guid id)
        {
            var entity =await _context.Set<TEntity>().FindAsync(id);
            if (entity == null)
            {
                throw new ArgumentException();
            }

            return entity;
        }
        
        public async Task<TEntity?> GetAsync(
            Expression<Func<TEntity, bool>>? predicate = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
            bool disableNoTracking = false)
        {
            var query = _context.Set<TEntity>().AsQueryable();

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (include != null)
            {
                query = include(query);
            }
            
            if (disableNoTracking)
            {
                return await query.FirstOrDefaultAsync();
            }

            return await query.AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<TEntity?> GetAsync(IQueryable<TEntity> query, Expression<Func<TEntity, bool>>? predicate = null)
        {
            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            return await query.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null)
        {
            var query = _context.Set<TEntity>().AsQueryable();

            if (include != null)
            {
                query = include(query);
            }

            return await query.ToListAsync();
        }

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _context.Set<TEntity>().AnyAsync(predicate);
        }

        public async Task<IEnumerable<TEntity>> FindAsync(
            Expression<Func<TEntity, bool>> predicate,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null)
        {
            var query = _context.Set<TEntity>().AsQueryable();
            query = query.Where(predicate);

            if (include != null)
            {
                query = include(query);
            }

            return await query.ToListAsync();
        }

        public async Task<EntityEntry<TEntity>> AddAsync(TEntity entity, Guid createdBy)
        {
            entity.Created = DateTimeOffset.UtcNow.ToDatabaseDateTimeOffset();
            entity.CreatedBy = createdBy;
            var result = await _context.Set<TEntity>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return result;
        }

        public async Task RemoveAsync(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
    
}
