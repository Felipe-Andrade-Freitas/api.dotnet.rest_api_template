using Microsoft.Extensions.DependencyInjection;
using Application.Common.Persistence;
using Infrastructure.Persistence.Repository;

namespace Infrastructure.Persistence;

public static class Startup
{
    public static IServiceCollection AddPersistence(this IServiceCollection services)
    {
        services.AddScoped(typeof(IRepository<>), typeof(RepositoryBase<>));
        services.AddScoped(typeof(InMemoryDatabase));
        return services;
    }
}