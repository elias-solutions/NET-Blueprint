using System.Linq.Expressions;
using System.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query;
using NET.Backend.Blueprint.Api.DataAccess;
using NET.Backend.Blueprint.Api.Entities.Base;
using NET.Backend.Blueprint.Api.ErrorHandling;
using NET.Backend.Blueprint.Extensions;

namespace NET.Backend.Blueprint.Api.Repository.Base
{
    public class Repository<TEntity> where TEntity : EntityBase
    {
        private readonly BlueprintDbContext _context;

        public Repository(BlueprintDbContext context) => _context = context;

        public async Task<TEntity> GetAsync(Guid id, bool asNoTracking = false)
        {
            var entity = asNoTracking ? 
                await _context.Set<TEntity>().AsNoTracking().SingleOrDefaultAsync(entity => entity.Id == id) :
                await _context.Set<TEntity>().SingleOrDefaultAsync(entity => entity.Id == id);

            if (entity == null)
            {
                throw new ProblemDetailsException(HttpStatusCode.BadRequest, "No entity found", $"No entity with id '{id}' found.");
            }

            return entity;
        }
        
        public async Task<TEntity?> GetAsync(
            Expression<Func<TEntity, bool>>? predicate = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
            bool asNoTracking = false)
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
            
            if (asNoTracking)
            {
                query.AsNoTracking();
            }

            return await query.FirstOrDefaultAsync();
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
            return await _context.Set<TEntity>().AsQueryable().AsNoTracking().AnyAsync(predicate);
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
            entity.Created = DateTime.UtcNow.ToUtcDateTimeOffset();
            entity.CreatedBy = createdBy;
            entity.Modified = DateTimeOffset.MinValue;
            entity.ModifiedBy = Guid.Empty;
            entity.Version = Guid.NewGuid();

            var result = await _context.Set<TEntity>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return result;
        }

        public async Task<TEntity> UpdateAsync(TEntity entity, Guid modifiedBy)
        {
            var hasVersionConflict = await AnyAsync(e => e.Id == entity.Id && e.Version != entity.Version);
            if (hasVersionConflict)
            {
                throw new ProblemDetailsException(
                    HttpStatusCode.BadRequest, "Entity version conflict", "Entity has been updated through other user.");
            }
            
            entity.Created = entity.Created;
            entity.CreatedBy = entity.CreatedBy;
            entity.Modified = DateTime.UtcNow.ToUtcDateTimeOffset();
            entity.ModifiedBy = modifiedBy;
            entity.Version = Guid.NewGuid();

            _context.Set<TEntity>().Attach(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task RemoveAsync(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
    
}
