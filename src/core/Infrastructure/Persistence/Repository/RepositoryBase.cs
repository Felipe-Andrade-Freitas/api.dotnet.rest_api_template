using Application.Common.Persistence;
using Domain.Common.Contracts;
using Namotion.Reflection;
using Shared.Exceptions;

namespace Infrastructure.Persistence.Repository;

public class RepositoryBase<T> : IRepository<T>  where T : class, IAggregateRoot
{
    private readonly string _tableName;
    private readonly List<T> _table;
    private readonly InMemoryDatabase _dbContext;

    public RepositoryBase(InMemoryDatabase dbContext)
    {
        _dbContext = dbContext;
        _tableName = typeof(T).GetTableName() ?? throw new CustomException(ErrorsMessages.InternalServerError,
            new List<string>
            {
                ErrorsMessages.NotFoundTable
            });
        _table = dbContext.TryGetPropertyValue<List<T>>(_tableName) ?? new List<T>();
    }


    public Task AddAsync(T entity, CancellationToken cancellationToken)
    {
        _table.Add(entity);
        return SaveChangesAsync();
    }

    public Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return Task.FromResult(_table.FirstOrDefault(v => v.TryGetPropertyValue<Guid>("Id") == id));
    }

    public Task UpdateAsync(T entity, CancellationToken cancellationToken)
    {
        var index = _table.FindIndex(v => v.TryGetPropertyValue<Guid>("Id") == entity.TryGetPropertyValue<Guid>("Id"));
        if (index >= 0)
        {
            _table[index] = entity;
        }

        return SaveChangesAsync();
    }

    private Task SaveChangesAsync()
    {
        _dbContext.GetType().GetProperty(_tableName)?.SetValue(_dbContext, _table);
        return _dbContext.SaveChangesAsync<T>();
    }

}