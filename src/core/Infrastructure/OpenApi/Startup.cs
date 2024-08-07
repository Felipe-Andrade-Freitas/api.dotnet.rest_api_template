using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json.Serialization;
using Microsoft.OpenApi.Models;

namespace Infrastructure.OpenApi;

internal static class Startup
{
    internal static IServiceCollection AddOpenApiDocumentation(this IServiceCollection services, IConfiguration config)
    {
        var settings = config.GetSection(nameof(SwaggerSettings)).Get<SwaggerSettings>();
        services.AddControllers().AddJsonOptions(options => { options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); });

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc(settings?.Version ?? "v1", new OpenApiInfo
            {
                Version = settings?.Version ?? "v1",
                Title = settings?.Title ?? "RestApiTemplate - API",
                Description = settings?.Description,
                Contact = new OpenApiContact
                {
                    Name = settings?.ContactName,
                    Email = settings?.ContactEmail,
                    Url = settings?.ContactUrl is not null ? new Uri(settings.ContactUrl ?? "") : null
                },
                License = new OpenApiLicense
                {
                    Name = settings?.LicenseName,
                    Url = settings?.LicenseUrl is not null ? new Uri(settings.LicenseUrl ?? "") : null
                }
            });
        });

        return services;
    }

    internal static IApplicationBuilder UseOpenApiDocumentation(this IApplicationBuilder app, IConfiguration config)
    {
        var settings = config.GetSection(nameof(SwaggerSettings)).Get<SwaggerSettings>();
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint($"/{settings?.EndpointPrefix ?? "dev"}/swagger/v1/swagger.json", settings?.Title ?? "RestApiTemplate - API");
            options.DocumentTitle = settings?.Title ?? "RestApiTemplate - API";
        });

        return app;
    }
}