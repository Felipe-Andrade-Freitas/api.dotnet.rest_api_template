using Domain.Common.Contracts;

namespace Application.Common.Persistence;

// The Repository for the Application Db
// I(Read)RepositoryBase<T> is from Ardalis.Specification

/// <summary>
///     The regular read/write repository for an aggregate root.
/// </summary>
public interface IRepository<T>
    where T : class, IEntity
{
    Task AddAsync(T entity, CancellationToken cancellationToken);
    Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task UpdateAsync(T entity, CancellationToken cancellationToken);
}