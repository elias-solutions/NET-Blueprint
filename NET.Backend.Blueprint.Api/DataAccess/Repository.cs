using System.Linq.Expressions;
using System.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query;
using NET.Backend.Blueprint.Api.Authorization;
using NET.Backend.Blueprint.Api.Entities.Base;
using NET.Backend.Blueprint.Api.ErrorHandling;
using NET.Backend.Blueprint.Extensions;

namespace NET.Backend.Blueprint.Api.DataAccess
{
    public class Repository<TEntity>(BlueprintDbContext context, IUserService userService)
        where TEntity : EntityBase
    {
        public async Task<TEntity> GetAsync(Guid id, bool asNoTracking = false)
        {
            var entity = asNoTracking ?
                await context.Set<TEntity>().AsNoTracking().SingleOrDefaultAsync(entity => entity.Id == id) :
                await context.Set<TEntity>().SingleOrDefaultAsync(entity => entity.Id == id);

            return entity ?? throw new ProblemDetailsException(HttpStatusCode.BadRequest,
                "No entity found", $"No entity with id '{id}' found.");
        }

        public async Task<TEntity?> SingleOrDefaultAsync(
            Expression<Func<TEntity, bool>>? predicate = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
            bool asNoTracking = false)
        {
            var query = GetEntitiesAsQueryable(include, asNoTracking);

            if (predicate != null)
            {
                return await query.SingleOrDefaultAsync(predicate);
            }

            return await query.SingleOrDefaultAsync();
        }

        public async Task<TEntity> SingleAsync(
            Expression<Func<TEntity, bool>>? predicate = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
            bool asNoTracking = false)
        {
            var query = GetEntitiesAsQueryable(include, asNoTracking);

            if (predicate != null)
            {
                return await query.SingleAsync(predicate);
            }

            return await query.SingleAsync();
        }

        public async Task<TEntity> FirstAsync(
            Expression<Func<TEntity, bool>>? predicate = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
            bool asNoTracking = false)
        {
            var query = GetEntitiesAsQueryable(include, asNoTracking);

            if (predicate != null)
            {
                return await query.FirstAsync(predicate);
            }

            return await query.FirstAsync();
        }

        public async Task<TEntity?> FirstOrDefaultAsync(
            Expression<Func<TEntity, bool>>? predicate = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
            bool asNoTracking = false)
        {
            var query = GetEntitiesAsQueryable(include, asNoTracking);

            if (predicate != null)
            {
                return await query.FirstOrDefaultAsync(predicate);
            }

            return await query.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null)
        {
            var query = context.Set<TEntity>().AsQueryable();

            if (include != null)
            {
                query = include(query);
            }

            return await query.ToListAsync();
        }

        private async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await context.Set<TEntity>().AsQueryable().AsNoTracking().AnyAsync(predicate);
        }

        public async Task<EntityEntry<TEntity>> AddAsync(TEntity entity)
        {
            entity.Created = DateTime.UtcNow.ToUtcDateTimeOffset();
            entity.CreatedBy = userService.GetCurrentUser()!.Id;
            entity.Modified = null;
            entity.ModifiedBy = null;
            entity.Version = Guid.NewGuid();

            return await context.Set<TEntity>().AddAsync(entity);
        }

        public async Task SaveChangesAsync() => await context.SaveChangesAsync();

        public async Task<TEntity> UpdateAsync(TEntity entity)
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
            entity.ModifiedBy = userService.GetCurrentUser()!.Id;
            entity.Version = Guid.NewGuid();

            context.Set<TEntity>().Update(entity);
            return entity;
        }

        public async Task RemoveAsync(Guid id)
        {
            var entity = await FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null)
            {
                throw new ProblemDetailsException(
                    HttpStatusCode.BadRequest, $"EntityAddress not found", $"No EntityAddress found with id '{id}'");
            }

            context.Set<TEntity>().Remove(entity);
        }

        private IQueryable<TEntity> GetEntitiesAsQueryable(Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include, bool asNoTracking)
        {
            var query = context.Set<TEntity>().AsQueryable();

            if (include != null)
            {
                query = include(query);
            }

            if (asNoTracking)
            {
                query.AsNoTracking();
            }

            return query;
        }
    }

}
