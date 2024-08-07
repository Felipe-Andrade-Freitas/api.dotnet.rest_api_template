using Application;
using Infrastructure;
using Infrastructure.Common;
using Infrastructure.Logging.Serilog;
using ProductService.Configurations;
using Serilog;

StaticLogger.EnsureInitialized();
Log.Information("Server Booting Up...");
try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.AddConfigurations().RegisterSerilog();

    builder.Services.AddControllers();
    builder.Services.AddInfrastructure(builder.Configuration);
    builder.Services.AddApplication();

    var app = builder.Build();

    app.UseInfrastructure(builder.Configuration, builder.Environment);

    app.MapEndpoints();

    app.Run();
}
catch (Exception ex) when (!ex.GetType().Name.Equals("StopTheHostException", StringComparison.Ordinal))
{
    StaticLogger.EnsureInitialized();
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    StaticLogger.EnsureInitialized();
    Log.Information("Server Shutting down...");
    Log.CloseAndFlush();
}

namespace ProductService
{
    public class Program { }
}