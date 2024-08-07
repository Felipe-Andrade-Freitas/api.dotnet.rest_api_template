using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Exceptions;

public static class ModelExtensions
{
    public static string? GetTableName(this Type? type)
    {
        if (type == null) return null;
        var attr = type.GetCustomAttributes(typeof(TableAttribute), true).ToList();
        var collectionName = attr.Count > 0 ? ((TableAttribute)attr[0]).Name : string.Empty;
        return collectionName;
    }
}