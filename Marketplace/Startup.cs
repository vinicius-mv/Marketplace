using Marketplace.Application;
using Marketplace.Application.Contracts;
using Marketplace.Domain;
using Marketplace.Framework;
using Marketplace.Infrastructure;
using Microsoft.Extensions.Configuration;
using Raven.Client.Documents;

namespace Marketplace;

public class Startup
{
    private IConfiguration Configuration { get; }
    private IWebHostEnvironment Environment { get; }

    public Startup(IWebHostEnvironment environment, IConfiguration configuration)
    {
        Environment = environment;
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        // RavenDB configuration
        var store = new DocumentStore
        {
            Urls = new[] { "http://localhost:8080" },
            Database = "Marketplace",
            Conventions =
            {
                FindIdentityProperty = x => x.Name == "_databaseId"
            }
        };
        store.Initialize();

        // Dependency Injection configuration
        services.AddSingleton<ICurrencyLookup, FixedCurrencyLookup>();
        services.AddScoped(c => store.OpenAsyncSession());
        services.AddScoped<IUnitOfWork, RavenDbUnitOfWork>();
        services.AddScoped<IClassifiedAdRepository, ClassifiedAdRepository>();
        services.AddScoped<IApplicationService, ClassifiedAdsApplicationService>();

        services.AddControllers();

        // Swagger configuration
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1",
                new Microsoft.OpenApi.OpenApiInfo
                {
                    Title = "ClassifiedAds",
                    Version = "v1"
                });
        });
    }


    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseRouting();

        // Swagger middleware
        app.UseSwagger();
        app.UseSwaggerUI(c =>
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "ClassifiedAds v1"));

        // Endpoint routing for Web API controllers
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}