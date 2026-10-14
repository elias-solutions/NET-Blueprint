using System.Linq.Expressions;
using NET.Backend.Blueprint.Application.Exceptions;
using NET.Backend.Blueprint.Domain.Entities.Base;

namespace NET.Backend.Blueprint.Application.Persistence;

public interface IRepository<TEntity> where TEntity : EntityBase
{
    /// <exception cref="EntityNotFoundException">No entity with the given id exists.</exception>
    Task<TEntity> GetAsync(Guid id, CancellationToken cancellationToken);

    Task<TEntity?> FirstOrDefaultAsync(
        Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, object>>[] includes,
        CancellationToken cancellationToken);

    Task<IReadOnlyList<TEntity>> GetAllAsync(
        Expression<Func<TEntity, object>>[] includes,
        CancellationToken cancellationToken);

    Task AddAsync(TEntity entity, CancellationToken cancellationToken);

    /// <exception cref="VersionConflictException">The stored version differs from the entity version.</exception>
    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken);

    /// <exception cref="EntityNotFoundException">No entity with the given id exists.</exception>
    Task RemoveAsync(Guid id, CancellationToken cancellationToken);

    Task SaveChangesAsync(CancellationToken cancellationToken);
}
