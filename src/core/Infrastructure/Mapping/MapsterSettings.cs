using Microsoft.Extensions.DependencyInjection;
using Mapster;

namespace Infrastructure.Mapping;

public static class MapsterSettings
{
    public static IServiceCollection AddMapster(this IServiceCollection services)
    {
        var typeAdapterConfig = TypeAdapterConfig.GlobalSettings;
        var applicationAssembly = typeof(Startup).Assembly;
        typeAdapterConfig.Scan(applicationAssembly);

        return services;
    }
}