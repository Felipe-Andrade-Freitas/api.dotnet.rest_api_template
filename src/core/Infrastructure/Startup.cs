using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Infrastructure.Common;
using Infrastructure.Cors;
using Infrastructure.OpenApi;
using Infrastructure.Validations;
using MediatR;
using Infrastructure.Persistence;
using Infrastructure.Mapping;

namespace Infrastructure;

public static class Startup
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        return services
            .AddApiVersioning()
            .AddBehaviours()
            .AddHealthCheck()
            .AddMediatR(Assembly.GetExecutingAssembly())
            .AddOpenApiDocumentation(config)
            .AddCorsPolicy(config)
            .AddRouting(options => options.LowercaseUrls = true)
            .AddServices()
            .AddPersistence()
            .AddMapster()
            .AddMemoryCache();
    }

    private static IServiceCollection AddApiVersioning(this IServiceCollection services)
    {
        return services.AddApiVersioning(config =>
        {
            config.DefaultApiVersion = new ApiVersion(1, 0);
            config.AssumeDefaultVersionWhenUnspecified = true;
            config.ReportApiVersions = true;
        });
    }

    private static IServiceCollection AddHealthCheck(this IServiceCollection services)
    {
        return services.AddHealthChecks().Services;
    }

    public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder builder, IConfiguration config, IWebHostEnvironment env)
    {
        return builder
            .UseStaticFiles()
            .UseRouting()
            .UseCorsPolicy()
            .UseOpenApiDocumentation(config);
    }

    public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder builder)
    {
        builder.MapControllers();
        builder.MapHealthCheck();
        return builder;
    }

    private static void MapHealthCheck(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapHealthChecks("/api/health");
    }
}