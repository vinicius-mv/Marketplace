using Marketplace.Contracts;
using Microsoft.Extensions.Configuration;

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
        services.AddControllers();

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