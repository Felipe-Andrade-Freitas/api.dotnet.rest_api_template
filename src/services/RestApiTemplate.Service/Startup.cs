using Application;
using Infrastructure;

namespace ProductService;

public sealed class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddInfrastructure(Configuration);
        services.AddApplication();

        services.Configure<RequestLocalizationOptions>(options =>
        {
            var supportedCultures = new[] { "pt-BR" };
            options.SetDefaultCulture(supportedCultures[0])
                .AddSupportedCultures(supportedCultures)
                .AddSupportedUICultures(supportedCultures);
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseInfrastructure(Configuration, env);

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapEndpoints();
        });
    }
}