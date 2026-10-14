using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using NET.Backend.Blueprint.Application.Exceptions;
using NET.Backend.Blueprint.Application.Persistence;
using NET.Backend.Blueprint.Domain.Entities.Base;

namespace NET.Backend.Blueprint.Infrastructure.DataAccess;

public class Repository<TEntity>(BlueprintDbContext context) : IRepository<TEntity> where TEntity : EntityBase
{
    private DbSet<TEntity> Entities => context.Set<TEntity>();

    public async Task<TEntity> GetAsync(Guid id, CancellationToken cancellationToken)
    {
        return await Entities.SingleOrDefaultAsync(entity => entity.Id == id, cancellationToken)
               ?? throw new EntityNotFoundException(typeof(TEntity).Name, id);
    }

    public async Task<TEntity?> FirstOrDefaultAsync(
        Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, object>>[] includes,
        CancellationToken cancellationToken)
    {
        return await Query(includes).FirstOrDefaultAsync(predicate, cancellationToken);
    }

    public async Task<IReadOnlyList<TEntity>> GetAllAsync(
        Expression<Func<TEntity, object>>[] includes,
        CancellationToken cancellationToken)
    {
        return await Query(includes).AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken)
    {
        await Entities.AddAsync(entity, cancellationToken);
    }

    public async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken)
    {
        var hasVersionConflict = await Entities
            .AsNoTracking()
            .AnyAsync(e => e.Id == entity.Id && e.Version != entity.Version, cancellationToken);

        if (hasVersionConflict)
        {
            throw new VersionConflictException();
        }

        Entities.Update(entity);
    }

    public async Task RemoveAsync(Guid id, CancellationToken cancellationToken)
    {
        var entity = await Entities.FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
                     ?? throw new EntityNotFoundException(typeof(TEntity).Name, id);

        Entities.Remove(entity);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        return context.SaveChangesAsync(cancellationToken);
    }

    private IQueryable<TEntity> Query(Expression<Func<TEntity, object>>[] includes)
    {
        return includes.Aggregate<Expression<Func<TEntity, object>>, IQueryable<TEntity>>(
            Entities, (query, include) => query.Include(include));
    }
}
