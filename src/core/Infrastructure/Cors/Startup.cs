using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Cors;

internal static class Startup
{
    private const string CorsPolicy = nameof(CorsPolicy);

    internal static IServiceCollection AddCorsPolicy(this IServiceCollection services, IConfiguration config)
    {
        var corsSettings = config.GetSection(nameof(CorsSettings)).Get<CorsSettings>();
        if (corsSettings == null) return services;
        var origins = new List<string>();
        if (corsSettings.Apis is not null)
            origins.AddRange(corsSettings.Apis.Split(';', StringSplitOptions.RemoveEmptyEntries));

        return services.AddCors(opt =>
            opt.AddPolicy(CorsPolicy, policy =>
                policy.AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials()
                    .WithOrigins(origins.ToArray())));
    }

    internal static IApplicationBuilder UseCorsPolicy(this IApplicationBuilder app)
    {
        return app.UseCors(CorsPolicy);
    }
}