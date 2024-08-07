using Application.Common.Interfaces;
using System.Text.Json;

namespace Infrastructure.Common.Services;

public class NewtonSoftService : ISerializerService
{
    public T? Deserialize<T>(string text)
    {
        return JsonSerializer.Deserialize<T>(text);
    }

    public string Serialize<T>(T obj)
    {
        return JsonSerializer.Serialize(obj, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        });
    }

    public string Serialize<T>(T obj, Type type)
    {
        return JsonSerializer.Serialize(obj, type, new JsonSerializerOptions());
    }
}