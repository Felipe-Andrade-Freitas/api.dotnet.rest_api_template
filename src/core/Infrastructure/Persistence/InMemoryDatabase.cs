using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.Extensions.Caching.Memory;
using Namotion.Reflection;
using Shared.Exceptions;
using Shared.Extensions;

namespace Infrastructure.Persistence;

public class InMemoryDatabase
{
    private readonly ISerializerService _serializerService;
    private readonly IMemoryCache _memoryCache;

    public List<Product> Products { get; set; } = new List<Product>();

    public InMemoryDatabase(ISerializerService serializer, IMemoryCache memoryCache)
    {
        _serializerService = serializer;
        _memoryCache = memoryCache;
        LoadTables();
    }
    private void LoadTables()
    {
        Products = GetTable<Product>();
    }

    private List<T> GetTable<T>()
    {
        var tableName = typeof(T).GetTableName() ?? throw new CustomException(ErrorsMessages.InternalServerError,
            new List<string>
            {
                ErrorsMessages.NotFoundTable
            });
        return _memoryCache.GetOrCreate(tableName, _ =>
        {
            var filePath = $"{tableName}.json";
            if (!File.Exists($"/tmp/{filePath}")) File.WriteAllText($"/tmp/{filePath}", new List<T>().ToJson());
            var jsonFromFile = File.ReadAllText($"/tmp/{filePath}");
            return _serializerService.Deserialize<List<T>>(jsonFromFile) ?? new List<T>();
        }) ?? new List<T>();
    }

    public Task SaveChangesAsync<T>()
    {
        var tableName = typeof(T).GetTableName() ?? throw new CustomException(ErrorsMessages.InternalServerError,
            new List<string>
            {
                ErrorsMessages.NotFoundTable
            });
        var table = this.TryGetPropertyValue<List<T>>(tableName) ?? new List<T>();

        var filePath = $"{tableName}.json";
        File.WriteAllText($"/tmp/{filePath}", table.ToJson());
        return Task.CompletedTask;
    }
}