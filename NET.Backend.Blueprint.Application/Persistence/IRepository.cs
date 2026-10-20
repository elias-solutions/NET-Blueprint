using System.Linq.Expressions;
using NET.Backend.Blueprint.Application.Exceptions;
using NET.Backend.Blueprint.Domain.Entities.Base;

namespace NET.Backend.Blueprint.Application.Persistence;

public interface IRepository<TEntity> where TEntity : EntityBase
{
    Task<TEntity> GetAsync(Guid id, CancellationToken cancellationToken);

    Task<TEntity?> FirstOrDefaultAsync(
        Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, object>>[] includes,
        CancellationToken cancellationToken);

    Task<IReadOnlyList<TEntity>> GetAllAsync(
        Expression<Func<TEntity, object>>[] includes,
        CancellationToken cancellationToken);

    Task AddAsync(TEntity entity, CancellationToken cancellationToken);

    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken);

    Task RemoveAsync(Guid id, CancellationToken cancellationToken);

    Task SaveChangesAsync(CancellationToken cancellationToken);
}
