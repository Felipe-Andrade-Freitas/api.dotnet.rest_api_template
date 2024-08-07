namespace ProductService.Configurations;

internal static class Startup
{
    internal static WebApplicationBuilder AddConfigurations(this WebApplicationBuilder builder)
    {
        const string configurationsDirectory = "Configurations";
        var env = builder.Environment;
        builder.Configuration.AddJsonFile("appsettings.json", false, true)
            .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true, true)
            .AddJsonFile($"{configurationsDirectory}/logger.json", false, true)
            .AddJsonFile($"{configurationsDirectory}/logger.{env.EnvironmentName}.json", true, true)
            .AddJsonFile($"{configurationsDirectory}/openapi.json", false, true)
            .AddJsonFile($"{configurationsDirectory}/openapi.{env.EnvironmentName}.json", true, true)
            .AddEnvironmentVariables();
        return builder;
    }
}